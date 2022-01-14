namespace Google.Cloud.Storage.Windows.winioctl
{
    public enum STORAGE_BUS_TYPE
    {
        BusTypeUnknown,
        BusTypeScsi,
        BusTypeAtapi,
        BusTypeAta,
        BusType1394,
        BusTypeSsa,
        BusTypeFibre,
        BusTypeUsb,
        BusTypeRAID,
        BusTypeiScsi,
        BusTypeSas,
        BusTypeSata,
        BusTypeSd,
        BusTypeMmc,
        BusTypeVirtual,
        BusTypeFileBackedVirtual,
        BusTypeSpaces,
        BusTypeNvme,
        BusTypeSCM,
        BusTypeUfs,
        BusTypeMax,
        BusTypeMaxReserved
    }
}