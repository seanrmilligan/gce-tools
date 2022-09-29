using System.ComponentModel;
using System.Text.Json;
using CommandLine;
using Google.Cloud.Storage.Extensions;
using Microsoft.Native.nvme.h;

namespace Google.Cloud.Storage
{
  public class Program
  {
    private static bool Verbose { get; set; }
    
    public static void Main(string[] args)
    {
      Parser.Default.ParseArguments<GceToolOptions>(args).WithParsed(options =>
      {
        Verbose = options.Verbose;
        IEnumerable<GoogleStorageDevice> devices = GetAllDevices(options.DeviceIds);

        if (options.BusType)
        {
          Console.WriteLine(string.Join(Environment.NewLine,
            devices.Select(d => d.GetBusType())));
          return;
        }
        
        if (options.StorageAdapterDescriptor)
        {
          Console.WriteLine(string.Join(Environment.NewLine,
            devices.Select(d => d.GetStorageAdapterDescriptor())));
          return;
        }

        if (options.StorageDeviceIdDescriptor)
        {
          Console.WriteLine(string.Join(Environment.NewLine,
            devices.Select(d => d.GetStorageDeviceIdDescriptor())));
          return;
        }
        
        IEnumerable<string> names = devices.Select(device => device.GetDeviceName());
        
        Console.WriteLine(string.Join(Environment.NewLine, names));
      });
    }

    public static IEnumerable<GoogleStorageDevice> GetAllDevices(IEnumerable<string> deviceIds)
    {
      if (deviceIds.None())
      {
        WriteDebugLine("No device IDs specified. Retrieving list of physical devices.");

        // List all physical drives if none specified.
        deviceIds = StorageDevice.GetAllPhysicalDeviceIds().ToList();

        if (deviceIds.None())
        {
          WriteDebugLine("No physical devices were found.");
          return Enumerable.Empty<GoogleStorageDevice>();
        }
      }
      
      return deviceIds
        .Select(deviceId => new GoogleStorageDevice(deviceId))
        .ToList();
    }

    public static Dictionary<string, IEnumerable<NamespaceIdMetadata>> GetNamespacesByController(IEnumerable<GoogleStorageDevice> devices)
    {
      return devices
        .GroupBy(device => device.NvmeIdentifyController().SN.ToAsciiString(0))
        .ToDictionary(group => group.Key,
          group => Enumerable.Range(1, 1024)
            .Select(namespaceId =>
            {
              try
              {
                return group
                  .First()
                  .GetNvmeNamespaceIdMetadata((uint)namespaceId);
              }
              catch (Win32Exception e)
              {
                switch (e.NativeErrorCode)
                {
                  case 0x1: // ERROR_INVALID_FUNCTION
                    // Occurs when you send an invalid namespace value like 0.
                    WriteDebugLine("Invalid namespace number.");
                    return null;
                    break;
                  case 0x45D: // ERROR_IO_DEVICE
                    // Occurs when the namespace requested does not exist.
                    WriteDebugLine($"Ignoring non-existent namespace {namespaceId}.");
                    return null;
                    break;
                  default:
                    // A new exception and we don't know why! How exciting.
                    throw;
                }
              }
            })
            .Where(metadata => metadata != null))!;
    }

    private static void WriteDebugLine(string line)
    {
      if (Verbose)
      {
        Console.Error.WriteLine(line);
      }
    }
  }
}
