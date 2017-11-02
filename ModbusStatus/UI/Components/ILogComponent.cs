using ModbusStatus.StateEvents;
using ModbusStatus.UI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Components
{
    public interface ILogComponent
    {
        void Initialize(FormPosition formPosition);
        void AddLog(IStateEvent stateEvent);
        void AddLog(IEnumerable<IStateEvent> stateEvents);
    }
}
