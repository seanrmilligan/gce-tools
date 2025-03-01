using Microsoft.Native.winioctl.h;

namespace Microsoft.Managed.winioctl.h;

public class StorageDeviceDescriptor
{
  /// <summary>
  /// Contains the size of this structure, in bytes. The value of this
  /// member will change as members are added to the structure.
  /// </summary>
  public uint Version;

  /// <summary>
  /// Specifies the total size of the descriptor, in bytes, which may
  /// include vendor ID, product ID, product revision, device serial
  /// number strings and bus-specific data which are appended to the
  /// structure.
  /// </summary>
  public uint Size;

  /// <summary>
  /// Specifies the device type as defined by the Small Computer Systems
  /// Interface (SCSI) specification.
  /// </summary>
  public byte DeviceType;

  /// <summary>
  /// Specifies the device type modifier, if any, as defined by the SCSI
  /// specification. If no device type modifier exists, this member is
  /// zero.
  /// </summary>
  public byte DeviceTypeModifier;

  /// <summary>
  /// Indicates when TRUE that the device's media (if any) is removable.
  /// If the device has no media, this member should be ignored. When
  /// FALSE the device's media is not removable.
  /// </summary>
  public bool RemovableMedia;

  /// <summary>
  /// Indicates when TRUE that the device supports multiple outstanding
  /// commands (SCSI tagged queuing or equivalent). When FALSE, the device
  /// does not support SCSI-tagged queuing or the equivalent.
  /// </summary>
  public bool CommandQueueing;

  /// <summary>
  /// Specifies the byte offset from the beginning of the structure to a
  /// null-terminated ASCII string that contains the device's vendor ID.
  /// If the device has no vendor ID, this member is zero.
  /// </summary>
  public string VendorId;

  /// <summary>
  /// Specifies the byte offset from the beginning of the structure to a
  /// null-terminated ASCII string that contains the device's product ID.
  /// If the device has no product ID, this member is zero.
  /// </summary>
  public string ProductId;

  /// <summary>
  /// Specifies the byte offset from the beginning of the structure to a
  /// null-terminated ASCII string that contains the device's product
  /// revision string. If the device has no product revision string, this
  /// member is zero.
  /// </summary>
  public string ProductRevision;

  /// <summary>
  /// Specifies the byte offset from the beginning of the structure to a
  /// null-terminated ASCII string that contains the device's serial
  /// number. If the device has no serial number, this member is zero.
  /// </summary>
  public string SerialNumber;

  /// <summary>
  /// Specifies an enumerator value of type STORAGE_BUS_TYPE that
  /// indicates the type of bus to which the device is connected. This
  /// should be used to interpret the raw device properties at the end of
  /// this structure (if any).
  /// </summary>
  public STORAGE_BUS_TYPE BusType;

  /// <summary>
  /// Indicates the number of bytes of bus-specific data that have been
  /// appended to this descriptor.
  /// </summary>
  public uint RawPropertiesLength;
  
  public override string ToString()
  {
    return string.Join("\n", new[]
    {
      $"Version:               {Version}",
      $"Size:                  {Size}",
      $"DeviceType:            {DeviceType}",
      $"DeviceTypeModifier:    {DeviceTypeModifier}",
      $"RemovableMedia:        {RemovableMedia}",
      $"CommandQueueing:       {CommandQueueing}",
      $"VendorId:              {VendorId}",
      $"ProductId:             {ProductId}",
      $"ProductRevision:       {ProductRevision}",
      $"SerialNumber:          {SerialNumber}",
      $"BusType:               {BusType}",
      $"RawPropertiesLength:   {RawPropertiesLength}"
    });
  }
}