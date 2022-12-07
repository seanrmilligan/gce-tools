using System.ComponentModel;
using System.Management.Automation;
using Google.Cloud.Storage.Models;
using Google.Cloud.Storage.Models.Extensions;

namespace Google.Cloud.Storage.Cmdlets
{
  [Cmdlet(verbName: VerbsCommon.Get, nounName: "GoogleDiskName")]
  public class GetGoogleDiskNameCommand : Cmdlet
  {
    #region Parameters
    /// <summary>
    /// List of physical disk numbers.
    /// </summary>
    // The type is string to match the DeviceId property from Get-PhysicalDisk
    // (Microsoft.Management.Infrastructure.CimInstance#root/microsoft/windows/storage/MSFT_PhysicalDisk).
    // The Parameter declaration here supports specifying the list of disk
    // numbers in three ways:
    //   Get-GcePdName 1,3,5
    //   @(1,3,5) | Get-GcePdName
    //   Get-PhysicalDisk | Get-GcePdName
    private IEnumerable<string> _deviceIds;
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public IEnumerable<string> DeviceIds
    {
      get => _deviceIds;
      set => _deviceIds = value;
    }
    #endregion Parameters

    protected override void ProcessRecord()
    {
      try
      {
        if (DeviceIds == null)
        {
          // List all physical drives if none specified.
          DeviceIds = StorageDevice.GetAllPhysicalDeviceIds();
          if (DeviceIds.None())
          {
            var ex = new InvalidOperationException(
              "No device IDs specified and no physical drives were found");
            WriteError(new ErrorRecord(ex, ex.ToString(),
              ErrorCategory.InvalidOperation, DeviceIds));
            return;
          }
        }

       ;
        foreach (DiskNameEntry diskNameEntry in DeviceIds.Select(deviceId => new DiskNameEntry
        {
         DeviceId = deviceId,
         Name = new GoogleStorageDevice(deviceId).GetDeviceName()
        }))
        {
          WriteObject(diskNameEntry);
        }
      }
      catch (Win32Exception ex)
      {
        WriteError(new ErrorRecord(ex, ex.ToString(),
          ErrorCategory.ReadError, 123));
      }
      catch (InvalidOperationException)
      {
        // InvalidOperation indicates that the deviceId is not for a GCE PD;
        // just ignore it.
      }
    }
  }
}
