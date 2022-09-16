using System.ComponentModel;
using CommandLine;
using Google.Cloud.Storage.Extensions;
using Microsoft.Windows.nvme.h;
using Microsoft.Windows.winioctl.h;

namespace Google.Cloud.Storage
{
  public class Options
  {
    [Option(shortName: 'd', longName: "device-ids", Required = false,
      Default = new string[0],
      HelpText = "The devices to run the command against.")]
    public IEnumerable<string> DeviceIds { get; set; }

    [Option(shortName: 'v', longName: "verbose", Required = false,
      HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
    
    [Option(longName: "bus-type")]
    public bool BusType { get; set; }
    
    [Option(longName: "nvme-identify-active-namespaces")]
    public bool NvmeIdentifyActiveNamespaces { get; set; }
    
    [Option(longName: "nvme-identify-controller")]
    public bool NvmeIdentifyController { get; set; }

    [Option(longName: "nvme-identify-namespace")]
    public bool NvmeIdentifyNamespace { get; set; }
    
    [Option(longName: "storage-adapter-descriptor")]
    public bool StorageAdapterDescriptor { get; set; }
    
    [Option(longName: "storage-device-descriptor")]
    public bool StorageDeviceDescriptor { get; set; }
    
    [Option(longName: "storage-device-id-descriptor")]
    public bool StorageDeviceIdDescriptor { get; set; }
  }

  public class Program
  {
    private static List<string> DeviceIds { get; set; }
    private static bool Verbose { get; set; }
    
    public static void Main(string[] args)
    {

      Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
      {
        Verbose = options.Verbose;
        DeviceIds = options.DeviceIds.ToList();

        try
        {
          if (DeviceIds.None())
          {
            WriteDebugLine("No device IDs specified. Retrieving list of physical devices.");

            // List all physical drives if none specified.
            DeviceIds = StorageDevice.GetAllPhysicalDeviceIds().ToList();

            if (DeviceIds.None())
            {
              WriteDebugLine("No physical devices were found.");
              return;
            }
          }

          List<StorageDevice> devices = DeviceIds
            .Select(deviceId => new StorageDevice(deviceId))
            .ToList();

          foreach (StorageDevice device in devices)
          {
            Console.WriteLine($"Physical Drive: {device.PhysicalDrive}");
            
            if (options.BusType)
            {
              Console.WriteLine($"BusType: {device.GetBusType()}");
              Console.WriteLine();
            }

            if (options.NvmeIdentifyActiveNamespaces)
            {
              Console.WriteLine($"{nameof(NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_ACTIVE_NAMESPACES)}:");
              int[] data = device.NvmeIdentifyActiveNamespaces();
              Console.WriteLine(string.Join(",", data));
            }
            if (options.NvmeIdentifyController)
            {
              Console.WriteLine($"{nameof(NVME_IDENTIFY_CONTROLLER_DATA)}:");
              NVME_IDENTIFY_CONTROLLER_DATA data = device.NvmeIdentifyController();
              Console.WriteLine(data);
              Console.WriteLine();
            }
            if (options.NvmeIdentifyNamespace)
            {
              NVME_IDENTIFY_NAMESPACE_DATA data = device.NvmeIdentifySpecificNamespace(1);
              string json = data.VS.ToAsciiString(0);
              Console.WriteLine($"{nameof(NVME_IDENTIFY_NAMESPACE_DATA)}:");
              Console.WriteLine(json);
              Console.WriteLine(data);
              Console.WriteLine();
            }
            if (options.StorageAdapterDescriptor)
            {
              Console.WriteLine($"{nameof(STORAGE_ADAPTER_DESCRIPTOR)}:");
              Console.WriteLine(device.GetAdapterDescriptor());
              Console.WriteLine();
            }
            if (options.StorageDeviceDescriptor)
            {
              Console.WriteLine($"{nameof(STORAGE_DEVICE_DESCRIPTOR)}:");
              Console.WriteLine(device.GetDeviceDescriptor());
              Console.WriteLine();
            }
            if (options.StorageDeviceIdDescriptor)
            {
              Console.WriteLine($"{nameof(STORAGE_DEVICE_ID_DESCRIPTOR)}:");
              Console.WriteLine(device.GetDeviceIdDescriptor());
              Console.WriteLine();
            }
          }
        }
        catch (Win32Exception ex)
        {
          Console.Error.WriteLine(ex);
        }
        catch (InvalidOperationException ex)
        {
          // InvalidOperation indicates that the deviceId is not for a GCE PD;
          // just ignore it.
          Console.Error.WriteLine(ex);
        }
      });
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
