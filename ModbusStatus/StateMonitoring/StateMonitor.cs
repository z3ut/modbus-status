using ModbusStatus.StateMonitoring.DeviceStateReader;
using ModbusStatus.StateEvents;
using ModbusStatus.UI;
using ModbusStatus.UI.Shared;
using ModbusStatus.UI.Shared.WindowBorders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModbusStatus.StateMonitoring
{
    public class StateMonitor : IStateMonitor
    {
        private bool _isInited = false;

        private IStateDisplay _stateDisplay;
        private ICurrentState _currentState;

        public StateMonitor()
        {
            _stateDisplay =  new StateDisplay(new ConsoleExtensions(), new WindowBorderFancy());
            _currentState = new CurrentState(new DeviceStateReaderMoq());
        }

        public StateMonitor(IStateDisplay stateDisplay, ICurrentState currentState)
        {
            _stateDisplay = stateDisplay;
            _currentState = currentState;
        }

        public void Init(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _currentState.Init(ip, port, slaveAddress, startAddress, numberOfInputs);
            _stateDisplay.Initialize(ip, port, slaveAddress, startAddress, numberOfInputs);

            _currentState.OnNewState += _currentState_OnNewState;
            _currentState.OnStateChanges += _currentState_OnStateChanges;
            _currentState.OnGoneOnline += SetOnline;
            _currentState.OnGoneOffline += SetOffline;

            _isInited = true;
        }

        public void Start(int updatePeriod)
        {
            if (!_isInited)
            {
                throw new Exception("StateMonitor must be inited before update");
            }

            for (; ; )
            {
                _currentState.Update();
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
        }
    }
}
