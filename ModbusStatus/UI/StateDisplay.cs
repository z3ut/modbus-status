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

        private const ConsoleColor ONLINE_COLOR = ConsoleColor.Green;
        private const ConsoleColor ONLINE_BACKGROUND_COLOR = ConsoleColor.DarkGreen;

        private const ConsoleColor OFFLINE_COLOR = ConsoleColor.Gray;
        private const ConsoleColor OFFLINE_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        private const ConsoleColor STATE_COLOR = ConsoleColor.DarkMagenta;
        private const ConsoleColor STATE_BACKGROUND_COLOR = ConsoleColor.DarkYellow;

        public StateDisplay(IConsoleExtensions consoleExtensions, IWindowBorders windowBorders)
        {
            _consoleExtensions = consoleExtensions;
            _windowBorder = windowBorders;
        }

        public void Initialize(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
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

            var numberOfLogsToShow = Console.WindowHeight - 2 - 5;
            var displayEvents = _stateEvents.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < displayEvents.Count; i++)
            {
                var stateEvent = displayEvents[i];
                Console.SetCursorPosition(12, 5 + i);
                Console.Write($"{stateEvent.Date.ToString(LOG_DATE_FORMAT)} {stateEvent.Message}");
            }
        }

        public void SetOffline()
        {
            ClearConnectionStatus();
            PrintConnectionStatus(OFFLINE_TEXT, OFFLINE_COLOR, OFFLINE_BACKGROUND_COLOR);
        }

        public void SetOnline()
        {
            ClearConnectionStatus();
            PrintConnectionStatus(ONLINE_TEXT, ONLINE_COLOR, ONLINE_BACKGROUND_COLOR);
        }

        public void SetState(bool[] state)
        {
            Console.BackgroundColor = STATE_COLOR;
            Console.ForegroundColor = STATE_BACKGROUND_COLOR;

            for (var i = 0; i < state.Length; i++)
            {
                Console.SetCursorPosition(2, i + 5);
                Console.Write($"DI-{i.ToString("00")}: {Convert.ToInt32(state[i])}");
            }

            Console.ResetColor();
        }

        private void PrintUiBorders()
        {
            Console.ResetColor();

            _consoleExtensions.DrawForm(0, 0, Console.WindowWidth, 5, _windowBorder);

            _consoleExtensions.DrawForm(0, 4, 12, Console.WindowHeight - 5,
                _windowBorder, topLeftSymbol: _windowBorder.VerticalAndRightSymbol);

            _consoleExtensions.DrawForm(11, 4, Console.WindowWidth - 11,
                Console.WindowHeight - 5, _windowBorder,
                _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                topLeftSymbol: _windowBorder.HorizontalAndBottomSymbol,
                topRightSymbol: _windowBorder.VerticalAndLeftSymbol,
                bottomLeftSymbol: _windowBorder.HorizontalAndTopSymbol);
        }

        private void PrintUiText()
        {
            Console.ResetColor();

            Console.SetCursorPosition(1, 1);
            Console.Write("IP:");

            Console.SetCursorPosition(20, 1);
            Console.Write("PORT:");

            Console.SetCursorPosition(1, 2);
            Console.Write("START ADDRESS:");

            Console.SetCursorPosition(20, 2);
            Console.Write("NUMBER OF INPUTS:");

            Console.SetCursorPosition(1, 3);
            Console.Write("STATUS:");
        }

        private void PrintConnectionText(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            Console.ResetColor();

            Console.SetCursorPosition(4, 1);
            Console.Write(ip);

            Console.SetCursorPosition(25, 1);
            Console.Write(port);

            // TODO: slave address

            Console.SetCursorPosition(15, 2);
            Console.Write(startAddress);

            Console.SetCursorPosition(37, 2);
            Console.Write(numberOfInputs);
        }

        private void ClearLog()
        {
            _consoleExtensions.ClearBox(12, 5, Console.WindowWidth - 13,
                Console.WindowHeight - 7);
        }

        private void ClearConnectionStatus()
        {
            var clearLength = Math.Max(ONLINE_TEXT.Length, OFFLINE_TEXT.Length);
            _consoleExtensions.ClearBox(8, 3, clearLength, 1);
        }

        private void PrintConnectionStatus(string status,
            ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.SetCursorPosition(8, 3);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(status);
            Console.ResetColor();
        }
    }
}
