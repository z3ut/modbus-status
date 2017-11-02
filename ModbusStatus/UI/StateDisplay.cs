using ModbusStatus.StateEvents;
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

        private const int MAX_EVENT_COUNT = 100;
        private List<IStateEvent> _stateEvents = new List<IStateEvent>();

        private const string LOG_DATE_FORMAT = "yyyy-dd-MM HH.mm.ss";

        private const string ONLINE_TEXT = "ONLINE";
        private const string OFFLINE_TEXT = "OFFLINE";
        private int CONNECTION_STATUS_MAX_LENGTH = Math.Max(ONLINE_TEXT.Length, OFFLINE_TEXT.Length);

        private const ConsoleColor ONLINE_COLOR = ConsoleColor.Green;
        private const ConsoleColor ONLINE_BACKGROUND_COLOR = ConsoleColor.DarkGreen;

        private const ConsoleColor OFFLINE_COLOR = ConsoleColor.Gray;
        private const ConsoleColor OFFLINE_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        private const ConsoleColor STATE_COLOR = ConsoleColor.DarkMagenta;
        private const ConsoleColor STATE_BACKGROUND_COLOR = ConsoleColor.DarkYellow;

        private WindowPosition _statusTextWindow;
        private WindowPosition _stateTextWindow;
        private WindowPosition _logTextWindow;

        private const string TEXT_IP = "IP:";
        private const string TEXT_PORT = "PORT:";
        private const string TEXT_SLAVE_ADDRESS = "SLAVE ADDRESS:";
        private const string TEXT_START_ADDRESS = "START ADDRESS:";
        private const string TEXT_NUMBER_OF_INPUTS = "NUMBER OF INPUTS:";
        private const string TEXT_STATUS = "STATUS:";

        private const int STATUS_COLUMN_WIDTH = 18;

        private CursorPosition textIpPosition;
        private CursorPosition textPortPosition;
        private CursorPosition textSlaveAddressPosition;
        private CursorPosition textStartAddressPosition;
        private CursorPosition textNumberOfInputsPosition;
        private CursorPosition textConnectionStatusPosition;

        public StateDisplay(IConsoleExtensions consoleExtensions, IWindowBorders windowBorders)
        {
            _consoleExtensions = consoleExtensions;
            _windowBorder = windowBorders;
        }

        public void Initialize(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            _statusTextWindow = new WindowPosition(0, 0, Console.WindowWidth, 5, 1);
            _stateTextWindow = new WindowPosition(0, 4, 12, Console.WindowHeight - 5, 1);
            _logTextWindow = new WindowPosition(11, 4, Console.WindowWidth - 11,
                Console.WindowHeight - 5, 1);

            // first row
            textIpPosition = new CursorPosition(_statusTextWindow.ContentLeft, _statusTextWindow.ContentTop);
            textPortPosition = new CursorPosition(_statusTextWindow.ContentLeft + STATUS_COLUMN_WIDTH, _statusTextWindow.ContentTop);

            // second row
            textSlaveAddressPosition = new CursorPosition(_statusTextWindow.ContentLeft, _statusTextWindow.ContentTop + 1);
            textStartAddressPosition = new CursorPosition(_statusTextWindow.ContentLeft + STATUS_COLUMN_WIDTH, _statusTextWindow.ContentTop + 1);
            textNumberOfInputsPosition = new CursorPosition(_statusTextWindow.ContentLeft + STATUS_COLUMN_WIDTH * 2, _statusTextWindow.ContentTop + 1);

            // third row
            textConnectionStatusPosition = new CursorPosition(_statusTextWindow.ContentLeft, _statusTextWindow.ContentTop + 2);

            Console.CursorVisible = false;
            //Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.Clear();
            PrintUiBorders();
            PrintUiText();
            PrintConnectionText(ip, port, slaveAddress, startAddress, numberOfInputs);
        }

        public void AddLog(IStateEvent stateEvents)
        {
            AddLog(new List<IStateEvent>() { stateEvents });
        }

        public void AddLog(IEnumerable<IStateEvent> stateEvents)
        {
            _stateEvents.AddRange(stateEvents);
            _stateEvents = _stateEvents
                .Skip(Math.Max(0, _stateEvents.Count() - MAX_EVENT_COUNT))
                .ToList();

            ClearLog();

            var numberOfLogsToShow = _logTextWindow.ContentHeight;
            var displayEvents = _stateEvents.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < displayEvents.Count; i++)
            {
                var stateEvent = displayEvents[i];
                Console.SetCursorPosition(_logTextWindow.ContentLeft, _logTextWindow.ContentTop + i);
                Console.Write($"{stateEvent.Date.ToString(LOG_DATE_FORMAT)} {stateEvent.Message}");
            }
        }

        public void SetOffline()
        {
            PrintConnectionStatus(OFFLINE_TEXT, OFFLINE_COLOR, OFFLINE_BACKGROUND_COLOR);
        }

        public void SetOnline()
        {
            PrintConnectionStatus(ONLINE_TEXT, ONLINE_COLOR, ONLINE_BACKGROUND_COLOR);
        }

        public void SetState(bool[] state)
        {
            Console.BackgroundColor = STATE_COLOR;
            Console.ForegroundColor = STATE_BACKGROUND_COLOR;

            for (var i = 0; i < state.Length; i++)
            {
                Console.SetCursorPosition(_stateTextWindow.ContentLeft + 1, _stateTextWindow.ContentTop + i);
                Console.Write($"DI-{i.ToString("00")}: {Convert.ToInt32(state[i])}");
            }

            Console.ResetColor();
        }

        private void PrintUiBorders()
        {
            Console.ResetColor();

            _consoleExtensions.DrawForm(_statusTextWindow.BorderLeft, _statusTextWindow.BorderTop,
                _statusTextWindow.TotalWidth, _statusTextWindow.TotalHeight, _windowBorder);

            _consoleExtensions.DrawForm(_stateTextWindow.BorderLeft, _stateTextWindow.BorderTop,
                _stateTextWindow.TotalWidth, _stateTextWindow.TotalHeight, _windowBorder,
                topLeftSymbol: _windowBorder.VerticalAndRightSymbol);

            _consoleExtensions.DrawForm(_logTextWindow.BorderLeft,
                _logTextWindow.BorderTop, _logTextWindow.TotalWidth,
                _logTextWindow.TotalHeight, _windowBorder,
                _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                topLeftSymbol: _windowBorder.HorizontalAndBottomSymbol,
                topRightSymbol: _windowBorder.VerticalAndLeftSymbol,
                bottomLeftSymbol: _windowBorder.HorizontalAndTopSymbol);
        }

        private void PrintUiText()
        {
            Console.ResetColor();

            Console.SetCursorPosition(textIpPosition.Left, textIpPosition.Top);
            Console.Write(TEXT_IP);

            Console.SetCursorPosition(textPortPosition.Left, textPortPosition.Top);
            Console.Write(TEXT_PORT);

            Console.SetCursorPosition(textSlaveAddressPosition.Left, textSlaveAddressPosition.Top);
            Console.Write(TEXT_SLAVE_ADDRESS);

            Console.SetCursorPosition(textStartAddressPosition.Left, textStartAddressPosition.Top);
            Console.Write(TEXT_START_ADDRESS);

            Console.SetCursorPosition(textNumberOfInputsPosition.Left, textNumberOfInputsPosition.Top);
            Console.Write(TEXT_NUMBER_OF_INPUTS);

            Console.SetCursorPosition(textConnectionStatusPosition.Left, textConnectionStatusPosition.Top);
            Console.Write(TEXT_STATUS);
        }

        private void PrintConnectionText(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            Console.ResetColor();

            Console.SetCursorPosition(textIpPosition.Left + TEXT_IP.Length, textIpPosition.Top);
            Console.Write(ip);

            Console.SetCursorPosition(textPortPosition.Left + TEXT_PORT.Length, textPortPosition.Top);
            Console.Write(port);

            Console.SetCursorPosition(textSlaveAddressPosition.Left + TEXT_SLAVE_ADDRESS.Length, textSlaveAddressPosition.Top);
            Console.Write(slaveAddress);

            Console.SetCursorPosition(textStartAddressPosition.Left + TEXT_START_ADDRESS.Length, textStartAddressPosition.Top);
            Console.Write(startAddress);

            Console.SetCursorPosition(textNumberOfInputsPosition.Left + TEXT_NUMBER_OF_INPUTS.Length, textNumberOfInputsPosition.Top);
            Console.Write(numberOfInputs);
        }

        private void ClearLog()
        {
            _consoleExtensions.ClearBox(_logTextWindow.ContentLeft, _logTextWindow.ContentTop,
                _logTextWindow.ContentWidth, _logTextWindow.ContentHeight);
        }

        private void PrintConnectionStatus(string status,
            ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            _consoleExtensions.ClearBox(textConnectionStatusPosition.Left + CONNECTION_STATUS_MAX_LENGTH, textConnectionStatusPosition.Top, CONNECTION_STATUS_MAX_LENGTH, 1);

            Console.SetCursorPosition(textConnectionStatusPosition.Left + CONNECTION_STATUS_MAX_LENGTH, textConnectionStatusPosition.Top);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(status);
            Console.ResetColor();
        }
    }
}
