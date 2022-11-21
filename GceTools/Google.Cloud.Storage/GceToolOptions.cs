using System.Collections.Generic;
using CommandLine;

namespace Google.Cloud.Storage;

public class GceToolOptions
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
    public uint NvmeIdentifyNamespace { get; set; }
    
    [Option(longName: "storage-adapter-descriptor")]
    public bool StorageAdapterDescriptor { get; set; }
    
    [Option(longName: "storage-device-descriptor")]
    public bool StorageDeviceDescriptor { get; set; }
    
    [Option(longName: "storage-device-id-descriptor")]
    public bool StorageDeviceIdDescriptor { get; set; }
}