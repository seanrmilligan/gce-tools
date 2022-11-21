using System;

namespace Microsoft.Native.nvme.h;

public readonly struct NMIC
{
  private readonly byte _nmic;
  public Byte SharedNameSpace => (Byte)(_nmic & 0b1);
  // the seven most significant bits are reserved
}