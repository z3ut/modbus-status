using ModbusStatus.UI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Components
{
    public interface IStateComponent
    {
        void Initialize(FormPosition formPosition);
        void SetState(bool[] state);
    }
}
