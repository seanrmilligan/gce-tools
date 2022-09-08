using UCHAR = System.Byte;

namespace Microsoft.nvme.h;

public readonly struct NVM_RESERVATION_CAPABILITIES
{
  private readonly byte _rescap;
  public UCHAR PersistThroughPowerLoss => (UCHAR)(_rescap & 0b00000001);
  public UCHAR WriteExclusiveReservation => (UCHAR)((_rescap >> 1) & 0b1);
  public UCHAR ExclusiveAccessReservation => (UCHAR)((_rescap >> 2) & 0b1);
  public UCHAR WriteExclusiveRegistrantsOnlyReservation => (UCHAR)((_rescap >> 3) & 0b1);
  public UCHAR ExclusiveAccessRegistrantsOnlyReservation => (UCHAR)((_rescap >> 4) & 0b1);
  public UCHAR WriteExclusiveAllRegistrantsReservation => (UCHAR)((_rescap >> 5) & 0b1);
  public UCHAR ExclusiveAccessAllRegistrantsReservation => (UCHAR)((_rescap >> 6) & 0b1);
  // the most significant bit is reserved
  
  public UCHAR AsUchar => _rescap;
}