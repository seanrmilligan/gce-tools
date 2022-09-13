using System.Text;
using Microsoft.Windows.winioctl.h;

namespace Google.Cloud.Storage.Models;

public class StorageIdentifier
{
    /// <summary>
    /// Specifies the code set used by a SCSI identification descriptor to
    /// identify a logical unit.
    /// </summary>
    public STORAGE_IDENTIFIER_CODE_SET CodeSet;
        
    /// <summary>
    /// Contains an enumerator value of type
    /// <see cref="Microsoft.Windows.winioctl.h.STORAGE_IDENTIFIER_TYPE"/> that indicates the identifier
    /// type.
    /// </summary>
    public STORAGE_IDENTIFIER_TYPE Type;
        
    /// <summary>
    /// Specifies the size in bytes of the identifier.
    /// </summary>
    public ushort IdentifierSize;
        
    /// <summary>
    /// Specifies the offset in bytes from the current descriptor to the
    /// next descriptor.
    /// </summary>
    public ushort NextOffset;
        
    /// <summary>
    /// Contains an enumerator value of type
    /// <see cref="Microsoft.Windows.winioctl.h.STORAGE_ASSOCIATION_TYPE"/> that indicates whether the
    /// descriptor identifies a device or a port.
    /// </summary>
    public STORAGE_ASSOCIATION_TYPE Association;

    /// <summary>
    /// Contains the identifier associated with this descriptor.
    /// </summary>
    public byte[] Identifier { get; set; }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"CodeSet:               {CodeSet}",
            $"Type:                  {Type}",
            $"IdentifierSize:        {IdentifierSize}",
            $"NextOffset:            {NextOffset}",
            $"Association:           {Association}",
            $"Identifier:            {Encoding.ASCII.GetString(Identifier)}"
        });
    }
}