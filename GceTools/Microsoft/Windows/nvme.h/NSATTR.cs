using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct NSATTR
{
  private readonly byte _nsattr;

  public UCHAR WriteProtected => (UCHAR)(_nsattr & 0b1);
  // the seven most significant bits are reserved
}