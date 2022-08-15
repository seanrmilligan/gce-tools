// Code for .exe version of Get-GcePdName. This code may be or become broken now
// that the Powershell module version (GetGcePdNameCommand.cs) is working.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommandLine;
using Google.Cloud.Storage.Extensions;

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

          if (options.BusType) ListBusTypes(devices);
          if (options.NvmeIdentify) NvmeIdentify(devices);
          if (options.StorageAdapterDescriptor) ListAdapterDescriptors(devices);
          if (options.StorageDeviceDescriptor) ListDeviceDescriptors(devices);
          
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

    private static void ListAdapterDescriptors(List<StorageDevice> devices)
    {
      foreach (StorageDevice device in devices)
      {
        Console.WriteLine(device.GetAdapterDescriptor());
      }
    }
    
    private static void ListBusTypes(IEnumerable<StorageDevice> devices)
    {
      IEnumerable<Tuple<string, string>> deviceBusType = devices
        .Select(device => 
          new Tuple<string, string>(device.Id, device.GetBusType()));

      foreach (Tuple<string, string> tuple in deviceBusType)
      {
        Console.WriteLine($"{tuple.Item1}: {tuple.Item2}");
      }
    }

    private static void ListDeviceDescriptors(List<StorageDevice> devices)
    {
      foreach (StorageDevice device in devices)
      {
        Console.WriteLine(device.GetDeviceDescriptor());
      }
    }

    private static void NvmeIdentify(List<StorageDevice> devices)
    {
      foreach (StorageDevice device in devices)
      {
        Console.WriteLine(device.Get_GcePdName_Nvme());
      }
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
