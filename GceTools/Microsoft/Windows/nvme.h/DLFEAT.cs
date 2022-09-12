using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct DLFEAT
{
  private readonly byte _dlfeat;
  
  /// <summary>
  /// Indicate deallocated logical block read behavior
  /// </summary>
  public UCHAR ReadBehavior => (UCHAR)(_dlfeat & 0b111);
  
  /// <summary>
  /// Indicates controller supports the deallocate bit in Write Zeroes
  /// </summary>
  public UCHAR WriteZeroes => (UCHAR)((_dlfeat >> 3) & 0b1);

  /// <summary>
  /// Indicate guard field for deallocated logical blocks is set to CRC
  /// </summary>
  public UCHAR GuardFieldWithCRC => (UCHAR)((_dlfeat >> 4) & 0b1);
  
  // the three most significant bits are reserved
}