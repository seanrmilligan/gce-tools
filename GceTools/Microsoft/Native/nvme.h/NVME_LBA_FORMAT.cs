namespace Microsoft.Native.nvme.h;

public readonly struct NVME_LBA_FORMAT
{
  private readonly uint _lbaFormat;
  
  /// <summary>
  /// Metadata Size
  /// </summary>
  public UInt16 MS => (UInt16)(_lbaFormat & 0xFFFF);

  /// <summary>
  /// LBA  Data  Size
  /// </summary>
  public Byte LBADS => (Byte)((_lbaFormat >> 16) & 0xFF);
  
  /// <summary>
  /// Relative Performance
  /// </summary>
  public Byte RP => (Byte)((_lbaFormat >> 24) & 0b11);
  
  // the six most significant bits are reserved
  
  public UInt32 AsUlong => _lbaFormat;
}