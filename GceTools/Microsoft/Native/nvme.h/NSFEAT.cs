namespace Microsoft.Native.nvme.h;

public readonly struct NSFEAT
{
  private readonly byte _nsfeat;
  public Byte ThinProvisioning => (Byte)(_nsfeat & 0b1);
  public Byte NameSpaceAtomicWriteUnit => (Byte)((_nsfeat >> 1) & 0b1);
  public Byte DeallocatedOrUnwrittenError => (Byte)((_nsfeat >> 2) & 0b1);
  public Byte SkipReuseUI => (Byte)((_nsfeat >> 3) & 0b1);
  public Byte NameSpaceIoOptimization => (Byte)((_nsfeat >> 4) & 0b1);
  // the three most significant bits are reserved
}