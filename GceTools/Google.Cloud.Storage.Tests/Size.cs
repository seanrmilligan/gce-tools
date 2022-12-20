/* Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     https://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Runtime.InteropServices;
using Microsoft.Native.winioctl.h;
using NUnit.Framework;

namespace Google.Cloud.Storage.Tests
{
    [TestFixture]
    public class Size
    {
        [Test]
        public void OfBusTypeIsOneByte()
        {
            Assert.That(sizeof(STORAGE_BUS_TYPE), Is.EqualTo(1));
        }

        [Test]
        public void OfStorageAdapterDescriptorIsThirtyTwoBytes()
        {
            STORAGE_ADAPTER_DESCRIPTOR example = default;
            Assert.That(Marshal.SizeOf(example), Is.EqualTo(32));
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
            STORAGE_DEVICE_DESCRIPTOR example = default;
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.Version)), Is.EqualTo(0));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.Size)), Is.EqualTo(4));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.DeviceType)), Is.EqualTo(8));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.DeviceTypeModifier)), Is.EqualTo(9));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.RemovableMedia)), Is.EqualTo(10));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.CommandQueueing)), Is.EqualTo(11));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.VendorIdOffset)), Is.EqualTo(12));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.ProductIdOffset)), Is.EqualTo(16));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.ProductRevisionOffset)), Is.EqualTo(20));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.SerialNumberOffset)), Is.EqualTo(24));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.BusType)), Is.EqualTo(28));
            Assert.That(OffsetOf<STORAGE_DEVICE_DESCRIPTOR>(
                nameof(STORAGE_DEVICE_DESCRIPTOR.RawPropertiesLength)), Is.EqualTo(29));
            Assert.That(Marshal.SizeOf(example), Is.EqualTo(1064));
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
            STORAGE_DEVICE_ID_DESCRIPTOR example = default;
            Assert.That(Marshal.SizeOf(example), Is.EqualTo(524));
        }

        public void OfStoragePropertyQueryIs_Bytes()
        {
            
        }

        [Test]
        public void OfStorageProtocolSpecificDataIsFortyBytes()
        {
            // TODO: Confirm
            STORAGE_PROTOCOL_SPECIFIC_DATA example = default;
            Assert.That(Marshal.SizeOf(example), Is.EqualTo(40));
        }

        [Test]
        public void OfStorageProtocolTypeIsFourBytes()
        {
            // TODO: Confirm
            Assert.That(sizeof(STORAGE_PROTOCOL_TYPE), Is.EqualTo(4));
        }

        private int OffsetOf<T>(string fieldName)
        {
            return Marshal.OffsetOf(typeof(T), fieldName).ToInt32();
        }
    }
}