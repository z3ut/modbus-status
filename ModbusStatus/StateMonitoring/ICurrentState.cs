using ModbusStatus.StateMonitoring.StateEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring
{
    public interface ICurrentState
    {
        void Init(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs);
        void Update();

        event NewState OnChange;

        event Action OnGoneOnline;
        event Action OnGoneOffline;
    }

    public delegate void NewState(bool[] values);
}
