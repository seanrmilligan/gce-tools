using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// The Format Progress Indicator
/// </summary>
public readonly struct FPI
{
  private readonly byte _fpi;
  
  /// <summary>
  /// Indicate the percentage of the namespace that remains to be formatted
  /// </summary>
  public Byte PercentageRemained => (Byte)(_fpi & 0b1111111);

  /// <summary>
  /// If set to 1 indicates that the namespace supports the Format Progress
  /// Indicator.
  /// </summary>
  public Byte Supported => (Byte)((_fpi >> 7) & 0b1);
}