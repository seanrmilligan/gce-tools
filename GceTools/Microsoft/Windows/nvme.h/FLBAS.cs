using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct FLBAS
{
  private readonly byte _flbas;
  public UCHAR LbaFormatIndex => (UCHAR)(_flbas & 0b1111);
  public UCHAR MetadataInExtendedDataLBA => (UCHAR)((_flbas >> 4) & 0b1);
  // the three most significant bits are reserved
}