using Google.Cloud.Storage.Models;

namespace Google.Cloud.Storage.Cmdlets.Engine;

public class GetGoogleDiskNameEngine
{
    public static IEnumerable<DiskNameEntry> ProcessRecord(IEnumerable<string>? deviceIds)
    {
        if (deviceIds == null)
        {
            // List all physical drives if none specified.
            deviceIds = StorageDevice.GetAllPhysicalDeviceIds();
            //if (_deviceIds.None())
            //{
            //  var ex = new InvalidOperationException(
            //    "No device IDs specified and no physical drives were found");
            //  WriteError(new ErrorRecord(ex, ex.ToString(),
            //    ErrorCategory.InvalidOperation, ""));
            //  return;
            //}
        }

        return deviceIds.Select(deviceId => new DiskNameEntry
        {
            DeviceId = deviceId,
            Name = new GoogleStorageDevice(deviceId).GetDeviceName()
        });
    }
}