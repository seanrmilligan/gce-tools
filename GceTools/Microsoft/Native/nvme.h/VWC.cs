namespace Microsoft.Native.nvme.h;

/// <summary>
/// Volatile Write Cache (VWC)
/// </summary>
public struct VWC
{
  private readonly Byte _vwc;
  public Byte Present => (Byte)(_vwc & 0b1);
  // the seven most significant bits are reserved
}