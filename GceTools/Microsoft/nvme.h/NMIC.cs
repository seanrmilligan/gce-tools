using UCHAR = System.Byte;

namespace Microsoft.nvme.h;

public readonly struct NMIC
{
  private readonly byte _nmic;
  public UCHAR SharedNameSpace => (UCHAR)(_nmic & 0b1);
  // the seven most significant bits are reserved
}