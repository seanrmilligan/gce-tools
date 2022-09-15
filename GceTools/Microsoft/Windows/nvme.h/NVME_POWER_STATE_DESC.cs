using System.Runtime.InteropServices;
using UCHAR = System.Byte;
using ULONG = System.UInt32;
using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public readonly struct NVME_POWER_STATE_DESC
{
    [FieldOffset(0)]
    public readonly USHORT MP;                 // bit 0:15.    Maximum  Power (MP)

    // byte 2 is reserved

    [FieldOffset(3)]
    private readonly UCHAR _mps_nops;
    public UCHAR MPS => (UCHAR)(_mps_nops & 0b1); // bit 24: Max Power Scale (MPS)

    public UCHAR NOPS => (UCHAR)((_mps_nops >> 1) & 0b1); // bit 25: Non-Operational State (NOPS)
    // the next six bits are reserved

    [FieldOffset(4)]
    public readonly ULONG ENLAT;              // bit 32:63.   Entry Latency (ENLAT)
    
    [FieldOffset(8)]
    public readonly ULONG EXLAT;              // bit 64:95.   Exit Latency (EXLAT)

    [FieldOffset(12)]
    private readonly UCHAR _rrt;
    public UCHAR RRT => (UCHAR)(_rrt & 0b11111);  // bit 96:100.  Relative Read Throughput (RRT)
    // the next three bits are reserved

    [FieldOffset(13)]
    private readonly UCHAR _rrl;
    public UCHAR RRL => (UCHAR)(_rrl & 0b11111); // bit 104:108  Relative Read Latency (RRL)
    // the next three bits are reserved

    [FieldOffset(14)]
    private readonly UCHAR _rwt;
    public UCHAR RWT => (UCHAR)(_rwt & 0b11111); // bit 112:116  Relative Write Throughput (RWT)
    // the next three bits are reserved

    [FieldOffset(15)]
    private readonly UCHAR _rwl;
    public UCHAR RWL => (UCHAR)(_rwl & 0b11111); // bit 120:124  Relative Write Latency (RWL)
    // the next three bits are reserved

    [FieldOffset(16)]
    public readonly USHORT IDLP;               // bit 128:143  Idle Power (IDLP)

    [FieldOffset(18)]
    private readonly UCHAR _ips;
    // the next six bits are reserved
    public UCHAR IPS => (UCHAR)((_ips >> 6) & 0b11); // bit 150:151  Idle Power Scale (IPS)

    // byte 19 is reserved

    [FieldOffset(20)]
    public readonly USHORT ACTP;               // bit 160:175  Active Power (ACTP)

    [FieldOffset(22)]
    private readonly UCHAR _apw_aps;
    public UCHAR APW => (UCHAR)(_apw_aps & 0b111); // bit 176:178  Active Power Workload (APW)
    // the next three bits are reserved
    public UCHAR APS => (UCHAR)((_apw_aps >> 6) & 0b11); // bit 182:183  Active Power Scale (APS)
    
    // the nine most significant bytes are reserved
}