﻿using System;
using System.Runtime.InteropServices;

namespace Microsoft.Native.nvme.h;

[StructLayout(LayoutKind.Explicit)]
public readonly struct NVME_IDENTIFY_NAMESPACE_DATA
{
    [FieldOffset(0)]
    public readonly UInt64 NSZE;                   // byte 0:7.    M - Namespace Size (NSZE)
    
    [FieldOffset(8)]
    public readonly UInt64 NCAP;                   // byte 8:15    M - Namespace Capacity (NCAP)
    
    [FieldOffset(16)]
    public readonly UInt64 NUSE;                   // byte 16:23   M - Namespace Utilization (NUSE)

    [FieldOffset(24)]
    public readonly NSFEAT NSFEAT;               // byte 24      M - Namespace Features (NSFEAT)

    [FieldOffset(25)]
    public readonly Byte NLBAF;                 // byte 25      M - Number of LBA Formats (NLBAF)

    [FieldOffset(26)]
    public readonly FLBAS FLBAS;                 // byte 26      M - Formatted LBA Size (FLBAS)

    [FieldOffset(27)]
    public readonly MC MC;                       // byte 27      M - Metadata Capabilities (MC)

    [FieldOffset(28)]
    public readonly DPC DPC;                     // byte 28      M - End-to-end Data Protection Capabilities (DPC)

    [FieldOffset(29)]
    public readonly DPS DPS;                     // byte 29      M - End-to-end Data Protection Type Settings (DPS)

    [FieldOffset(30)]
    public readonly NMIC NMIC;                   // byte 30      O - Namespace Multi-path I/O and Namespace Sharing Capabilities (NMIC)

    [FieldOffset(31)]
    public readonly NVM_RESERVATION_CAPABILITIES RESCAP;    // byte 31      O - Reservation Capabilities (RESCAP)

    [FieldOffset(32)]
    public readonly FPI FPI;                     // byte 32      O - Format Progress Indicator (FPI)

    [FieldOffset(33)]
    public readonly DLFEAT DLFEAT;               // byte 33

    [FieldOffset(34)]
    public readonly UInt16 NAWUN;                // byte 34:35 O - Namespace Atomic Write Unit Normal (NAWUN)
    
    [FieldOffset(36)]
    public readonly UInt16 NAWUPF;               // byte 36:37 O - Namespace Atomic Write Unit Power Fail (NAWUPF)
    
    [FieldOffset(38)]
    public readonly UInt16 NACWU;                // byte 38:39 O - Namespace Atomic Compare & Write Unit (NACWU)
    
    [FieldOffset(40)]
    public readonly UInt16 NABSN;                // byte 40:41 O - Namespace Atomic Boundary Size Normal (NABSN)
    
    [FieldOffset(42)]
    public readonly UInt16 NABO;                 // byte 42:43 O - Namespace Atomic Boundary Offset (NABO)
    
    [FieldOffset(44)]
    public readonly UInt16 NABSPF;               // byte 44:45 O - Namespace Atomic Boundary Size Power Fail (NABSPF)
    
    [FieldOffset(46)]
    public readonly UInt16 NOIOB;                // byte 46:47 O - Namespace Optimal IO Boundary (NOIOB)

    [FieldOffset(48)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly Byte[] NVMCAP;                // byte 48:63 O - NVM Capacity (NVMCAP)

    [FieldOffset(64)]
    public readonly UInt16 NPWG;                 // byte 64:65 O - Namespace Preferred Write Granularity (NPWG)
    
    [FieldOffset(66)]
    public readonly UInt16 NPWA;                 // byte 66:67 O - Namespace Preferred Write Alignment (NPWA)
    
    [FieldOffset(68)]
    public readonly UInt16 NPDG;                 // byte 68:69 O - Namespace Preferred Deallocate Granularity (NPDG)
    
    [FieldOffset(70)]
    public readonly UInt16 NPDA;                 // byte 70:71 O - Namespace Preferred Deallocate Alignment (NPDA)
    
    [FieldOffset(72)]
    public readonly UInt16 NOWS;                 // byte 72:73 O - Namespace Optimal Write Size (NOWS)

    [FieldOffset(74)]
    public readonly UInt16 MSSRL;                // byte 74:75 O - Maximum Single Source Range Length(MSSRL)
    
    [FieldOffset(76)]
    public readonly UInt32 MCL;                   // byte 76:79 O - Maximum Copy Length(MCL)
    
    [FieldOffset(80)]
    public readonly Byte MSRC;                  // byte 80 O - Maximum Source Range Count(MSRC)
    
    // bytes [81, 91] are reserved

    [FieldOffset(92)]
    public readonly UInt32 ANAGRPID;              // byte 92:95 O - ANA Group Identifier (ANAGRPID)

    // bytes [96, 98] are reserved

    [FieldOffset(99)]
    public readonly NSATTR NSATTR;               // byte 99 O - Namespace Attributes

    [FieldOffset(100)]
    public readonly UInt16 NVMSETID;             // byte 100:101 O - Associated NVM Set Identifier
    
    [FieldOffset(102)]
    public readonly UInt16 ENDGID;               // byte 102:103 O - Associated Endurance Group Identier
    
    [FieldOffset(104)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly Byte[] NGUID;                 // byte 104:119 O - Namespace Globally Unique Identifier (NGUID)
    
    [FieldOffset(120)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public readonly Byte[] EUI64;                 // byte 120:127 M - IEEE Extended Unique Identifier (EUI64)

    [FieldOffset(128)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public readonly NVME_LBA_FORMAT[] LBAF;        // byte 128:131 M - LBA Format 0 Support (LBAF0)
                                        // byte 132:135 O - LBA Format 1 Support (LBAF1)
                                        // byte 136:139 O - LBA Format 2 Support (LBAF2)
                                        // byte 140:143 O - LBA Format 3 Support (LBAF3)
                                        // byte 144:147 O - LBA Format 4 Support (LBAF4)
                                        // byte 148:151 O - LBA Format 5 Support (LBAF5)
                                        // byte 152:155 O - LBA Format 6 Support (LBAF6)
                                        // byte 156:159 O - LBA Format 7 Support (LBAF7)
                                        // byte 160:163 O - LBA Format 8 Support (LBAF8)
                                        // byte 164:167 O - LBA Format 9 Support (LBAF9)
                                        // byte 168:171 O - LBA Format 10 Support (LBAF10)
                                        // byte 172:175 O - LBA Format 11 Support (LBAF11)
                                        // byte 176:179 O - LBA Format 12 Support (LBAF12)
                                        // byte 180:183 O - LBA Format 13 Support (LBAF13)
                                        // byte 184:187 O - LBA Format 14 Support (LBAF14)
                                        // byte 188:191 O - LBA Format 15 Support (LBAF15)

    // bytes [192, 383] are reserved

    /// <summary>
    /// Vendor Specific (VS): This range of bytes is allocated for vendor
    /// specific usage.
    /// </summary>
    [FieldOffset(384)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3712)]
    public readonly  Byte[] VS;

}

