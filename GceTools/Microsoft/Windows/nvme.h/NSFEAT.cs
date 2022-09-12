using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct NSFEAT
{
  private readonly byte _nsfeat;
  public UCHAR ThinProvisioning => (UCHAR)(_nsfeat & 0b1);
  public UCHAR NameSpaceAtomicWriteUnit => (UCHAR)((_nsfeat >> 1) & 0b1);
  public UCHAR DeallocatedOrUnwrittenError => (UCHAR)((_nsfeat >> 2) & 0b1);
  public UCHAR SkipReuseUI => (UCHAR)((_nsfeat >> 3) & 0b1);
  public UCHAR NameSpaceIoOptimization => (UCHAR)((_nsfeat >> 4) & 0b1);
  // the three most significant bits are reserved
}