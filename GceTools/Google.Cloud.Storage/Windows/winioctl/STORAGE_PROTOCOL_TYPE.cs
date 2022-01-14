namespace Google.Cloud.Storage.Windows.winioctl
{
    public enum STORAGE_PROTOCOL_TYPE
    {
        ProtocolTypeUnknown,
        ProtocolTypeScsi,
        ProtocolTypeAta,
        ProtocolTypeNvme,
        ProtocolTypeSd,
        ProtocolTypeUfs,
        ProtocolTypeProprietary,
        ProtocolTypeMaxReserved
    }
}