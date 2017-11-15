using ModbusStatus.UI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Components
{
    public interface IStatusComponent
    {
        void Initialize(FormPosition formPosition, string ip, int port,
            int slaveAddress, int startAddress, int numberOfInputs);

        void SetOnline();
        void SetOffline();
    }
}
