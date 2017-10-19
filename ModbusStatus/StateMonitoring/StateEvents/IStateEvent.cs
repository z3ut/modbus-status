using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus
{
    public interface IStateEvent
    {
        DateTime Date { get; }
        string Message { get; }
    }
}
