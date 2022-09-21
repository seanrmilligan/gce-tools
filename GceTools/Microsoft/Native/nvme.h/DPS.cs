namespace Microsoft.Native.nvme.h;

public readonly struct DPS
{
  private readonly byte _dps;
  public Byte ProtectionInfoTypeEnabled => (Byte)(_dps & 0b111);
  public Byte InfoAtBeginningOfMetadata => (Byte)((_dps >> 3) & 0b1);
  // the four most significant bits are reserved
}