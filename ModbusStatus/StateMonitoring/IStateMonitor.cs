using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring
{
    public interface IStateMonitor
    {
        void Init(int updatePeriod, string ip, int port, int slaveAddress, int startAddress, int numberOfInputs);
        void Start(int updatePeriod);
    }
}
