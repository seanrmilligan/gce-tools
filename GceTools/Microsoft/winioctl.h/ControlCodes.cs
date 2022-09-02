namespace Google.Cloud.Storage.Windows.winioctl.h;

public class ControlCodes
{
    
    public static readonly uint IOCTL_STORAGE_QUERY_PROPERTY =
        CTL_CODE(IOCTL_STORAGE_BASE, 0x0500, METHOD_BUFFERED, FILE_ANY_ACCESS);
    
    private const uint FILE_ANY_ACCESS = 0;
    private const uint FILE_DEVICE_MASS_STORAGE = 0x0000002d;
    private const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;
    private const uint METHOD_BUFFERED = 0;
    private static uint CTL_CODE (uint deviceType, uint function, uint method, uint access) =>
        (deviceType << 16) | (access << 14) | (function << 2) | method;
}