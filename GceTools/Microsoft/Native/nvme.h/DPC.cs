using System;

namespace Microsoft.Native.nvme.h;

public readonly struct DPC
{
  private readonly byte _dpc;
  public Byte ProtectionInfoType1 => (Byte)(_dpc & 0b1);
  public Byte ProtectionInfoType2 => (Byte)((_dpc >> 1) & 0b1);
  public Byte ProtectionInfoType3 => (Byte)((_dpc >> 2) & 0b1);
  public Byte InfoAtBeginningOfMetadata => (Byte)((_dpc >> 3) & 0b1);
  public Byte InfoAtEndOfMetadata => (Byte)((_dpc >> 4) & 0b1);
  // the three most significant bits are reserved
}