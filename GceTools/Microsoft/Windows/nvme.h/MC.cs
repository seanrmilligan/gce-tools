using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct MC
{
  private readonly byte _mc; 
  public UCHAR MetadataInExtendedDataLBA => (UCHAR)(_mc & 0b1);
  public UCHAR MetadataInSeparateBuffer => (UCHAR)((_mc >> 1) & 0b1);
  // the six most significant bits are reserved
}