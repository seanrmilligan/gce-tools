using System.Runtime.InteropServices;

using UCHAR = System.Byte;
using ULONG = System.UInt32;
using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

[StructLayout(LayoutKind.Explicit, Size = 4096)]
public readonly struct NVME_IDENTIFY_CONTROLLER_DATA
{
    //
    // byte 0 : 255, Controller Capabilities and Features
    //
    
    /// <summary>
    /// PCI Vendor ID (VID)
    /// </summary>
    [FieldOffset(0)]
    public readonly USHORT VID;
    
    /// <summary>
    /// PCI Subsystem Vendor ID (SSVID)
    /// </summary>
    [FieldOffset(2)]
    public readonly USHORT SSVID;
    
    [FieldOffset(4)]
    private readonly byte _sn0;
    
    [FieldOffset(5)]
    private readonly byte _sn1;
    
    [FieldOffset(6)]
    private readonly byte _sn2;
    
    [FieldOffset(7)]
    private readonly byte _sn3;
    
    [FieldOffset(8)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    private readonly UCHAR[] _sn4;
    
    /// <summary>
    /// Serial Number (SN)
    /// </summary>
    public UCHAR[] SN => new [] { _sn0, _sn1, _sn2, _sn3, }
        .Concat(_sn4)
        .ToArray();
    
    /// <summary>
    /// Model Number (MN)
    /// </summary>
    [FieldOffset(24)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
    public readonly UCHAR[] MN;
    
    /// <summary>
    /// Firmware Revision (FR)
    /// </summary>
    [FieldOffset(64)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public readonly UCHAR[] FR;
    
    /// <summary>
    /// Recommended Arbitration Burst (RAB)
    /// </summary>
    [FieldOffset(72)]
    public readonly UCHAR RAB;

    [FieldOffset(73)]
    private readonly byte _ieee0;
    
    [FieldOffset(74)]
    private readonly byte _ieee1;
    
    [FieldOffset(75)]
    private readonly byte _ieee2;
    
    /// <summary>
    /// IEEE OUI Identifier (IEEE). Controller Vendor code.
    /// </summary>
    public readonly UCHAR[] IEEE => new []{ _ieee0, _ieee1, _ieee2 };

    [FieldOffset(76)]
    public readonly CMIC CMIC;                     // byte 76.     O - Controller Multi-Path I/O and Namespace Sharing Capabilities (CMIC)

    [FieldOffset(77)]
    public readonly UCHAR MDTS;               // byte 77.     M - Maximum Data Transfer Size (MDTS)
    
    [FieldOffset(78)]
    public readonly USHORT CNTLID;             // byte 78:79.   M - Controller ID (CNTLID)
    
    [FieldOffset(80)]
    public readonly ULONG VER;                // byte 80:83.   M - Version (VER)
    
    [FieldOffset(84)]
    public readonly ULONG RTD3R;              // byte 84:87.   M - RTD3 Resume Latency (RTD3R)
    
    [FieldOffset(88)]
    public readonly ULONG RTD3E;              // byte 88:91.   M - RTD3 Entry Latency (RTD3E)

    [FieldOffset(92)]
    public readonly OAES OAES;                     // byte 92:95.   M - Optional Asynchronous Events Supported (OAES)

    // bytes [96, 239] are reserved
    
    [FieldOffset(240)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly UCHAR[] ReservedForManagement;     // byte 240:255.  Refer to the NVMe Management Interface Specification for definition.

    //
    // byte 256 : 511, Admin Command Set Attributes
    //
    [FieldOffset(256)]
    public readonly OACS OACS;                     // byte 256:257. M - Optional Admin Command Support (OACS)

    [FieldOffset(258)]
    public readonly UCHAR ACL;                // byte 258.    M - Abort Command Limit (ACL)
    
    [FieldOffset(259)]
    public readonly UCHAR AERL;               // byte 259.    M - Asynchronous Event Request Limit (AERL)

    [FieldOffset(260)]
    public readonly FRMW FRMW;                     // byte 260.    M - Firmware Updates (FRMW)

    [FieldOffset(261)]
    public readonly LPA LPA;                      // byte 261.    M - Log Page Attributes (LPA)

    [FieldOffset(262)]
    public readonly UCHAR ELPE;               // byte 262.    M - Error Log Page Entries (ELPE)
    
    [FieldOffset(263)]
    public readonly UCHAR NPSS;               // byte 263.    M - Number of Power States Support (NPSS)

    [FieldOffset(264)]
    public readonly AVSCC AVSCC;                    // byte 264.    M - Admin Vendor Specific Command Configuration (AVSCC)

    [FieldOffset(265)]
    public readonly APSTA APSTA;                    // byte 265.     O - Autonomous Power State Transition Attributes (APSTA)

    [FieldOffset(266)]
    public readonly USHORT WCTEMP;             // byte 266:267. M - Warning Composite Temperature Threshold (WCTEMP)
    
    [FieldOffset(268)]
    public readonly USHORT CCTEMP;             // byte 268:269. M - Critical Composite Temperature Threshold (CCTEMP)
    
    [FieldOffset(270)]
    public readonly USHORT MTFA;               // byte 270:271. O - Maximum Time for Firmware Activation (MTFA)
    
    [FieldOffset(272)]
    public readonly ULONG HMPRE;              // byte 272:275. O - Host Memory Buffer Preferred Size (HMPRE)
    
    [FieldOffset(276)]
    public readonly ULONG HMMIN;              // byte 276:279. O - Host Memory Buffer Minimum Size (HMMIN)
    
    [FieldOffset(280)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly UCHAR[] TNVMCAP;        // byte 280:295. O - Total NVM Capacity (TNVMCAP)
    
    [FieldOffset(296)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly UCHAR[] UNVMCAP;        // byte 296:311. O - Unallocated NVM Capacity (UNVMCAP)

    [FieldOffset(312)]
    public readonly RPMBS RPMBS;                    // byte 312:315. O - Replay Protected Memory Block Support (RPMBS)

    [FieldOffset(316)]
    public readonly USHORT EDSTT;              // byte 316:317. O - Extended Device Self-test Time (EDSTT)
    
    [FieldOffset(318)]
    public readonly UCHAR DSTO;               // byte 318.     O - Device Self-test Options (DSTO)
    
    [FieldOffset(319)]
    public readonly UCHAR FWUG;               // byte 319.     M - Firmware Update Granularity (FWUG)
    
    [FieldOffset(320)]
    public readonly USHORT KAS;                // byte 320:321  M - Keep Alive Support (KAS)
    
    [FieldOffset(322)]
    public readonly HCTMA HCTMA;                    // byte 322:323  O - Host Controlled Thermal Management Attributes (HCTMA)

    [FieldOffset(324)]
    public readonly USHORT MNTMT;              // byte 324:325  O - Minimum Thermal Management Temperature (MNTMT)
    
    [FieldOffset(326)]
    public readonly USHORT MXTMT;              // byte 326:327  O - Maximum Thermal Management Temperature (MXTMT)

    [FieldOffset(328)]
    public readonly SANICAP SANICAP;                  // byte 328:331  O - Sanitize Capabilities (SANICAP)
    
    // bytes [332, 511] are reserved

    //
    // byte 512 : 703, NVM Command Set Attributes
    //
    [FieldOffset(512)]
    public readonly SQES SQES;                     // byte 512.    M - 

    [FieldOffset(513)]
    public readonly CQES CQES;                     // byte 513.    M - Completion Queue Entry Size (CQES)

    // bytes [514, 515] are reserved

    [FieldOffset(516)]
    public readonly ULONG NN;                 // byte 516:519. M - Number of Namespaces (NN)

    [FieldOffset(520)]
    public readonly ONCS ONCS;                     // byte 520:521. M - Optional NVM Command Support (ONCS)

    [FieldOffset(522)]
    public readonly FUSES FUSES;                    // byte 522:523. M - Fused Operation Support (FUSES)

    [FieldOffset(524)]
    public readonly FNA FNA;                      // byte 524.     M - Format NVM Attributes (FNA)

    [FieldOffset(525)]
    public readonly VWC VWC;                      // byte 525.     M - Volatile Write Cache (VWC)

    [FieldOffset(526)]
    public readonly USHORT AWUN;               // byte 526:527. M - Atomic Write Unit Normal (AWUN)
    
    [FieldOffset(528)]
    public readonly USHORT AWUPF;              // byte 528:529. M - Atomic Write Unit Power Fail (AWUPF)

    [FieldOffset(530)]
    public readonly NVSCC NVSCC;                    // byte 530.     M - NVM Vendor Specific Command Configuration (NVSCC)

    // byte 531 is reserved

    [FieldOffset(532)]
    public readonly USHORT ACWU;               // byte 532:533  O - Atomic Compare & Write Unit (ACWU)

    // bytes [534, 535] are reserved

    [FieldOffset(536)]
    public readonly SGLS SGLS;                     // byte 536:539. O - SGL Support (SGLS)

    // bytes [540, 703] are reserved

    //
    // byte 704 : 2047, I/O Command Set Attributes
    //
    
    // bytes [704, 2047] are reserved

    //
    // byte 2048 : 3071, Power State Descriptors
    //
    [FieldOffset(2048)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public readonly NVME_POWER_STATE_DESC[] PDS;    // byte 2048:2079. M - Power State 0 Descriptor (PSD0):  This field indicates the characteristics of power state 0
                                        // byte 2080:2111. O - Power State 1 Descriptor (PSD1):  This field indicates the characteristics of power state 1
                                        // byte 2112:2143. O - Power State 2 Descriptor (PSD1):  This field indicates the characteristics of power state 2
                                        // byte 2144:2175. O - Power State 3 Descriptor (PSD1):  This field indicates the characteristics of power state 3
                                        // byte 2176:2207. O - Power State 4 Descriptor (PSD1):  This field indicates the characteristics of power state 4
                                        // byte 2208:2239. O - Power State 5 Descriptor (PSD1):  This field indicates the characteristics of power state 5
                                        // byte 2240:2271. O - Power State 6 Descriptor (PSD1):  This field indicates the characteristics of power state 6
                                        // byte 2272:2303. O - Power State 7 Descriptor (PSD1):  This field indicates the characteristics of power state 7
                                        // byte 2304:2335. O - Power State 8 Descriptor (PSD1):  This field indicates the characteristics of power state 8
                                        // byte 2336:2367. O - Power State 9 Descriptor (PSD1):  This field indicates the characteristics of power state 9
                                        // byte 2368:2399. O - Power State 10 Descriptor (PSD1):  This field indicates the characteristics of power state 10
                                        // byte 2400:2431. O - Power State 11 Descriptor (PSD1):  This field indicates the characteristics of power state 11
                                        // byte 2432:2463. O - Power State 12 Descriptor (PSD1):  This field indicates the characteristics of power state 12
                                        // byte 2464:2495. O - Power State 13 Descriptor (PSD1):  This field indicates the characteristics of power state 13
                                        // byte 2496:2527. O - Power State 14 Descriptor (PSD1):  This field indicates the characteristics of power state 14
                                        // byte 2528:2559. O - Power State 15 Descriptor (PSD1):  This field indicates the characteristics of power state 15
                                        // byte 2560:2591. O - Power State 16 Descriptor (PSD1):  This field indicates the characteristics of power state 16
                                        // byte 2592:2623. O - Power State 17 Descriptor (PSD1):  This field indicates the characteristics of power state 17
                                        // byte 2624:2655. O - Power State 18 Descriptor (PSD1):  This field indicates the characteristics of power state 18
                                        // byte 2656:2687. O - Power State 19 Descriptor (PSD1):  This field indicates the characteristics of power state 19
                                        // byte 2688:2719. O - Power State 20 Descriptor (PSD1):  This field indicates the characteristics of power state 20
                                        // byte 2720:2751. O - Power State 21 Descriptor (PSD1):  This field indicates the characteristics of power state 21
                                        // byte 2752:2783. O - Power State 22 Descriptor (PSD1):  This field indicates the characteristics of power state 22
                                        // byte 2784:2815. O - Power State 23 Descriptor (PSD1):  This field indicates the characteristics of power state 23
                                        // byte 2816:2847. O - Power State 24 Descriptor (PSD1):  This field indicates the characteristics of power state 24
                                        // byte 2848:2879. O - Power State 25 Descriptor (PSD1):  This field indicates the characteristics of power state 25
                                        // byte 2880:2911. O - Power State 26 Descriptor (PSD1):  This field indicates the characteristics of power state 26
                                        // byte 2912:2943. O - Power State 27 Descriptor (PSD1):  This field indicates the characteristics of power state 27
                                        // byte 2944:2975. O - Power State 28 Descriptor (PSD1):  This field indicates the characteristics of power state 28
                                        // byte 2976:3007. O - Power State 29 Descriptor (PSD1):  This field indicates the characteristics of power state 29
                                        // byte 3008:3039. O - Power State 30 Descriptor (PSD1):  This field indicates the characteristics of power state 30
                                        // byte 3040:3071. O - Power State 31 Descriptor (PSD1):  This field indicates the characteristics of power state 31

    //
    // byte 3072 : 4095, Vendor Specific
    //
    [FieldOffset(3072)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public readonly UCHAR[] VS;           // byte 3072 : 4095.
}