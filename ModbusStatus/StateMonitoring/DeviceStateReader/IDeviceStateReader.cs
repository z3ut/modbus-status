using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring.DeviceStateReader
{
    public interface IDeviceStateReader
    {
        bool[] ReadValues(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs);
    }
}
