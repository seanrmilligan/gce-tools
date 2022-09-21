namespace Microsoft.Native.nvme.h;

public readonly struct FLBAS
{
  private readonly byte _flbas;
  public Byte LbaFormatIndex => (Byte)(_flbas & 0b1111);
  public Byte MetadataInExtendedDataLBA => (Byte)((_flbas >> 4) & 0b1);
  // the three most significant bits are reserved
}