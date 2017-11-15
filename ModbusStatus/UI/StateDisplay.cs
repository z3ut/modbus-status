using ModbusStatus.StateEvents;
using ModbusStatus.UI.Components;
using ModbusStatus.UI.Shared;
using ModbusStatus.UI.Shared.WindowBorders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.UI
{
    public class StateDisplay : IStateDisplay
    {
        private const int STATUS_HEIGHT = 5;
        private const int VALUES_WIDTH = 12;
        private const int BORDER_WIDTH = 1;

        private readonly IConsoleExtensions _consoleExtensions;
        private readonly IWindowBorders _windowBorder;

        private readonly ILogComponent _logComponent;
        private readonly IValuesComponent _valuesComponent;
        private readonly IStatusComponent _statusComponent;

        private FormPosition _statusFormPosition;
        private FormPosition _valuesFormPosition;
        private FormPosition _logFormPosition;

        private string _ip;
        private int _port;
        private int _slaveAddress;
        private int _startAddress;
        private int _numberOfInputs;

        public StateDisplay(IConsoleExtensions consoleExtensions,
            IWindowBorders windowBorders, ILogComponent logComponent,
            IValuesComponent valuesComponent, IStatusComponent statusComponent)
        {
            _consoleExtensions = consoleExtensions;
            _windowBorder = windowBorders;

            _logComponent = logComponent;
            _valuesComponent = valuesComponent;
            _statusComponent = statusComponent;
        }

        public void Initialize(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            _ip = ip;
            _port = port;
            _slaveAddress = slaveAddress;
            _startAddress = startAddress;
            _numberOfInputs = numberOfInputs;

            InitializeConsoleSettings();
            InitializeFormPositions();
            PrintUiBorders();
            InitializeComponents();
        }

        public void AddLog(IStateEvent stateEvent)
        {
            _logComponent.AddLog(stateEvent);
        }

        public void AddLog(IEnumerable<IStateEvent> stateEvents)
        {
            _logComponent.AddLog(stateEvents);
        }

        public void SetOffline()
        {
            _statusComponent.SetOffline();
        }

        public void SetOnline()
        {
            _statusComponent.SetOnline();
        }

        public void SetValues(bool[] values)
        {
            _valuesComponent.SetValues(values);
        }

        private void InitializeConsoleSettings()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.Title = $"ModbusStatus {_ip}";
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        private void InitializeFormPositions()
        {
            int secondRowTopPosition = STATUS_HEIGHT - 1;
            int logLeftPosition = VALUES_WIDTH - 1;

            _statusFormPosition = new FormPosition(0, 0, Console.WindowWidth,
                STATUS_HEIGHT, BORDER_WIDTH);
            _valuesFormPosition = new FormPosition(0, secondRowTopPosition,
                VALUES_WIDTH, Console.WindowHeight - STATUS_HEIGHT, BORDER_WIDTH);
            _logFormPosition = new FormPosition(logLeftPosition,
                secondRowTopPosition, Console.WindowWidth - logLeftPosition,
                Console.WindowHeight - STATUS_HEIGHT, BORDER_WIDTH);
        }

        private void PrintUiBorders()
        {
            Console.ResetColor();

            _consoleExtensions.DrawForm(_statusFormPosition.BorderLeft,
                _statusFormPosition.BorderTop, _statusFormPosition.TotalWidth,
                _statusFormPosition.TotalHeight, _windowBorder);

            _consoleExtensions.DrawForm(_valuesFormPosition.BorderLeft,
                _valuesFormPosition.BorderTop, _valuesFormPosition.TotalWidth,
                _valuesFormPosition.TotalHeight, _windowBorder,
                topLeftSymbol: _windowBorder.VerticalAndRightSymbol);

            _consoleExtensions.DrawForm(_logFormPosition.BorderLeft,
                _logFormPosition.BorderTop, _logFormPosition.TotalWidth,
                _logFormPosition.TotalHeight, _windowBorder,
                _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                topLeftSymbol: _windowBorder.HorizontalAndBottomSymbol,
                topRightSymbol: _windowBorder.VerticalAndLeftSymbol,
                bottomLeftSymbol: _windowBorder.HorizontalAndTopSymbol);
        }

        private void InitializeComponents()
        {
            _logComponent.Initialize(_logFormPosition);
            _valuesComponent.Initialize(_valuesFormPosition);
            _statusComponent.Initialize(_statusFormPosition, _ip, _port,
                _slaveAddress, _startAddress, _numberOfInputs);
        }
    }
}
