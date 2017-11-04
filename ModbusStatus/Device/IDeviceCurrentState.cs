using ModbusStatus.StateEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.Device
{
    public interface IDeviceCurrentState
    {
        void Initialize(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs);
        void Update();

        event NewState OnNewState;

        event Action OnGoneOnline;
        event Action OnGoneOffline;

        event StateChanges OnStateChanges;
    }

    public delegate void NewState(bool[] values);
    public delegate void StateChanges(IDictionary<int, bool> changes);
}
