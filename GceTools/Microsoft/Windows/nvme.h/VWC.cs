using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Volatile Write Cache (VWC)
/// </summary>
public struct VWC
{
  private readonly UCHAR _vwc;
  public UCHAR Present => (UCHAR)(_vwc & 0b1);
  // the seven most significant bits are reserved
}