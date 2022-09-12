using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct DPC
{
  private readonly byte _dpc;
  public UCHAR ProtectionInfoType1 => (UCHAR)(_dpc & 0b1);
  public UCHAR ProtectionInfoType2 => (UCHAR)((_dpc >> 1) & 0b1);
  public UCHAR ProtectionInfoType3 => (UCHAR)((_dpc >> 2) & 0b1);
  public UCHAR InfoAtBeginningOfMetadata => (UCHAR)((_dpc >> 3) & 0b1);
  public UCHAR InfoAtEndOfMetadata => (UCHAR)((_dpc >> 4) & 0b1);
  // the three most significant bits are reserved
}