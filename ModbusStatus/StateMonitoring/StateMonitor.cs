using ModbusStatus.StateMonitoring.DeviceStateReader;
using ModbusStatus.StateMonitoring.StateEvents;
using ModbusStatus.UI;
using ModbusStatus.UI.WindowBorders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModbusStatus.StateMonitoring
{
    public class StateMonitor : IStateMonitor
    {
        private bool[] _currentValues;
        private bool _isInited = false;

        private List<IStateEvent> stateEvents = new List<IStateEvent>();

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

        public void Init(int updatePeriod, string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _currentState.Init(ip, port, slaveAddress, startAddress, numberOfInputs);
            _stateDisplay.Initialize(ip, port, slaveAddress, startAddress, numberOfInputs);

            _currentState.OnChange += _currentState_OnChange;
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

        private void _currentState_OnChange(bool[] values)
        {
            var stateDifferenceEvents = GetStateDifferenceEvents(_currentValues, values);

            if (stateDifferenceEvents.Any())
            {
                _currentValues = values;
                _stateDisplay.SetState(_currentValues);
                stateEvents.AddRange(stateDifferenceEvents);
                RedrawLog();
            }
        }

        private IEnumerable<IStateEvent> GetStateDifferenceEvents(bool[] previousState, bool[] currentState)
        {
            for (var i = 0; i < currentState.Length; i++)
            {
                if (previousState == null || previousState[i] != currentState[i])
                {
                    yield return (new InputChange(i, currentState[i], DateTime.Now));
                }
            }
        }

        void SetOnline()
        {
            stateEvents.Add(new GoneOnline(DateTime.Now));
            _stateDisplay.SetOnline();
            RedrawLog();
        }

        void SetOffline()
        {
            stateEvents.Add(new GoneOffline(DateTime.Now));
            _stateDisplay.SetOffline();
            RedrawLog();
        }

        void RedrawLog()
        {
            _stateDisplay.SetLog(stateEvents);
        }
    }
}
