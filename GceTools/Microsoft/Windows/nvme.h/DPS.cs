using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct DPS
{
  private readonly byte _dps;
  public UCHAR ProtectionInfoTypeEnabled => (UCHAR)(_dps & 0b111);
  public UCHAR InfoAtBeginningOfMetadata => (UCHAR)((_dps >> 3) & 0b1);
  // the four most significant bits are reserved
}