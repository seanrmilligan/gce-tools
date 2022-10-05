// To test and debug this cmdlet:
//   - Build the solution (see below if you get errors for
//     System.Management.Automation).
//   - In powershell use a nested powershell to:
//     - Import-Module -Verbose -Force -Name C:\...\GceTools\GetGcePdName\bin\Debug\GetGcePdName.dll
//     - Get-Module  # verify Get-GcePdName is exported from GetGcePdName module
//     - Get-GcePdName -?
//   - Close the nested powershell and reopen a new one to re-build; Visual
//     Studio will not overwrite the .dll while the powershell that imported it
//     is still running.

using System.ComponentModel;
using Google.Cloud.Storage.Models;

namespace Google.Cloud.Storage
{


  using System.Management.Automation;

  [Cmdlet(VerbsCommon.Get, "GoogleDiskName")]
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
    
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public List<string>? DeviceIds { get; set; }
    #endregion Parameters

    #region PdName
    /// <summary>
    /// The object returned by the Get-GcePdName cmdlet: a mapping from the PD
    /// name to its physical disk number.
    /// </summary>
    // Adapted from example at
    // https://docs.microsoft.com/en-us/powershell/developer/cmdlet/creating-a-cmdlet-to-access-a-data-store#code-sample
    public class PdName
    {
      public string Name { get; set; }

      public string DeviceId { get; set; }

      public override string ToString()
      {
        return $"{Name}\t{DeviceId}";
      }
    }
    #endregion PdName

    #region Cmdlet Overrides
    protected override void ProcessRecord()
    {
      List<GoogleStorageDevice> devices = Program.GetAllDevices(DeviceIds);

      foreach (GoogleStorageDevice device in devices)
      {
        try
        {
          PdName pd = new PdName
          {
            Name = device.GetDeviceName(),
            DeviceId = device.Id
          };
          WriteObject(pd);
        }
        catch (Win32Exception ex)
        {
          WriteError(new ErrorRecord(ex, ex.ToString(),
              ErrorCategory.ReadError, device.Id));
        }
        catch (InvalidOperationException)
        {
          // InvalidOperation indicates that the deviceId is not for a GCE PD;
          // just ignore it.
        }
      }
    }
    #endregion Cmdlet Overrides
  }
}
