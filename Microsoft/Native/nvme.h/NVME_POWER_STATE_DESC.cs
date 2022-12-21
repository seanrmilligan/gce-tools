using System;
using System.Runtime.InteropServices;

namespace Microsoft.Native.nvme.h;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public readonly struct NVME_POWER_STATE_DESC
{
    [FieldOffset(0)]
    public readonly UInt16 MP;                 // bit 0:15.    Maximum  Power (MP)

    // byte 2 is reserved

    [FieldOffset(3)]
    private readonly Byte _mps_nops;
    public Byte MPS => (Byte)(_mps_nops & 0b1); // bit 24: Max Power Scale (MPS)

    public Byte NOPS => (Byte)((_mps_nops >> 1) & 0b1); // bit 25: Non-Operational State (NOPS)
    // the next six bits are reserved

    [FieldOffset(4)]
    public readonly UInt32 ENLAT;              // bit 32:63.   Entry Latency (ENLAT)
    
    [FieldOffset(8)]
    public readonly UInt32 EXLAT;              // bit 64:95.   Exit Latency (EXLAT)

    [FieldOffset(12)]
    private readonly Byte _rrt;
    public Byte RRT => (Byte)(_rrt & 0b11111);  // bit 96:100.  Relative Read Throughput (RRT)
    // the next three bits are reserved

    [FieldOffset(13)]
    private readonly Byte _rrl;
    public Byte RRL => (Byte)(_rrl & 0b11111); // bit 104:108  Relative Read Latency (RRL)
    // the next three bits are reserved

    [FieldOffset(14)]
    private readonly Byte _rwt;
    public Byte RWT => (Byte)(_rwt & 0b11111); // bit 112:116  Relative Write Throughput (RWT)
    // the next three bits are reserved

    [FieldOffset(15)]
    private readonly Byte _rwl;
    public Byte RWL => (Byte)(_rwl & 0b11111); // bit 120:124  Relative Write Latency (RWL)
    // the next three bits are reserved

    [FieldOffset(16)]
    public readonly UInt16 IDLP;               // bit 128:143  Idle Power (IDLP)

    [FieldOffset(18)]
    private readonly Byte _ips;
    // the next six bits are reserved
    public Byte IPS => (Byte)((_ips >> 6) & 0b11); // bit 150:151  Idle Power Scale (IPS)

    // byte 19 is reserved

    [FieldOffset(20)]
    public readonly UInt16 ACTP;               // bit 160:175  Active Power (ACTP)

    [FieldOffset(22)]
    private readonly Byte _apw_aps;
    public Byte APW => (Byte)(_apw_aps & 0b111); // bit 176:178  Active Power Workload (APW)
    // the next three bits are reserved
    public Byte APS => (Byte)((_apw_aps >> 6) & 0b11); // bit 182:183  Active Power Scale (APS)
    
    // the nine most significant bytes are reserved
}