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
            Assert.AreEqual(0, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.Version)));
            Assert.AreEqual(4, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.Size)));
            Assert.AreEqual(8, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.DeviceType)));
            Assert.AreEqual(9, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.DeviceTypeModifier)));
            Assert.AreEqual(10, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.RemovableMedia)));
            Assert.AreEqual(11, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.CommandQueueing)));
            Assert.AreEqual(12, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.VendorIdOffset)));
            Assert.AreEqual(16, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.ProductIdOffset)));
            Assert.AreEqual(20, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.ProductRevisionOffset)));
            Assert.AreEqual(24, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.SerialNumberOffset)));
            Assert.AreEqual(28, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.BusType)));
            Assert.AreEqual(29, OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.RawPropertiesLength)));
            Assert.AreEqual(1064, Marshal.SizeOf(example));
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

        [Test]
        public void OfStorageProtocolSpecificDataIsFortyBytes()
        {
            // TODO: Confirm
            STORAGE_PROTOCOL_SPECIFIC_DATA example = default(STORAGE_PROTOCOL_SPECIFIC_DATA);
            Assert.AreEqual(40, Marshal.SizeOf(example));
        }

        [Test]
        public void OfStorageProtocolTypeIsFourBytes()
        {
            // TODO: Confirm
            Assert.AreEqual(4, sizeof(STORAGE_PROTOCOL_TYPE));
        }

        private int OffsetOf<T>(string fieldName)
        {
            return Marshal.OffsetOf(typeof(T), fieldName).ToInt32();
        }
    }
}