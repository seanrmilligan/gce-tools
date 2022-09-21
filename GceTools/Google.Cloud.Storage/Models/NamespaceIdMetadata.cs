using System.Text.Json.Serialization;

namespace Google.Cloud.Storage;

public class NamespaceIdMetadata
{
    [JsonPropertyName("disk_type")]
    public string DiskType { get; set; }
    
    [JsonPropertyName("device_name")]
    public string DeviceName { get; set; }

    public override string ToString()
    {
        return $"DiskType={DiskType}, DeviceName={DeviceName}";
    }
}