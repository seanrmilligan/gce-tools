using System;
using System.Runtime.InteropServices;  // for DllImport, Marshal
using System.ComponentModel;  // for Win32Exception

// https://github.com/DKorablin/DeviceIoControl
using AlphaOmega.Debug;
using Google.Cloud.Storage.Windows.nvme;
using Google.Cloud.Storage.Windows.winioctl;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Linq;
// Make sure that the Visual Studio project "references" System.Management:
// right-click on References in the Solution Explorer, Add Reference, search
// for System.Management and check the box.
// https://stackoverflow.com/a/11660206/1230197
using System.Management;
using Newtonsoft.Json; // for WqlObjectQuery

using LPSECURITY_ATTRIBUTES = System.IntPtr;
using LPOVERLAPPED = System.IntPtr;
using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using LPCTSTR = System.String;

using static Google.Cloud.Storage.Windows.kernel32.IOApiSet;
using static Google.Cloud.Storage.Windows.kernel32.FileApi;

namespace Google.Cloud.Storage
{
  public class StorageDevice
  {
    /// <summary>
    /// The SCSI query that we execute below returns a string for the disk name
    /// that includes this prefix plus the PD name that we care about.
    /// </summary>
    private const string GoogleScsiPrefix = "Google  ";
    
    // Append the drive number to this string for use with the CreateFile API.
    private const string PhysicalDrivePrefix = @"\\.\PHYSICALDRIVE";
    
    //TODO: substantiate with citation
    private const uint NvmeMaxLogSize = 4096;

    public readonly string Id;
    private readonly SafeFileHandle _hDevice;
    private readonly bool _verbose;
    
    private string PhysicalDrive => PhysicalDrivePrefix + Id;

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

    // Copied from C:\Program Files (x86)\Windows Kits\10\Include\10.0.17763.0\um\winioctl.h
    private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
    {
      return (deviceType << 16) | (access << 14) | (function << 2) | method;
    }
    
    private const uint METHOD_BUFFERED = 0;
    private const uint FILE_ANY_ACCESS = 0;
    private const uint FILE_DEVICE_MASS_STORAGE = 0x0000002d;
    private const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;
    private static readonly uint IOCTL_STORAGE_QUERY_PROPERTY = CTL_CODE(
        IOCTL_STORAGE_BASE, 0x0500, METHOD_BUFFERED, FILE_ANY_ACCESS);

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
      // https://stackoverflow.com/a/17354960/1230197
      var query = new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      // https://stackoverflow.com/a/2069456/1230197
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
      // https://stackoverflow.com/a/17354960/1230197
      var query = new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      // https://stackoverflow.com/a/2069456/1230197
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
    
    
    public string GetBusType()
    {
      STORAGE_DEVICE_DESCRIPTOR descriptor = GetDeviceDescriptor();
      STORAGE_BUS_TYPE busType = descriptor.BusType;
      return busType.ToString();
    }
    // deviceId is the physical disk number, e.g. from Get-PhysicalDisk. If the
    // device is determined to be a GCE PD then its name will be returned.
    //
    // Throws:
    //   System.ComponentModel.Win32Exception: if we could not open a handle for
    //     the device (probably because deviceId is invalid)
    //   InvalidOperationException: if the device is not a GCE PD.
    public string Get_GcePdName(string deviceId)
    {
      // https://stackoverflow.com/a/17354960/1230197
      var query = new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageDeviceIdProperty,  // page 83
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery
      };
      var qsize = (uint)Marshal.SizeOf(query);
      // https://stackoverflow.com/a/2069456/1230197
      var result = default(STORAGE_DEVICE_ID_DESCRIPTOR);
      var rsize = Marshal.SizeOf(result);
      uint written = 0;
      bool ok = DeviceIoControl(_hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
                   ref query, qsize, out result, rsize, ref written,
                   IntPtr.Zero);
      ThrowOnFailure(ok);

      uint numIdentifiers = result.NumberOfIdentifiers;
      WriteDebugLine($"numIdentifiers: {numIdentifiers}");

      int identifierBufferStart = 0;
      for (int i = 0; i < numIdentifiers; ++i)
      {
        // Example:
        // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.copy?view=netframework-4.7.2#System_Runtime_InteropServices_Marshal_Copy_System_Byte___System_Int32_System_IntPtr_System_Int32_.
        // We don't know exactly how large this identifier is until we marshal
        // the struct below. "BUFFER_SIZE" is used by
        // STORAGE_DEVICE_ID_DESCRIPTOR for the combined size of all the
        // identifiers so it's an upper bound on the size of this one.
        IntPtr storageIdentifierBuffer =
          Marshal.AllocHGlobal(512);
        int identifiersBufferLeft =
          512 - identifierBufferStart;
        WriteDebugLine(
          $"getting storageIdentifier {i} from memory [{identifierBufferStart}, {identifierBufferStart + identifiersBufferLeft})");
        Marshal.Copy(result.Identifiers, identifierBufferStart,
            storageIdentifierBuffer, identifiersBufferLeft);

        var storageIdentifier =
          Marshal.PtrToStructure<STORAGE_IDENTIFIER>(
            storageIdentifierBuffer);

        WriteDebugLine($"storageIdentifier type: {storageIdentifier.Type} ({(int)storageIdentifier.Type})");
        WriteDebugLine(
          $"storageIdentifier association: {storageIdentifier.Association} ({(int)storageIdentifier.Association})");
        WriteDebugLine($"storageIdentifier size: {(int)storageIdentifier.IdentifierSize}");

        if (storageIdentifier.Type == STORAGE_IDENTIFIER_TYPE.StorageIdTypeVendorId &&
            storageIdentifier.Association == STORAGE_ASSOCIATION_TYPE.StorageIdAssocDevice)
        {
          IntPtr identifierData =
            Marshal.AllocHGlobal(storageIdentifier.IdentifierSize + 1);
          Marshal.Copy(storageIdentifier.Identifier, 0,
            identifierData, storageIdentifier.IdentifierSize);

          // Empirically the SCSI identifier for GCE PDs always begins with
          // "Google". Physical disk objects passed to this function may
          // represent storage devices other than PDs though - for example,
          // Docker containers that are running have a "Msft Virtual Disk"
          // associated with them (why this is considered a PhysicalDisk, I have
          // no idea...). Therefore we check for the presence of the Google
          // prefix and throw an exception if it's not found.
          //
          // TODO(pjh): make this more robust? Not sure if the Google prefix is
          // guaranteed or subject to change in the future.
          string fullName = System.Text.Encoding.ASCII.GetString(
            storageIdentifier.Identifier, 0, storageIdentifier.IdentifierSize);
          if (!fullName.StartsWith(GoogleScsiPrefix))
          {
            var e = new InvalidOperationException(
              $"deviceId {deviceId} maps to {fullName} which is not a GCE PD");
            WriteDebugLine($"{e}");
            throw e;
          }
          return fullName.Substring(GoogleScsiPrefix.Length);
        }

        // To get the start of the next identifier we need to advance
        // by the length of the STORAGE_IDENTIFIER struct as well as
        // the size of its variable-length data array. We subtract
        // the size of the "byte[] Identifiers" member because it's
        // included in the size of the data array.
        //
        // TODO(pjh): figure out how to make this more robust.
        // Marshal.SizeOf(storageIdentifier) returns bonkers
        // values when we set the SizeConst MarshalAs attribute (which
        // we need to do in order to copy from the byte[] above). I
        // couldn't figure out how to correctly calculate this value
        // using Marshal.SizeOf, but it's 20 bytes - this value is
        // fixed (for this platform at least) by the definition of
        // STORAGE_IDENTIFIER in winioctl.h.
        int advanceBy = 20 - sizeof(byte) + storageIdentifier.IdentifierSize;
        WriteDebugLine(
          $"advanceBy = {20} - {sizeof(byte)} + {storageIdentifier.IdentifierSize} = {advanceBy}");
        identifierBufferStart += advanceBy;
        WriteDebugLine($"will read next identifier starting at {identifierBufferStart}");
        WriteDebugLine("");
        Marshal.FreeHGlobal(storageIdentifierBuffer);
      }
      
      return null;
    }

    public string Get_GcePdName_Nvme()
    {
      var protocolSpecificData = default(STORAGE_PROTOCOL_SPECIFIC_DATA);
      protocolSpecificData = new STORAGE_PROTOCOL_SPECIFIC_DATA
      {
        ProtocolType = STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme,
        DataType = (uint)STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeIdentify,
        ProtocolDataRequestValue = (uint)NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_CONTROLLER,
        ProtocolDataRequestSubValue = 0,
        ProtocolDataOffset = (uint)Marshal.SizeOf(protocolSpecificData),
        ProtocolDataLength = NvmeMaxLogSize
      };
      
      // https://stackoverflow.com/a/17354960/1230197
      var query = new STORAGE_PROPERTY_QUERY
      {
        PropertyId = STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty,
        QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery,
        AdditionalParameters = ToBytes(protocolSpecificData)
      };
      var qsize = (uint)Marshal.SizeOf(query);
      // https://stackoverflow.com/a/2069456/1230197
      var result = default(STORAGE_PROTOCOL_DATA_DESCRIPTOR);
      var rsize = Marshal.SizeOf(result);
      uint written = 0;
      bool ok = DeviceIoControl(_hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
        ref query, qsize, out result, rsize, ref written,
        IntPtr.Zero);

      ThrowOnFailure(ok);
      
      Console.WriteLine(JsonConvert.SerializeObject(result));

      return string.Empty;
    }

    private static SafeFileHandle OpenDrive(string physicalDrive)
    {
      SafeFileHandle hDevice = CreateFile(physicalDrive,
        dwDesiredAccess: (uint)WinAPI.FILE_ACCESS_FLAGS.GENERIC_READ | (uint)WinAPI.FILE_ACCESS_FLAGS.GENERIC_WRITE,
        dwShareMode: (uint)WinAPI.FILE_SHARE.READ,
        lpSecurityAttributes: IntPtr.Zero,
        dwCreationDisposition: (uint)WinAPI.CreateDisposition.OPEN_EXISTING,
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
    /// Taken from https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
    /// </summary>
    /// <param name="protocolSpecificData"></param>
    /// <returns></returns>
    private static byte[] ToBytes(STORAGE_PROTOCOL_SPECIFIC_DATA protocolSpecificData)
    {
      int size = Marshal.SizeOf(protocolSpecificData);
      byte[] arr = new byte[1024];

      // We have to know ahead of time the size of the array we marshal/unmarshal so this is made to match
      // the SizeConst in STORAGE_PROPERTY_QUERY.
      if (size > 1024)
      {
        Console.WriteLine("struct size larger than expected");
      }
      
      IntPtr ptr = Marshal.AllocHGlobal(size);
      Marshal.StructureToPtr(protocolSpecificData, ptr, true);
      Marshal.Copy(ptr, arr, 0, size);
      Marshal.FreeHGlobal(ptr);
      return arr;
    }

    /// <summary>
    /// Taken from https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    private static STORAGE_PROTOCOL_DATA_DESCRIPTOR FromBytes(byte[] arr)
    {
      STORAGE_PROTOCOL_DATA_DESCRIPTOR str = default(STORAGE_PROTOCOL_DATA_DESCRIPTOR);

      int size = Marshal.SizeOf(str);
      IntPtr ptr = Marshal.AllocHGlobal(size);

      Marshal.Copy(arr, 0, ptr, size);

      str = (STORAGE_PROTOCOL_DATA_DESCRIPTOR)Marshal.PtrToStructure(ptr, str.GetType());
      Marshal.FreeHGlobal(ptr);

      return str;
    }
    
    // Returns a list of the deviceIds of all of the physical disks attached
    // to the system. This is equivalent to running
    // `(Get-PhysicalDisk).deviceId` in PowerShell.
    public static IEnumerable<string> GetAllPhysicalDeviceIds()
    {
      // Adapted from https://stackoverflow.com/a/39869074/1230197
      IEnumerable<string> physicalDrives;

      var query = new WqlObjectQuery("SELECT * FROM Win32_DiskDrive");
      using (var searcher = new ManagementObjectSearcher(query))
      {
        physicalDrives = searcher.Get()
          .OfType<ManagementObject>()
          .Select(o => o.Properties["DeviceID"].Value.ToString());
      }
      
      // Strip off '\\.\PHYSICALDRIVE' prefix
      return physicalDrives.Select(drive => drive.Substring(PhysicalDrivePrefix.Length));
    }
  }
}
