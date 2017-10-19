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

        public StateMonitor(int updatePeriod, string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _updatePeriod = updatePeriod;
            _ip = ip;
            _port = port;
            _slaveAddress = slaveAddress;
            _startAddress = startAddress;
            _numberOfInputs = numberOfInputs;
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

        private void UpdateDeviceTimerCall(object state)
        {
            CollectAndDisplayState(_ip, _port, _slaveAddress, _startAddress, _numberOfInputs);
        }

        void CollectAndDisplayState(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            try
            {
                var currentState = GetValues(ip, port, slaveAddress, startAddress, numberOfInputs);

                var stateDifferenceEvents = GetStateDifferenceEvents(_previousState, currentState);

                if (stateDifferenceEvents.Any())
                {
                    stateDisplay.SetState(currentState);
                    stateEvents.AddRange(stateDifferenceEvents);
                    RedrawLog();
                }

                _previousState = currentState;

                if (!_isOnline || _isFirstTimeRequest)
                {
                    _isFirstTimeRequest = false;
                    SetOnline();
                }
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

        bool[] GetValues(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
#if DEBUG
            var gen = new Random();
            if (gen.Next(100) > 90)
            {
                throw new Exception();
            }

            return Enumerable.Range(0, numberOfInputs)
                .Select(s => gen.Next(100) > 50)
                .ToArray();
#else
            using (var client = new TcpClient(ip, port))
            {
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);
                return master.ReadInputs(slaveAddress, (ushort)startAddress, (ushort)numberOfInputs);
            }
#endif
        }

        void SetOnline()
        {
            _isOnline = true;
            stateEvents.Add(new GoneOnline(DateTime.Now));
            stateDisplay.SetOffline();
            RedrawLog();
        }

        void SetOffline()
        {
            _isOnline = false;
            stateEvents.Add(new GoneOffline(DateTime.Now));
            stateDisplay.SetOnline();
            RedrawLog();
        }

        void RedrawLog()
        {
            stateDisplay.SetLog(stateEvents);
        }
    }
}
