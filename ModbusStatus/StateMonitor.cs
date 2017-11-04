using ModbusStatus.Device;
using ModbusStatus.Device.DeviceStateReader;
using ModbusStatus.StateEvents;
using ModbusStatus.UI;
using ModbusStatus.UI.Shared;
using ModbusStatus.UI.Shared.WindowBorders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModbusStatus
{
    public class StateMonitor : IStateMonitor
    {
        private bool _isInited = false;

        private IStateDisplay _stateDisplay;
        private IDeviceCurrentState _deviceCurrentState;

        public StateMonitor(IStateDisplay stateDisplay, IDeviceCurrentState currentState)
        {
            _stateDisplay = stateDisplay;
            _deviceCurrentState = currentState;
        }

        public void Initialize(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _deviceCurrentState.Initialize(ip, port, slaveAddress, startAddress, numberOfInputs);
            _stateDisplay.Initialize(ip, port, slaveAddress, startAddress, numberOfInputs);

            _deviceCurrentState.OnNewState += _currentState_OnNewState;
            _deviceCurrentState.OnStateChanges += _currentState_OnStateChanges;
            _deviceCurrentState.OnGoneOnline += SetOnline;
            _deviceCurrentState.OnGoneOffline += SetOffline;

            _isInited = true;
        }

        public void StartSync(int updatePeriod)
        {
            if (!_isInited)
            {
                throw new Exception("StateMonitor must be inited before update");
            }

            for (; ; )
            {
                _deviceCurrentState.Update();
                Thread.Sleep(updatePeriod);
            }
        }

        private void _currentState_OnStateChanges(IDictionary<int, bool> changes)
        {
            _stateDisplay.AddLog(changes.Select(c => new InputChange(c.Key, c.Value, DateTime.Now)));
        }

        private void _currentState_OnNewState(bool[] values)
        {
            _stateDisplay.SetState(values);
        }

        void SetOnline()
        {
            _stateDisplay.AddLog(new GoneOnline(DateTime.Now));
            _stateDisplay.SetOnline();
        }

        void SetOffline()
        {
            _stateDisplay.AddLog(new GoneOffline(DateTime.Now));
            _stateDisplay.SetOffline();
        }
    }
}
