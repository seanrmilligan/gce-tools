// Copyright (c) Microsoft Corporation. All rights reserved.
// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ns-winioctl-storage_device_descriptor

using System.Linq;
using System.Runtime.InteropServices;
using BOOLEAN = System.Boolean;
using BYTE = System.Byte;
using DWORD = System.UInt32;
using ULONG = System.UInt32;

namespace Google.Cloud.Storage.Windows.winioctl
{
    /// <summary>
    /// Used in conjunction with the IOCTL_STORAGE_QUERY_PROPERTY control code
    /// to retrieve the storage device descriptor data for a device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct STORAGE_DEVICE_DESCRIPTOR
    {
        /// <summary>
        /// Contains the size of this structure, in bytes. The value of this
        /// member will change as members are added to the structure.
        /// </summary>
        [FieldOffset(0)]
        public readonly DWORD Version;
        
        /// <summary>
        /// Specifies the total size of the descriptor, in bytes, which may
        /// include vendor ID, product ID, product revision, device serial
        /// number strings and bus-specific data which are appended to the
        /// structure.
        /// </summary>
        [FieldOffset(4)]
        public readonly DWORD Size;
        
        /// <summary>
        /// Specifies the device type as defined by the Small Computer Systems
        /// Interface (SCSI) specification.
        /// </summary>
        [FieldOffset(8)]
        public readonly BYTE DeviceType;
        
        /// <summary>
        /// Specifies the device type modifier, if any, as defined by the SCSI
        /// specification. If no device type modifier exists, this member is
        /// zero.
        /// </summary>
        [FieldOffset(9)]
        public readonly BYTE DeviceTypeModifier;
        
        /// <summary>
        /// Indicates when TRUE that the device's media (if any) is removable.
        /// If the device has no media, this member should be ignored. When
        /// FALSE the device's media is not removable.
        /// </summary>
        [FieldOffset(10)]
        public readonly BOOLEAN RemovableMedia;
        
        /// <summary>
        /// Indicates when TRUE that the device supports multiple outstanding
        /// commands (SCSI tagged queuing or equivalent). When FALSE, the device
        /// does not support SCSI-tagged queuing or the equivalent.
        /// </summary>
        [FieldOffset(11)]
        public readonly BOOLEAN CommandQueueing;
       
        /// <summary>
        /// Specifies the byte offset from the beginning of the structure to a
        /// null-terminated ASCII string that contains the device's vendor ID.
        /// If the device has no vendor ID, this member is zero.
        /// </summary>
        [FieldOffset(12)]
        public readonly DWORD VendorIdOffset;
        
        /// <summary>
        /// Specifies the byte offset from the beginning of the structure to a
        /// null-terminated ASCII string that contains the device's product ID.
        /// If the device has no product ID, this member is zero.
        /// </summary>
        [FieldOffset(16)]
        public readonly DWORD ProductIdOffset;
       
        /// <summary>
        /// Specifies the byte offset from the beginning of the structure to a
        /// null-terminated ASCII string that contains the device's product
        /// revision string. If the device has no product revision string, this
        /// member is zero.
        /// </summary>
        [FieldOffset(20)]
        public readonly DWORD ProductRevisionOffset;
        
        /// <summary>
        /// Specifies the byte offset from the beginning of the structure to a
        /// null-terminated ASCII string that contains the device's serial
        /// number. If the device has no serial number, this member is zero.
        /// </summary>
        [FieldOffset(24)]
        public readonly DWORD SerialNumberOffset;
        
        /// <summary>
        /// Specifies an enumerator value of type STORAGE_BUS_TYPE that
        /// indicates the type of bus to which the device is connected. This
        /// should be used to interpret the raw device properties at the end of
        /// this structure (if any).
        /// </summary>
        [FieldOffset(28)]
        public readonly STORAGE_BUS_TYPE BusType;
        
        /// <summary>
        /// Indicates the number of bytes of bus-specific data that have been
        /// appended to this descriptor.
        /// </summary>
        [FieldOffset(29)]
        public readonly DWORD RawPropertiesLength;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(33)]
        private readonly BYTE RawDeviceProperties0;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(34)]
        private readonly BYTE RawDeviceProperties1;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(35)]
        private readonly BYTE RawDeviceProperties2;
        
        /// <summary>
        /// Contains an array of length one that serves as a place holder for
        /// the first byte of the bus specific property data.
        /// </summary>
        [FieldOffset(36)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        private readonly BYTE[] RawDeviceProperties3;
        
        public BYTE[] RawDeviceProperties()
        {

            return new[]
                {
                    RawDeviceProperties0,
                    RawDeviceProperties1,
                    RawDeviceProperties2
                }.Concat(RawDeviceProperties3)
                .ToArray();
        }

        public override string ToString()
        {
            return string.Join("\n", new[] {
                $"Version:               {Version}",
                $"Size:                  {Size}",
                $"DeviceType:            {DeviceType}",
                $"DeviceTypeModifier:    {DeviceTypeModifier}",
                $"RemovableMedia:        {RemovableMedia}",
                $"CommandQueueing:       {CommandQueueing}",
                $"VendorIdOffset:        {VendorIdOffset}",
                $"ProductIdOffset:       {ProductIdOffset}",
                $"ProductRevisionOffset: {ProductRevisionOffset}",
                $"SerialNumberOffset:    {SerialNumberOffset}",
                $"BusType:               {BusType}",
                $"RawPropertiesLength:   {RawPropertiesLength}",
                $"RawDeviceProperties:   {System.Text.Encoding.ASCII.GetString(RawDeviceProperties())}"
            });
        }
    }
}