using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring
{
    public interface IStateMonitor
    {
        void Initialize(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs);
        void StartSync(int updatePeriod);
    }
}
