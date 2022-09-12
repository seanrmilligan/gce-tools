using UCHAR = System.Byte;
using ULONG = System.UInt32;
using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

public readonly struct NVME_LBA_FORMAT
{
  private readonly uint _lbaFormat;
  
  /// <summary>
  /// Metadata Size
  /// </summary>
  public USHORT MS => (USHORT)(_lbaFormat & 0xFFFF);

  /// <summary>
  /// LBA  Data  Size
  /// </summary>
  public UCHAR LBADS => (UCHAR)((_lbaFormat >> 16) & 0xFF);
  
  /// <summary>
  /// Relative Performance
  /// </summary>
  public UCHAR RP => (UCHAR)((_lbaFormat >> 24) & 0b11);
  
  // the six most significant bits are reserved
  
  public ULONG AsUlong => _lbaFormat;
}