namespace Microsoft.Native.nvme.h;

public readonly struct NVM_RESERVATION_CAPABILITIES
{
  private readonly byte _rescap;
  public Byte PersistThroughPowerLoss => (Byte)(_rescap & 0b00000001);
  public Byte WriteExclusiveReservation => (Byte)((_rescap >> 1) & 0b1);
  public Byte ExclusiveAccessReservation => (Byte)((_rescap >> 2) & 0b1);
  public Byte WriteExclusiveRegistrantsOnlyReservation => (Byte)((_rescap >> 3) & 0b1);
  public Byte ExclusiveAccessRegistrantsOnlyReservation => (Byte)((_rescap >> 4) & 0b1);
  public Byte WriteExclusiveAllRegistrantsReservation => (Byte)((_rescap >> 5) & 0b1);
  public Byte ExclusiveAccessAllRegistrantsReservation => (Byte)((_rescap >> 6) & 0b1);
  // the most significant bit is reserved
  
  public Byte AsUchar => _rescap;
}