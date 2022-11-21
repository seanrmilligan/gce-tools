namespace Google.Cloud.Storage.Models;

public class DiskNameEntry
{
    public string Name { get; set; }
    public string DeviceId { get; set; }

    public override string ToString()
    {
        return $"{Name}\t{DeviceId}";
    }
}