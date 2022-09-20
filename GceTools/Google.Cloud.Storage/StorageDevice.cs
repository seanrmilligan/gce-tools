using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Google.Cloud.Storage.Extensions;
using Google.Cloud.Storage.Models;
using Microsoft.Win32.SafeHandles;
using Microsoft.Windows.fileapi.h;
using Microsoft.Windows.nvme.h;
using Microsoft.Windows.winioctl.h;
using Microsoft.Windows.winnt.h;
using LPSECURITY_ATTRIBUTES = System.IntPtr;
using LPOVERLAPPED = System.IntPtr;
using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using LPCTSTR = System.String;

using static Microsoft.Windows.kernel32.h.IOApiSet;
using static Microsoft.Windows.kernel32.h.FileApi;

namespace Google.Cloud.Storage
{
  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
  public class StorageDevice
  {
    /// <summary>
    /// The SCSI query that we execute below returns a string for the disk name
    /// that includes this prefix plus the PD name that we care about.
    /// </summary>
    private const string GoogleScsiPrefix = "Google  ";
    
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

    public STORAGE_ADAPTER_DESCRIPTOR GetAdapterDescriptor()
    {
      STORAGE_PROPERTY_QUERY query = new()
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      
      var result = default(STORAGE_ADAPTER_DESCRIPTOR);
      uint written = 0;
      
      bool ok = DeviceIoControl(
        hDevice: _hDevice,
        dwIoControlCode: IOCTL_STORAGE_QUERY_PROPERTY,
        lpInBuffer: ref query,
        nInBufferSize: (uint)Marshal.SizeOf(query),
        lpOutBuffer: out result,
        nOutBufferSize: Marshal.SizeOf(result),
        lpBytesReturned: ref written,
        lpOverlapped: IntPtr.Zero);
      ThrowOnFailure(ok);
      
      return result;
    }
    
    public StorageDeviceDescriptor GetDeviceDescriptor()
    {
      // determine an adequate buffer size
      int bufferSize = 1024;
      
      // calloc some memory
      IntPtr ptr = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(new byte[bufferSize], 0, ptr, bufferSize);
      
      // write our query to the allocated memory
      Marshal.StructureToPtr(structure: new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      }, ptr: ptr, fDeleteOld: true);
      
      // issue the query
      uint written = 0;
      bool ok = DeviceIoControl(
        hDevice: _hDevice,
        dwIoControlCode: IOCTL_STORAGE_QUERY_PROPERTY,
        lpInBuffer: ptr,
        nInBufferSize: (uint)bufferSize,
        lpOutBuffer: ptr,
        nOutBufferSize: bufferSize,
        lpBytesReturned: ref written,
        lpOverlapped: IntPtr.Zero);

      StorageDeviceDescriptor? result = default;
      
      if (ok)
      {
        STORAGE_DEVICE_DESCRIPTOR descriptor = Marshal
          .PtrToStructure<STORAGE_DEVICE_DESCRIPTOR>(ptr);

        byte[] buffer = new byte[descriptor.Size];
        Marshal.Copy(
          source: ptr,
          destination: buffer,
          startIndex: 0,
          length: (int)descriptor.Size);
        
        result = new StorageDeviceDescriptor
        {
          Version = descriptor.Version,
          Size = descriptor.Size,
          DeviceType = descriptor.DeviceType,
          DeviceTypeModifier = descriptor.DeviceTypeModifier,
          RemovableMedia = descriptor.RemovableMedia,
          CommandQueueing = descriptor.CommandQueueing,
          VendorId = descriptor.VendorIdOffset == 0 ?
            string.Empty :
            buffer.ToAsciiString(descriptor.VendorIdOffset),
          ProductId = descriptor.ProductIdOffset == 0 ?
            string.Empty :
            buffer.ToAsciiString(descriptor.ProductIdOffset),
          ProductRevision = descriptor.ProductRevisionOffset == 0 ?
            string.Empty :
            buffer.ToAsciiString(descriptor.ProductRevisionOffset),
          SerialNumber = descriptor.SerialNumberOffset == 0 ?
            string.Empty :
            buffer.ToAsciiString(descriptor.SerialNumberOffset),
          BusType = descriptor.BusType,
          RawPropertiesLength = descriptor.RawPropertiesLength
        };
      }
      
      Marshal.FreeHGlobal(ptr);
      
      ThrowOnFailure(ok);
      
      return result!;
    }

    public StorageDeviceIdDescriptor GetDeviceIdDescriptor()
    {
      // determine an adequate buffer size
      int bufferSize = 1024;
      
      // calloc some memory
      IntPtr ptr = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(new byte[bufferSize], 0, ptr, bufferSize);
      
      // write our query to the allocated memory
      Marshal.StructureToPtr(new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceIdProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      }, ptr, true);
      
      // issue the query
      uint written = 0;
      bool ok = DeviceIoControl(
        hDevice: _hDevice,
        dwIoControlCode: IOCTL_STORAGE_QUERY_PROPERTY,
        lpInBuffer: ptr,
        nInBufferSize: (uint)bufferSize,
        lpOutBuffer: ptr,
        nOutBufferSize: bufferSize,
        lpBytesReturned: ref written,
        lpOverlapped: IntPtr.Zero);

      StorageDeviceIdDescriptor? result = default;

      if (ok)
      {
        STORAGE_DEVICE_ID_DESCRIPTOR storageDeviceIdDescriptor = Marshal
          .PtrToStructure<STORAGE_DEVICE_ID_DESCRIPTOR>(ptr);
        
        int identifiersOffset = Marshal
          .OffsetOf<STORAGE_DEVICE_ID_DESCRIPTOR>(nameof(STORAGE_DEVICE_ID_DESCRIPTOR.Identifiers))
          .ToInt32();
        
        result = new StorageDeviceIdDescriptor
        {
          Version = storageDeviceIdDescriptor.Version,
          Size = storageDeviceIdDescriptor.Size,
          NumberOfIdentifiers = storageDeviceIdDescriptor.NumberOfIdentifiers,
          Identifiers = GetStorageIdentifiers(
            buffer: ptr + identifiersOffset,
            numIdentifiers: storageDeviceIdDescriptor.NumberOfIdentifiers)
        };
      }
      
      Marshal.FreeHGlobal(ptr);
      
      ThrowOnFailure(ok);

      return result!;
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
      StorageDeviceDescriptor descriptor = GetDeviceDescriptor();
      return descriptor.BusType;
    }
    
    public string GetScsiPersistentDiskName()
    {
      StorageDeviceIdDescriptor deviceIdDescriptor = GetDeviceIdDescriptor();

      string persistentDiskName = deviceIdDescriptor
        .Identifiers
        .Where(storageIdentifier =>
          storageIdentifier.Type == STORAGE_IDENTIFIER_TYPE.StorageIdTypeVendorId &&
          storageIdentifier.Association == STORAGE_ASSOCIATION_TYPE.StorageIdAssocDevice)
        .Select(storageIdentifier => Encoding.ASCII.GetString(storageIdentifier.Identifier))
        .FirstOrDefault(name => name.StartsWith(GoogleScsiPrefix))
        ?.Substring(GoogleScsiPrefix.Length) ?? throw new InvalidOperationException(
        message: $"DeviceId {Id} is not a GCE PD");

      return persistentDiskName;
    }

    public string GetDeviceName()
    {
      return GetBusType() switch
      {
        STORAGE_BUS_TYPE.BusTypeNvme => GetNvmeDeviceName(),
        STORAGE_BUS_TYPE.BusTypeScsi => GetScsiPersistentDiskName(),
        _ => string.Empty
      };
    }

    public string GetNvmeDeviceName()
    {
      // Google writes NVMe vendor-specific data as a JSON string to the start
      // of the vendor-specific section of the NVMe Identify Specific Namespace
      // page.
      string jsonString = Encoding.UTF8.GetString(bytes:
        NvmeIdentifySpecificNamespace(1)
          .VS
          .TakeWhile(b => b != 0)
          .ToArray());
      return string.Empty;
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

    public T NvmeIdentify<T>(STORAGE_PROPERTY_ID propertyId, NVME_IDENTIFY_CNS_CODES identifyCode, uint subValue)
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
      bool ok = DeviceIoControl(
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

    private static SafeFileHandle OpenDrive(string physicalDrive)
    {
      SafeFileHandle hDevice = CreateFile(physicalDrive,
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
        physicalDrives = searcher.Get()
          .OfType<ManagementObject>()
          .Select(o => o.Properties["DeviceID"].Value.ToString())
          .Where(id => !string.IsNullOrEmpty(id))!;
      }
      
      // Strip off '\\.\PHYSICALDRIVE' prefix
      return physicalDrives.Select(drive => drive.Substring(PhysicalDrivePrefix.Length));
    }
  }
}
