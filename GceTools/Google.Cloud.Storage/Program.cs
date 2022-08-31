using System.ComponentModel;
using CommandLine;
using Google.Cloud.Storage.Extensions;
using Google.Cloud.Storage.Windows.nvme;

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
    
    [Option(longName: "nvme-identify")]
    public bool NvmeIdentify { get; set; }
    
    [Option(longName: "storage-adapter-descriptor")]
    public bool StorageAdapterDescriptor { get; set; }
    
    [Option(longName: "storage-device-descriptor")]
    public bool StorageDeviceDescriptor { get; set; }
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
            .Select(deviceId => new StorageDevice(deviceId, Verbose))
            .ToList();

          foreach (StorageDevice device in devices)
          {
            Console.WriteLine($"Physical Drive: {device.PhysicalDrive}");
            if (options.BusType)
              Console.WriteLine(device.GetBusType());
            if (options.NvmeIdentify)
              Console.WriteLine(device.NvmeIdentify(NVME_IDENTIFY_CNS_CODES.NVME_IDENTIFY_CNS_CONTROLLER));
            if (options.StorageAdapterDescriptor)
              Console.WriteLine(device.GetAdapterDescriptor());
            if (options.StorageDeviceDescriptor)
              Console.WriteLine(device.GetDeviceDescriptor());
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
