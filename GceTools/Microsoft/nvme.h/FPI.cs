using UCHAR = System.Byte;

namespace Microsoft.nvme.h;

/// <summary>
/// The Format Progress Indicator
/// </summary>
public readonly struct FPI
{
  private readonly byte _fpi;
  
  /// <summary>
  /// Indicate the percentage of the namespace that remains to be formatted
  /// </summary>
  public UCHAR PercentageRemained => (UCHAR)(_fpi & 0b1111111);

  /// <summary>
  /// If set to 1 indicates that the namespace supports the Format Progress
  /// Indicator.
  /// </summary>
  public UCHAR Supported => (UCHAR)((_fpi >> 7) & 0b1);
}