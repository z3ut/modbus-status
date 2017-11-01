using ModbusStatus.StateMonitoring.DeviceStateReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.StateMonitoring
{
    class CurrentState : ICurrentState
    {
        public event NewState OnNewState;
        public event Action OnGoneOnline;
        public event Action OnGoneOffline;
        public event StateChanges OnStateChanges;

        private string _ip;
        private int _port;
        private int _slaveAddress;
        private int _startAddress;
        private int _numberOfInputs;

        private readonly IDeviceStateReader _deviceStateReader;

        private bool _isInited = false;

        private bool[] _currentState;
        private bool _isFirstUpdate = true;
        private bool _isOnline = false;

        public CurrentState(IDeviceStateReader deviceStateReader)
        {
            _deviceStateReader = deviceStateReader;
        }

        public void Init(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _ip = ip;
            _port = port;
            _slaveAddress = slaveAddress;
            _startAddress = startAddress;
            _numberOfInputs = numberOfInputs;

            _isInited = true;
        }

        public void Update()
        {
            if (!_isInited)
            {
                throw new Exception("CurrentState must be inited before update");
            }

            try
            {
                var newState = _deviceStateReader.ReadValues(_ip, _port,
                    _slaveAddress, _startAddress, _numberOfInputs);

                if (!_isOnline)
                {
                    _isOnline = true;
                    OnGoneOnline?.Invoke();
                }

                if (_currentState == null || !_currentState.SequenceEqual(newState))
                {
                    var stateDifferences = GetStateDifferences(_currentState, newState);
                    _currentState = newState;
                    OnNewState?.Invoke(_currentState);
                    OnStateChanges?.Invoke(stateDifferences);
                }
            }
            catch (Exception)
            {
                if (_isOnline || _isFirstUpdate)
                {
                    _isOnline = false;
                    OnGoneOffline?.Invoke();
                }
            }

            _isFirstUpdate = false;
        }

        private IDictionary<int, bool> GetStateDifferences(
            bool[] previousState, bool[] currentState)
        {
            var changes = new Dictionary<int, bool>();
            for (var i = 0; i < currentState.Length; i++)
            {
                if (previousState == null || previousState[i] != currentState[i])
                {
                    changes.Add(i, currentState[i]);
                }
            }
            return changes;
        }
    }
}
