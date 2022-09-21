namespace Microsoft.Native.nvme.h;

public readonly struct MC
{
  private readonly byte _mc; 
  public Byte MetadataInExtendedDataLBA => (Byte)(_mc & 0b1);
  public Byte MetadataInSeparateBuffer => (Byte)((_mc >> 1) & 0b1);
  // the six most significant bits are reserved
}