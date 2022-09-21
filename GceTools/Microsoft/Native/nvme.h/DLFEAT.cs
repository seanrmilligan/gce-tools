namespace Microsoft.Native.nvme.h;

public readonly struct DLFEAT
{
  private readonly byte _dlfeat;
  
  /// <summary>
  /// Indicate deallocated logical block read behavior
  /// </summary>
  public Byte ReadBehavior => (Byte)(_dlfeat & 0b111);
  
  /// <summary>
  /// Indicates controller supports the deallocate bit in Write Zeroes
  /// </summary>
  public Byte WriteZeroes => (Byte)((_dlfeat >> 3) & 0b1);

  /// <summary>
  /// Indicate guard field for deallocated logical blocks is set to CRC
  /// </summary>
  public Byte GuardFieldWithCRC => (Byte)((_dlfeat >> 4) & 0b1);
  
  // the three most significant bits are reserved
}