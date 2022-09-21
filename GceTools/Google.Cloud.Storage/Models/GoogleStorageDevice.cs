using System.Text;
using System.Text.Json;
using Google.Cloud.Storage.Extensions;
using Google.Cloud.Storage.Models;
using Microsoft.Native.winioctl.h;

namespace Google.Cloud.Storage;

public class GoogleStorageDevice : StorageDevice
{
    /// <summary>
    /// The SCSI query that we execute below returns a string for the disk name
    /// that includes this prefix plus the PD name that we care about.
    /// </summary>
    private const string GoogleScsiPrefix = "Google  ";
    
    public GoogleStorageDevice(string id) : base(id)
    {
    }
    
    public string GetDeviceName()
    {
        return GetBusType() switch
        {
            STORAGE_BUS_TYPE.BusTypeNvme => GetNvmeDeviceName(),
            STORAGE_BUS_TYPE.BusTypeScsi => GetScsiDeviceName(),
            _ => throw new InvalidOperationException(
                message: "Unsupported device type. Device must be attached as an NVMe or SCSI device.")
        };
    }

    public uint GetNvmeNamespaceId()
    {
        StorageDeviceIdDescriptor deviceIdDescriptor = GetDeviceIdDescriptor();
        return uint.Parse(deviceIdDescriptor
            .Identifiers
            .First()
            .Identifier
            .Skip(44)
            .Take(4)
            .ToArray()
            .ToAsciiString(0));
    }

    public string GetNvmeDeviceName()
    {
        return GetNvmeNamespaceIdMetadata(GetNvmeNamespaceId()).DeviceName;
    }
    
    public string GetScsiDeviceName()
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

    public NamespaceIdMetadata GetNvmeNamespaceIdMetadata(uint namespaceId)
    {
        // Google writes metadata about the disk as a JSON string to the start
        // of the vendor-specific (VS) section of the NVMe Identify Specific
        // Namespace response struct.
        return JsonSerializer.Deserialize<NamespaceIdMetadata>(json: 
            NvmeIdentifySpecificNamespace(namespaceId)
                .VS
                .ToAsciiString(0))!;
    }
}