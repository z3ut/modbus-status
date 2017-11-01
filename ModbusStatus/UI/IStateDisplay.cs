﻿using ModbusStatus.StateMonitoring.StateEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI
{
    public interface IStateDisplay
    {
        void Initialize(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs);
        void SetState(bool[] state);
        void AddLog(IStateEvent stateEvent);
        void AddLog(IEnumerable<IStateEvent> stateEvents);
        void SetOnline();
        void SetOffline();
    }
}
