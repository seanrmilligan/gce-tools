﻿/* Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     https://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
    /// A list of physical disk numbers.
    /// </summary>
    /// <remarks>
    /// The type is string to match the DeviceId property from Get-PhysicalDisk.
    /// See third_party.Microsoft.Management.Infrastructure.CimInstance#root/microsoft/windows/storage/MSFT_PhysicalDisk
    /// for more on this type.
    /// The Parameter declaration here supports specifying the list of disk
    /// numbers in three ways:
    ///   Get-GoogleDiskName 1,3,5
    ///   @(1,3,5) | Get-GoogleDiskName
    ///   Get-PhysicalDisk | Get-GoogleDiskName
    /// Despite being an IEnumerable, the name cannot be pluralized and must be
    /// "DeviceId" to match the property outputted by Get-PhysicalDisk.
    /// </remarks>
    private IEnumerable<string> _deviceId;
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public IEnumerable<string> DeviceId
    {
      get => _deviceId;
      set => _deviceId = value;
    }
    #endregion Parameters

    protected override void ProcessRecord()
    {
      try
      {
        // List all physical drives if none were specified.
        if (DeviceId.IsNullOrEmpty())
        {
          DeviceId = StorageDevice.GetAllPhysicalDeviceIds();
        }
        
        // If there are still no DeviceId then we cannot proceed.
        if (DeviceId.IsNullOrEmpty())
        {
          InvalidOperationException ex = new(message:
            "No DeviceId(s) specified and no physical drives were found");
          WriteError(new ErrorRecord(ex, ex.ToString(),
            ErrorCategory.InvalidOperation, DeviceId));
          return;
        }

        foreach (string deviceId in DeviceId)
        {
          WriteObject(new DiskNameEntry
          {
            DeviceId = deviceId,
            Name = new GoogleStorageDevice(deviceId).GetDeviceName()
          });
        }
      }
      catch (Win32Exception ex)
      {
        WriteError(new ErrorRecord(ex, ex.ToString(),
          ErrorCategory.ReadError, null));
      }
      catch (InvalidOperationException ex)
      {
        WriteError(new ErrorRecord(ex, ex.ToString(),
          ErrorCategory.ReadError, null));
      }
    }
  }
}
