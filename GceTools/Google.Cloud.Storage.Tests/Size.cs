using System.Runtime.InteropServices;
using Google.Cloud.Storage.Windows.winioctl;
using NUnit.Framework;

namespace Google.Cloud.Storage.Tests
{
    [TestFixture]
    public class Size
    {
        [Test]
        public void OfBusTypeIsOneByte()
        {
            Assert.AreEqual(1, sizeof(STORAGE_BUS_TYPE));
        }

        [Test]
        public void OfStorageAdapterDescriptorIsThirtyTwoBytes()
        {
            STORAGE_ADAPTER_DESCRIPTOR example = default(STORAGE_ADAPTER_DESCRIPTOR);
            Assert.AreEqual(32, Marshal.SizeOf(example));
        }

        [Test]
        public void OfStorageDeviceDescriptorIsFortyBytes()
        {
            // The size of the base struct is 40 bytes, including a byte array
            // of size 1. In order to avoid the complexities of marhsalling
            // variable size arrays, we declare the array at the end to be 1024
            // bytes, making the struct marshal as 1023 bytes larger than
            // expected per the Microsoft documentation.
            //
            // Therefore, the expected size is 40 bytes + 1023 bytes = 1063 bytes
            STORAGE_DEVICE_DESCRIPTOR example = default(STORAGE_DEVICE_DESCRIPTOR);
            Assert.AreEqual(1063, Marshal.SizeOf(example));
        }

        [Test]
        public void OfStorageDeviceIdDescriptorIsThirteenBytes()
        {
            // The size of the base struct is 13 bytes, including a byte array
            // of size 1. In order to avoid the complexities of marhsalling
            // variable size arrays, we declare the array at the end to be 512
            // bytes, making the struct marshal as 511 bytes larger than
            // expected per the Microsoft documentation.
            //
            // Therefore, the expected size is 13 bytes + 511 bytes = 524 bytes
            STORAGE_DEVICE_ID_DESCRIPTOR example = default(STORAGE_DEVICE_ID_DESCRIPTOR);
            Assert.AreEqual(524, Marshal.SizeOf(example));
        }

        public void OfStoragePropertyQueryIs_Bytes()
        {
            
        }
    }
}