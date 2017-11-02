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
        private IConsoleExtensions _consoleExtensions;
        private IWindowBorders _windowBorder;

        private FormPosition _statusTextForm;
        private FormPosition _stateTextForm;
        private FormPosition _logTextForm;

        private ILogComponent _logComponent;
        private IStateComponent _stateComponent;
        private IStatusComponent _statusComponent;

        public StateDisplay(IConsoleExtensions consoleExtensions, IWindowBorders windowBorders)
        {
            _consoleExtensions = consoleExtensions;
            _windowBorder = windowBorders;

            _logComponent = new LogComponent(consoleExtensions);
            _stateComponent = new StateComponent(consoleExtensions);
            _statusComponent = new StatusComponent(consoleExtensions);
        }

        public void Initialize(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            Console.Clear();

            _statusTextForm = new FormPosition(0, 0, Console.WindowWidth, 5, 1);
            _stateTextForm = new FormPosition(0, 4, 12, Console.WindowHeight - 5, 1);
            _logTextForm = new FormPosition(11, 4, Console.WindowWidth - 11,
                Console.WindowHeight - 5, 1);

            _logComponent.Initialize(_logTextForm);
            _stateComponent.Initialize(_stateTextForm);
            _statusComponent.Initialize(_statusTextForm, ip, port, slaveAddress, startAddress, numberOfInputs);
            
            Console.CursorVisible = false;
            //Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            
            PrintUiBorders();
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

        public void SetState(bool[] state)
        {
            _stateComponent.SetState(state);
        }

        private void PrintUiBorders()
        {
            Console.ResetColor();

            _consoleExtensions.DrawForm(_statusTextForm.BorderLeft, _statusTextForm.BorderTop,
                _statusTextForm.TotalWidth, _statusTextForm.TotalHeight, _windowBorder);

            _consoleExtensions.DrawForm(_stateTextForm.BorderLeft, _stateTextForm.BorderTop,
                _stateTextForm.TotalWidth, _stateTextForm.TotalHeight, _windowBorder,
                topLeftSymbol: _windowBorder.VerticalAndRightSymbol);

            _consoleExtensions.DrawForm(_logTextForm.BorderLeft,
                _logTextForm.BorderTop, _logTextForm.TotalWidth,
                _logTextForm.TotalHeight, _windowBorder,
                _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                topLeftSymbol: _windowBorder.HorizontalAndBottomSymbol,
                topRightSymbol: _windowBorder.VerticalAndLeftSymbol,
                bottomLeftSymbol: _windowBorder.HorizontalAndTopSymbol);
        }
    }
}
