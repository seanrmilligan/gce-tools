// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ns-winioctl-storage_adapter_descriptor
using System.Runtime.InteropServices;
using BOOLEAN = System.Boolean;
using BYTE = System.Byte;
using WORD = System.UInt16;
using DWORD = System.UInt32;
using ULONG = System.UInt32;

namespace Google.Cloud.Storage.Windows.winioctl
{
    /// <summary>
    /// Used with the IOCTL_STORAGE_QUERY_PROPERTY control code to retrieve the
    /// storage adapter descriptor data for a device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct STORAGE_ADAPTER_DESCRIPTOR
    {
        /// <summary>
        /// Contains the size of this structure, in bytes. The value of this
        /// member will change as members are added to the structure.
        /// </summary>
        [FieldOffset(0)]
        public readonly DWORD Version;
        
        /// <summary>
        /// Specifies the total size of the data returned, in bytes. This may
        /// include data that follows this structure.
        /// </summary>
        [FieldOffset(4)]
        public readonly DWORD Size;
        
        /// <summary>
        /// Specifies the maximum number of bytes the storage adapter can
        /// transfer in a single operation.
        /// </summary>
        [FieldOffset(8)]
        public readonly DWORD MaximumTransferLength;
        
        /// <summary>
        /// Specifies the maximum number of discontinuous physical pages the
        /// storage adapter can manage in a single transfer (in other words, the
        /// extent of its scatter/gather support).
        /// </summary>
        [FieldOffset(12)]
        public readonly DWORD MaximumPhysicalPages;
        
        /// <summary>
        /// Specifies the storage adapter's alignment requirements for
        /// transfers. The alignment mask indicates alignment restrictions for
        /// buffers required by the storage adapter for transfer operations.
        /// Valid mask values are also restricted by characteristics of the
        /// memory managers on different versions of Windows.
        /// 
        /// 0 - Buffers must be aligned on BYTE boundaries.
        /// 1 - Buffers must be aligned on WORD boundaries.
        /// 3 - Buffers must be aligned on DWORD32 boundaries.
        /// 7 - Buffers must be aligned on DWORD64 boundaries.
        /// </summary>
        [FieldOffset(16)]
        public readonly DWORD AlignmentMask;
        
        /// <summary>
        /// If this member is TRUE, the storage adapter uses programmed I/O
        /// (PIO) and requires the use of system-space virtual addresses mapped
        /// to physical memory for data buffers. When this member is FALSE, the
        /// storage adapter does not use PIO.
        /// </summary>
        [FieldOffset(20)]
        public readonly BOOLEAN AdapterUsesPio;
        
        /// <summary>
        /// If this member is TRUE, the storage adapter scans down for BIOS
        /// devices, that is, the storage adapter begins scanning with the
        /// highest device number rather than the lowest. When this member is
        /// FALSE, the storage adapter begins scanning with the lowest device
        /// number. This member is reserved for legacy miniport drivers.
        /// </summary>
        [FieldOffset(21)]
        public readonly BOOLEAN AdapterScansDown;
        
        /// <summary>
        /// If this member is TRUE, the storage adapter supports SCSI tagged
        /// queuing and/or per-logical-unit internal queues, or the non-SCSI
        /// equivalent. When this member is FALSE, the storage adapter neither
        /// supports SCSI-tagged queuing nor per-logical-unit internal queues.
        /// </summary>
        [FieldOffset(22)]
        public readonly BOOLEAN CommandQueueing;
        
        /// <summary>
        /// If this member is TRUE, the storage adapter supports synchronous
        /// transfers as a way of speeding up I/O. When this member is FALSE,
        /// the storage adapter does not support synchronous transfers as a way
        /// of speeding up I/O.
        /// </summary>
        [FieldOffset(23)]
        public readonly BOOLEAN AcceleratedTransfer;
        
        /// <summary>
        /// Specifies a value of type <see cref="STORAGE_BUS_TYPE" /> that
        /// indicates the type of the bus to which the device is connected.
        /// </summary>
        [FieldOffset(24)]
        public readonly STORAGE_BUS_TYPE BusType;
        
        /// <summary>
        /// Specifies the major version number, if any, of the storage adapter.
        /// </summary>
        [FieldOffset(25)]
        public readonly WORD BusMajorVersion;
        
        /// <summary>
        /// Specifies the minor version number, if any, of the storage adapter.
        /// 
        /// </summary>
        [FieldOffset(27)]
        public readonly WORD BusMinorVersion;
        
        /// <summary>
        /// Specifies the SCSI request block (SRB) type used by the HBA.
        /// 
        /// SRB_TYPE_SCSI_REQUEST_BLOCK - The HBA uses SCSI request blocks.
        /// SRB_TYPE_STORAGE_REQUEST_BLOCK - The HBA uses extended SCSI request
        /// blocks.
        ///
        /// This member is valid starting with Windows 8.
        /// </summary>
        [FieldOffset(29)]
        public readonly BYTE SrbType;
        
        /// <summary>
        /// Specifies the address type of the HBA.
        ///
        /// STORAGE_ADDRESS_TYPE_BTL8 The HBA uses 8-bit bus, target, and LUN
        /// addressing.
        ///
        /// This member is valid starting with Windows 8.
        /// </summary>
        [FieldOffset(30)]
        public readonly BYTE AddressType;
        
        public override string ToString()
        {
            return string.Join("\n", new[] {
                $"Version:               {Version}",
                $"Size:                  {Size}",
                $"MaximumTransferLength: {MaximumTransferLength}",
                $"MaximumPhysicalPages:  {MaximumPhysicalPages}",
                $"AlignmentMask:         {AlignmentMask}",
                $"AdapterUsesPio:        {AdapterUsesPio}",
                $"AdapterScansDown:      {AdapterScansDown}",
                $"CommandQueueing:       {CommandQueueing}",
                $"AcceleratedTransfer:   {AcceleratedTransfer}",
                $"BusType:               {BusType}",
                $"BusMajorVersion:       {BusMajorVersion}",
                $"BusMinorVersion:       {BusMinorVersion}",
                $"SrbType:               {SrbType}",
                $"AddressType:           {AddressType}"
            });
        }
    }
}