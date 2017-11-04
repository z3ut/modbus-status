using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.Device.DeviceStateReader
{
    public class DeviceStateReaderMoq : IDeviceStateReader
    {
        public bool[] ReadValues(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            var gen = new Random();
            if (gen.Next(100) > 90)
            {
                throw new Exception();
            }

            return Enumerable.Range(0, numberOfInputs)
                .Select(s => gen.Next(100) > 50)
                .ToArray();
        }
    }
}
