using ModbusStatus.StateEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI
{
    public interface IStateDisplay
    {
        void Initialize(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs);
        
        void AddLog(IStateEvent stateEvent);
        void AddLog(IEnumerable<IStateEvent> stateEvents);

        void SetValues(bool[] state);
        void SetOnline();
        void SetOffline();
    }
}
