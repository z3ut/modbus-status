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
        string _ip;
        int _port;
        int _slaveAddress;
        int _startAddress;
        int _numberOfInputs;
        int _updatePeriod;

        bool[] _previousState;
        bool _isFirstTimeRequest = true;
        bool _isOnline = false;

        private List<IStateEvent> stateEvents = new List<IStateEvent>();

        private IStateDisplay stateDisplay = new StateDisplay(new ConsoleExtensions(), new WindowBorderFancy());
        private IDeviceStateReader _deviceStateReader;

        public StateMonitor(int updatePeriod, string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _updatePeriod = updatePeriod;
            _ip = ip;
            _port = port;
            _slaveAddress = slaveAddress;
            _startAddress = startAddress;
            _numberOfInputs = numberOfInputs;

            _deviceStateReader = new DeviceStateReaderMoq();
        }

        public void Start()
        {
            stateDisplay.Initialize(_ip, _port, _slaveAddress, _startAddress, _numberOfInputs);

            for (; ; )
            {
                CollectAndDisplayState(_ip, _port, _slaveAddress, _startAddress, _numberOfInputs);
                Thread.Sleep(_updatePeriod);
            }
        }

        void CollectAndDisplayState(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            try
            {
                var currentState = _deviceStateReader.ReadValues(ip, port, slaveAddress, startAddress, numberOfInputs);

                if (!_isOnline || _isFirstTimeRequest)
                {
                    _isFirstTimeRequest = false;
                    SetOnline();
                }

                var stateDifferenceEvents = GetStateDifferenceEvents(_previousState, currentState);

                if (stateDifferenceEvents.Any())
                {
                    stateDisplay.SetState(currentState);
                    stateEvents.AddRange(stateDifferenceEvents);
                    RedrawLog();
                }

                _previousState = currentState;
            }
            catch (Exception)
            {
                if (_isOnline)
                {
                    SetOffline();
                }
            }
        }

        IEnumerable<IStateEvent> GetStateDifferenceEvents(bool[] previousState, bool[] currentState)
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
            _isOnline = true;
            stateEvents.Add(new GoneOnline(DateTime.Now));
            stateDisplay.SetOnline();
            RedrawLog();
        }

        void SetOffline()
        {
            _isOnline = false;
            stateEvents.Add(new GoneOffline(DateTime.Now));
            stateDisplay.SetOffline();
            RedrawLog();
        }

        void RedrawLog()
        {
            stateDisplay.SetLog(stateEvents);
        }
    }
}
