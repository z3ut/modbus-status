using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.StateMonitoring.DeviceStateReader
{
    public class DeviceStateReaderMoq : IDeviceStateReader
    {
        private readonly int _numberOfInputs;

        public DeviceStateReaderMoq(int numberOfInputs)
        {
            _numberOfInputs = numberOfInputs;
        }

        public bool[] ReadValues()
        {
            var gen = new Random();
            if (gen.Next(100) > 90)
            {
                throw new Exception();
            }

            return Enumerable.Range(0, _numberOfInputs)
                .Select(s => gen.Next(100) > 50)
                .ToArray();
        }
    }
}
