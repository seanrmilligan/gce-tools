using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Google.Cloud.Storage.ManagedModels;
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
    private readonly bool _verbose;
    
    public string PhysicalDrive => PhysicalDrivePrefix + Id;

    public StorageDevice(string id, bool verbose = false)
    {
      Id = id;
      _verbose = verbose;
      _hDevice = OpenDrive(PhysicalDrive);
    }

    ~StorageDevice()
    {
      _hDevice.Close();
    }
    
    private void WriteDebugLine(string line)
    {
      if (_verbose)
      {
        Console.Error.WriteLine(line);
      }
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
    
    public STORAGE_DEVICE_DESCRIPTOR GetDeviceDescriptor()
    {
      var query = new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      
      var result = default(STORAGE_DEVICE_DESCRIPTOR);
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

    public StorageDeviceIdDescriptor GetDeviceIdDescriptor()
    {
      int bufferSize = 1024;
      
      IntPtr ptr = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(new byte[bufferSize], 0, ptr, bufferSize);
      
      STORAGE_PROPERTY_QUERY query = new()
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceIdProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      
      // write our query to the allocated memory
      Marshal.StructureToPtr(query, ptr, true);
      
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

      StorageDeviceIdDescriptor result = default;

      if (ok)
      {
        STORAGE_DEVICE_ID_DESCRIPTOR storageDeviceIdDescriptor = Marshal
          .PtrToStructure<STORAGE_DEVICE_ID_DESCRIPTOR>(ptr);
        
        int identifiersOffset = Marshal
          .OffsetOf<STORAGE_DEVICE_ID_DESCRIPTOR>(nameof(STORAGE_DEVICE_ID_DESCRIPTOR.Identifiers))
          .ToInt32();

        StorageIdentifier[] identifiers = GetStorageIdentifiers(
          buffer: ptr + identifiersOffset,
          numIdentifiers: storageDeviceIdDescriptor.NumberOfIdentifiers);

        result = new StorageDeviceIdDescriptor()
        {
          Version = storageDeviceIdDescriptor.Version,
          Size = storageDeviceIdDescriptor.Size,
          NumberOfIdentifiers = storageDeviceIdDescriptor.NumberOfIdentifiers,
          Identifiers = identifiers
        };
      }
      
      Marshal.FreeHGlobal(ptr);
      
      ThrowOnFailure(ok);

      return result;
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
      STORAGE_DEVICE_DESCRIPTOR descriptor = GetDeviceDescriptor();
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

    public NVME_IDENTIFY_NAMESPACE_DATA NvmeIdentifySpecificNamespace(uint namespaceId)
    {
      return NvmeIdentify<NVME_IDENTIFY_NAMESPACE_DATA>(
        NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_SPECIFIC_NAMESPACE,
        namespaceId);
    }

    public T NvmeIdentify<T>(NVME_IDENTIFY_CNS_CODES identifyCode, uint subValue)
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
      
      IntPtr ptr = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(new byte[bufferSize], 0, ptr, bufferSize);
      
      STORAGE_PROPERTY_QUERY query = new()
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty,
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
