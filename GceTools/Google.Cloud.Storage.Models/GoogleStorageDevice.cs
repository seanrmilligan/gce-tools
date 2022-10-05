using System.Text;
using System.Text.Json;
using Google.Cloud.Storage.Models.Extensions;
using Microsoft.Managed.winioctl.h;
using Microsoft.Native.nvme.h;
using Microsoft.Native.winioctl.h;
using NVMe.ScsiTranslation.v1_5;

namespace Google.Cloud.Storage.Models;

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
        NVME_IDENTIFY_CONTROLLER_DATA controller = NvmeIdentifyController();
        
        return controller.VER switch
        {
            (uint)NvmeVersion.Version_1_0 => uint.Parse(GetStorageDeviceIdDescriptor()
                .Identifiers
                .First()
                .Identifier
                .ToStruct<NvmeV1BasedScsiNameString>()
                .NamespaceId
                .ToAsciiString(0)),
            
            // On Windows Builds 1903 or later, we would expect this to NOT be
            // an NvmeV1BasedScsiNameString when the NvmeVersion is greater than
            // 1.0 due to StorNVMe on 1903 or later complying with SCSI-NVMe
            // Translation Layer (SNTL) Reference Revision 1.5. However, for
            // builds prior to 1903 we are observing a return value with the
            // shape of an NvmeV1BasedScsiNameString.
            //
            // No documentation on StorNVMe behavior on builds prior to 1903
            // have been found and behavior has been detected experimentally.
            //
            // For Windows Builds 1903 or later this will need to be expanded to
            // detect and support other identifier types in SNTL Ref. Rev. 1.5,
            // including:
            // - 6.1.4.1 NAA IEEE Registered Extended designator format
            // - 6.1.4.3 T10 Vendor ID based designator format
            // - 6.1.4.5 EUI-64 designator format
            > (uint)NvmeVersion.Version_1_0 => uint.Parse(GetStorageDeviceIdDescriptor()
                .Identifiers
                .First()
                .Identifier
                .ToStruct<NvmeV1BasedScsiNameString>()
                .NamespaceId
                .ToAsciiString(0)),
            
            _ => throw new NotSupportedException(
                message: $"NVMe versions prior to v1.0 are not supported by this application. {PhysicalDrive} reported NVMe Version {controller.VER}.")
        };
    }

    public string GetNvmeDeviceName()
    {
        return GetNvmeNamespaceIdMetadata(GetNvmeNamespaceId()).DeviceName;
    }
    
    public string GetScsiDeviceName()
    {
        StorageDeviceIdDescriptor deviceIdDescriptor = GetStorageDeviceIdDescriptor();

        string persistentDiskName = Enumerable.FirstOrDefault<string>(deviceIdDescriptor
                .Identifiers
                .Where(storageIdentifier =>
                    storageIdentifier.Type == STORAGE_IDENTIFIER_TYPE.StorageIdTypeVendorId &&
                    storageIdentifier.Association == STORAGE_ASSOCIATION_TYPE.StorageIdAssocDevice)
                .Select(storageIdentifier => Encoding.ASCII.GetString((byte[])storageIdentifier.Identifier)), name => name.StartsWith(GoogleScsiPrefix))
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