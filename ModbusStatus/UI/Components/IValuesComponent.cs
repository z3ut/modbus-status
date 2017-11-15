using ModbusStatus.UI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Components
{
    public interface IValuesComponent
    {
        void Initialize(FormPosition formPosition);
        void SetValues(bool[] values);
    }
}
