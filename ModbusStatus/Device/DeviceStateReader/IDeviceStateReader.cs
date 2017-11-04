using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.Device.DeviceStateReader
{
    public interface IDeviceStateReader
    {
        bool[] ReadValues(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs);
    }
}
