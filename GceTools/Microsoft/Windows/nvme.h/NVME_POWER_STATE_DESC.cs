using System.Runtime.InteropServices;
using UCHAR = System.Byte;
using ULONG = System.UInt32;
using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

[StructLayout(LayoutKind.Explicit)]
public readonly struct NVME_POWER_STATE_DESC
{
    [FieldOffset(0)]
    public readonly USHORT MP;                 // bit 0:15.    Maximum  Power (MP)

    UCHAR   Reserved0;          // bit 16:23

    UCHAR   MPS         : 1;    // bit 24: Max Power Scale (MPS)
    UCHAR   NOPS        : 1;    // bit 25: Non-Operational State (NOPS)
    UCHAR   Reserved1   : 6;    // bit 26:31

    ULONG   ENLAT;              // bit 32:63.   Entry Latency (ENLAT)
    ULONG   EXLAT;              // bit 64:95.   Exit Latency (EXLAT)

    UCHAR   RRT         : 5;    // bit 96:100.  Relative Read Throughput (RRT)
    UCHAR   Reserved2   : 3;    // bit 101:103

    UCHAR   RRL         : 5;    // bit 104:108  Relative Read Latency (RRL)
    UCHAR   Reserved3   : 3;    // bit 109:111

    UCHAR   RWT         : 5;    // bit 112:116  Relative Write Throughput (RWT)
    UCHAR   Reserved4   : 3;    // bit 117:119

    UCHAR   RWL         : 5;    // bit 120:124  Relative Write Latency (RWL)
    UCHAR   Reserved5   : 3;    // bit 125:127

    USHORT  IDLP;               // bit 128:143  Idle Power (IDLP)

    UCHAR   Reserved6   : 6;    // bit 144:149
    UCHAR   IPS         : 2;    // bit 150:151  Idle Power Scale (IPS)

    UCHAR   Reserved7;          // bit 152:159

    USHORT  ACTP;               // bit 160:175  Active Power (ACTP)

    UCHAR   APW         : 3;    // bit 176:178  Active Power Workload (APW)
    UCHAR   Reserved8   : 3;    // bit 179:181
    UCHAR   APS         : 2;    // bit 182:183  Active Power Scale (APS)


    UCHAR   Reserved9[9];       // bit 184:255.
}