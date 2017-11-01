using ModbusStatus.StateMonitoring.DeviceStateReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.StateMonitoring
{
    class CurrentState : ICurrentState
    {
        public event NewState OnChange;
        public event Action OnGoneOnline;
        public event Action OnGoneOffline;

        private string _ip;
        private int _port;
        private int _slaveAddress;
        private int _startAddress;
        private int _numberOfInputs;
        private int _updatePeriod;

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
                    _currentState = newState;
                    OnChange?.Invoke(_currentState);
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
    }
}
