using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Runtime.InteropServices;
using Google.Cloud.Storage.Models.Extensions;
using Microsoft.Managed.winioctl.h;
using Microsoft.Native.fileapi.h;
using Microsoft.Native.kernel32.h;
using Microsoft.Native.nvme.h;
using Microsoft.Native.winioctl.h;
using Microsoft.Native.winnt.h;
using Microsoft.Win32.SafeHandles;

namespace Google.Cloud.Storage.Models
{
  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
  public class StorageDevice
  {
    // Append the drive number to this string for use with the CreateFile API.
    private const string PhysicalDrivePrefix = @"\\.\PHYSICALDRIVE";
    
    private static uint IOCTL_STORAGE_QUERY_PROPERTY = ControlCodes.IOCTL_STORAGE_QUERY_PROPERTY;

    public readonly string Id;
    private readonly SafeFileHandle _hDevice;
    
    public string PhysicalDrive => PhysicalDrivePrefix + Id;

    public StorageDevice(string id)
    {
      Id = id;
      _hDevice = OpenDrive(PhysicalDrive);
    }

    ~StorageDevice()
    {
      _hDevice.Close();
    }
    
    private void ThrowOnFailure(bool ok)
    {
      if (!ok)
      {
        throw new Win32Exception(Marshal.GetLastWin32Error(),
          $"DeviceIoControl({PhysicalDrive}) failed.");
      }
    }

    public STORAGE_ADAPTER_DESCRIPTOR GetStorageAdapterDescriptor()
    {
      return IssueIoctl<STORAGE_ADAPTER_DESCRIPTOR>(STORAGE_PROPERTY_ID.StorageAdapterProperty);
    }
    
    public StorageDeviceDescriptor GetStorageDeviceDescriptor()
    {
      return IssueIoctl(
        STORAGE_PROPERTY_ID.StorageDeviceProperty,
        bufferSize: 1024,
        marshaller: buffer =>
        {
          STORAGE_DEVICE_DESCRIPTOR descriptor = Marshal
            .PtrToStructure<STORAGE_DEVICE_DESCRIPTOR>(buffer);

          byte[] structBytes = new byte[descriptor.Size];
          Marshal.Copy(
            source: buffer,
            destination: structBytes,
            startIndex: 0,
            length: (int)descriptor.Size);
        
          return new StorageDeviceDescriptor
          {
            Version = descriptor.Version,
            Size = descriptor.Size,
            DeviceType = descriptor.DeviceType,
            DeviceTypeModifier = descriptor.DeviceTypeModifier,
            RemovableMedia = descriptor.RemovableMedia,
            CommandQueueing = descriptor.CommandQueueing,
            VendorId = descriptor.VendorIdOffset == 0 ?
              string.Empty :
              structBytes.ToAsciiString(descriptor.VendorIdOffset),
            ProductId = descriptor.ProductIdOffset == 0 ?
              string.Empty :
              structBytes.ToAsciiString(descriptor.ProductIdOffset),
            ProductRevision = descriptor.ProductRevisionOffset == 0 ?
              string.Empty :
              structBytes.ToAsciiString(descriptor.ProductRevisionOffset),
            SerialNumber = descriptor.SerialNumberOffset == 0 ?
              string.Empty :
              structBytes.ToAsciiString(descriptor.SerialNumberOffset),
            BusType = descriptor.BusType,
            RawPropertiesLength = descriptor.RawPropertiesLength
          };
        });
    }

    public StorageDeviceIdDescriptor GetStorageDeviceIdDescriptor()
    {
      return IssueIoctl(
        STORAGE_PROPERTY_ID.StorageDeviceIdProperty,
        bufferSize: 1024,
        marshaller: buffer =>
        {
          STORAGE_DEVICE_ID_DESCRIPTOR storageDeviceIdDescriptor = Marshal
            .PtrToStructure<STORAGE_DEVICE_ID_DESCRIPTOR>(buffer);

          int identifiersOffset = Marshal
            .OffsetOf<STORAGE_DEVICE_ID_DESCRIPTOR>(nameof(STORAGE_DEVICE_ID_DESCRIPTOR.Identifiers))
            .ToInt32();

          return new StorageDeviceIdDescriptor
          {
            Version = storageDeviceIdDescriptor.Version,
            Size = storageDeviceIdDescriptor.Size,
            NumberOfIdentifiers = storageDeviceIdDescriptor.NumberOfIdentifiers,
            Identifiers = GetStorageIdentifiers(
              buffer: buffer + identifiersOffset,
              numIdentifiers: storageDeviceIdDescriptor.NumberOfIdentifiers)
          };
        });
    }

    public StorageIdentifier[] GetStorageIdentifiers(IntPtr buffer, uint numIdentifiers)
    {
      StorageIdentifier[] results = new StorageIdentifier[numIdentifiers];
      
      // TODO(seanrmilligan): The bufferSize is assumed to be sufficient.
      // We do not have guardrails to stop us from reading past the end.
      IntPtr ptr = buffer;
      
      for (uint i = 0; i < numIdentifiers; i++)
      {
        STORAGE_IDENTIFIER storageIdentifier = Marshal
          .PtrToStructure<STORAGE_IDENTIFIER>(ptr);
        int identifierOffset = Marshal
          .OffsetOf<STORAGE_IDENTIFIER>(nameof(STORAGE_IDENTIFIER.Identifier))
          .ToInt32();
        
        byte[] identifier = new byte[storageIdentifier.IdentifierSize];
        Marshal.Copy(
          source: ptr + identifierOffset,
          destination: identifier,
          startIndex: 0,
          length: storageIdentifier.IdentifierSize);

        results[i] = new StorageIdentifier
        {
          CodeSet = storageIdentifier.CodeSet,
          Type = storageIdentifier.Type,
          IdentifierSize = storageIdentifier.IdentifierSize,
          NextOffset = storageIdentifier.NextOffset,
          Association = storageIdentifier.Association,
          Identifier = identifier
        };

        ptr += storageIdentifier.NextOffset;
      }

      return results;
    }
    
    public STORAGE_BUS_TYPE GetBusType()
    {
      StorageDeviceDescriptor descriptor = GetStorageDeviceDescriptor();
      return descriptor.BusType;
    }

    public int[] NvmeIdentifyActiveNamespaces()
    {
      Page page = NvmeIdentify<Page>(
        STORAGE_PROPERTY_ID.StorageDeviceProtocolSpecificProperty,
        NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_ACTIVE_NAMESPACES,
        0);

      Console.WriteLine(page.Bytes.ToHexString(separator: " "));
      return page.Bytes.ToInt32Array();
    }

    public NVME_IDENTIFY_CONTROLLER_DATA NvmeIdentifyController()
    {
      return NvmeIdentify<NVME_IDENTIFY_CONTROLLER_DATA>(
        STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty,
        NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_CONTROLLER,
        0);
    }
    
    public NVME_IDENTIFY_NAMESPACE_DATA NvmeIdentifySpecificNamespace(uint namespaceId)
    {
      return NvmeIdentify<NVME_IDENTIFY_NAMESPACE_DATA>(
        STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty,
        NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_SPECIFIC_NAMESPACE,
        namespaceId);
    }

    private T NvmeIdentify<T>(STORAGE_PROPERTY_ID propertyId, NVME_IDENTIFY_CNS_CODES identifyCode, uint subValue)
    {
      // STORAGE_PROTOCOL_SPECIFIC_DATA forms the AdditionalParameters field of
      // the STORAGE_PROPERTY_QUERY, so we find where AdditionalParameters is in
      // order to write protocolSpecificData to memory at that address.
      int additionalParametersOffset = Marshal
        .OffsetOf<STORAGE_PROPERTY_QUERY>(nameof(STORAGE_PROPERTY_QUERY.AdditionalParameters))
        .ToInt32();
      
      // The bufferSize is the sum of:
      // - the first N bytes of the STORAGE_PROPERTY_QUERY, up to but not
      //   including the AdditionalParameters field
      // - the size of AdditionalParameters, which varies but in this case
      //   contains STORAGE_PROTOCOL_SPECIFIC_DATA
      // - the maximum size of the response payload, NVME_MAX_LOG_SIZE
      int bufferSize = additionalParametersOffset 
                       + Marshal.SizeOf(typeof(STORAGE_PROTOCOL_SPECIFIC_DATA))
                       + Constants.NVME_MAX_LOG_SIZE;
      
      // Alloc then clear the buffer
      IntPtr ptr = Marshal.AllocHGlobal(bufferSize);
      // Copying a new managed byte array into the buffer clears it
      Marshal.Copy(new byte[bufferSize], 0, ptr, bufferSize);
      
      STORAGE_PROPERTY_QUERY query = new()
      {
        PropertyId = propertyId,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      STORAGE_PROTOCOL_SPECIFIC_DATA protocolSpecificData = new()
      {
        ProtocolType = STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme,
        DataType = (uint)STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeIdentify,
        ProtocolDataRequestValue = (uint)identifyCode,
        ProtocolDataRequestSubValue = subValue,
        ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(STORAGE_PROTOCOL_SPECIFIC_DATA)),
        ProtocolDataLength = Constants.NVME_MAX_LOG_SIZE
      };
      
      // write our query to the allocated memory
      Marshal.StructureToPtr(query, ptr, true);
      Marshal.StructureToPtr(protocolSpecificData, ptr + additionalParametersOffset, true);
      
      uint written = 0;
      bool ok = IOApiSet.DeviceIoControl(
        hDevice: _hDevice,
        dwIoControlCode: IOCTL_STORAGE_QUERY_PROPERTY,
        lpInBuffer: ptr,
        nInBufferSize: (uint)bufferSize,
        lpOutBuffer: ptr,
        nOutBufferSize: bufferSize,
        lpBytesReturned: ref written,
        lpOverlapped: IntPtr.Zero);

      T? result = default;

      if (ok)
      {
        // read the response back from the same memory (the query was overwritten)
        STORAGE_PROTOCOL_DATA_DESCRIPTOR dataDescriptor = Marshal
          .PtrToStructure<STORAGE_PROTOCOL_DATA_DESCRIPTOR>(ptr);

        long identifyOffset = Marshal
          .OffsetOf<STORAGE_PROTOCOL_DATA_DESCRIPTOR>(nameof(STORAGE_PROTOCOL_DATA_DESCRIPTOR.ProtocolSpecificData))
          .ToInt32()
          + dataDescriptor.ProtocolSpecificData.ProtocolDataOffset;

        result = Marshal.PtrToStructure<T>(ptr + (int)identifyOffset);
      }
      
      Marshal.FreeHGlobal(ptr);
      
      ThrowOnFailure(ok);
      
      return result!;
    }

    private TResult IssueIoctl<TResult>(
      STORAGE_PROPERTY_ID propertyId,
      int bufferSize = 0,
      Func<IntPtr, TResult>? marshaller = null
    )
    {
      return IssueIoctl<TResult, byte>(
        propertyId, default, bufferSize, marshaller);
    }
    
    private TResult IssueIoctl<TResult, TAdditionalParameters>(
      STORAGE_PROPERTY_ID propertyId,
      TAdditionalParameters? additionalParameters = default,
      int bufferSize = 0,
      Func<IntPtr, TResult>? marshaller = null
    ) where TAdditionalParameters : struct
    {
      if (bufferSize == 0)
      {
        bufferSize = Math.Max(
          Marshal.SizeOf(typeof(STORAGE_PROPERTY_QUERY)),
          Marshal.SizeOf(typeof(TResult)));
      }
      
      IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(new byte[bufferSize], 0, buffer, bufferSize);
      
      Marshal.StructureToPtr(structure: new STORAGE_PROPERTY_QUERY
      {
        PropertyId = propertyId,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      }, ptr: buffer, fDeleteOld: true);
      
      if (!object.Equals(additionalParameters, default))
      {
        // STORAGE_PROTOCOL_SPECIFIC_DATA forms the AdditionalParameters field of
        // the STORAGE_PROPERTY_QUERY, so we find where AdditionalParameters is in
        // order to write protocolSpecificData to memory at that address.
        int additionalParametersOffset = Marshal
          .OffsetOf<STORAGE_PROPERTY_QUERY>(
            nameof(STORAGE_PROPERTY_QUERY.AdditionalParameters))
          .ToInt32();
        Marshal.StructureToPtr(structure: additionalParameters,
          ptr: buffer + additionalParametersOffset, fDeleteOld: true);
      }
      
      uint written = 0;
      bool ok = IOApiSet.DeviceIoControl(
        hDevice: _hDevice,
        dwIoControlCode: IOCTL_STORAGE_QUERY_PROPERTY,
        lpInBuffer: buffer,
        nInBufferSize: (uint)bufferSize,
        lpOutBuffer: buffer,
        nOutBufferSize: bufferSize,
        lpBytesReturned: ref written,
        lpOverlapped: IntPtr.Zero);

      if (!ok)
      {
        Marshal.FreeHGlobal(buffer);
        throw new Win32Exception(Marshal.GetLastWin32Error());
      }
      
      TResult result = marshaller != null ?
        marshaller.Invoke(buffer) :
        Marshal.PtrToStructure<TResult>(buffer)!;
        
      Marshal.FreeHGlobal(buffer);
      return result;
    }
    
    private static SafeFileHandle OpenDrive(string physicalDrive)
    {
      SafeFileHandle hDevice = FileApi.CreateFile(physicalDrive,
        dwDesiredAccess: (uint)ACCESS_TYPES.GENERIC_READ | (uint)ACCESS_TYPES.GENERIC_WRITE,
        dwShareMode: (uint)FILE_SHARE.READ,
        lpSecurityAttributes: IntPtr.Zero,
        dwCreationDisposition: (uint)CreateDisposition.OPEN_EXISTING,
        dwFlagsAndAttributes: 0,
        hTemplateFile: IntPtr.Zero
      );
      
      if (hDevice.IsInvalid)
      {
        throw new Win32Exception(Marshal.GetLastWin32Error(),
          message: $"CreateFile({physicalDrive}) failed. " + 
          "Is the drive number valid? Confirm physical drive number with Get-PhysicalDisk");
      }

      return hDevice;
    }

    /// <summary>
    /// Returns a list of the device Ids of all of the physical disks attached
    /// to the system. This is equivalent to running
    /// `(Get-PhysicalDisk).deviceId` in PowerShell.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetAllPhysicalDeviceIds()
    {
      IEnumerable<string> physicalDrives;

      WqlObjectQuery query = new("SELECT * FROM Win32_DiskDrive");
      using (ManagementObjectSearcher searcher = new(query))
      {
        physicalDrives = Enumerable.Where<string?>(searcher.Get()
            .OfType<ManagementObject>()
            .Select(o => o.Properties["DeviceID"].Value.ToString()), id => !string.IsNullOrEmpty(id))!;
      }
      
      // Strip off '\\.\PHYSICALDRIVE' prefix
      return physicalDrives.Select(drive => drive.Substring(PhysicalDrivePrefix.Length));
    }
  }
}
