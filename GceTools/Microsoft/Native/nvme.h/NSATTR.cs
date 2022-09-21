namespace Microsoft.Native.nvme.h;

public readonly struct NSATTR
{
  private readonly byte _nsattr;

  public Byte WriteProtected => (Byte)(_nsattr & 0b1);
  // the seven most significant bits are reserved
}