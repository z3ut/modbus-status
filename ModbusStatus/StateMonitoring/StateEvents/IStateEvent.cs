using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring.StateEvents
{
    public interface IStateEvent
    {
        DateTime Date { get; }
        string Message { get; }
    }
}
