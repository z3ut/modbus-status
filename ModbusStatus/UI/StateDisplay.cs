using ModbusStatus.StateMonitoring.StateEvents;
using ModbusStatus.UI.WindowBorders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModbusStatus.UI
{
    public class StateDisplay : IStateDisplay
    {

        private IConsoleExtensions _consoleExtensions = new ConsoleExtensions();
        private IWindowBorders _windowBorder = new WindowBorderFancy();

        public StateDisplay(IConsoleExtensions consoleExtensions, IWindowBorders windowBorders)
        {
            _consoleExtensions = consoleExtensions;
            _windowBorder = windowBorders;
        }

        public void Initialize(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            Console.CursorVisible = false;
            //Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.Clear();
            PrintUiBorders();
            PrintUiText();
            PrintConnectionText(ip, port, slaveAddress, startAddress, numberOfInputs);
        }

        public void SetLog(IEnumerable<IStateEvent> events)
        {
            ClearLog();

            var numberOfLogsToShow = Console.WindowHeight - 2 - 5;
            var displayEvents = events.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < displayEvents.Count; i++)
            {
                var stateEvent = displayEvents[i];
                Console.SetCursorPosition(12, 5 + i);
                Console.Write($"{stateEvent.Date.ToString("yyyy-dd-MM HH.mm.ss")} {stateEvent.Message}");
            }
        }

        public void SetOffline()
        {
            ClearConnectionStatus();
            PrintConnectionStatus("OFFLINE", ConsoleColor.Gray, ConsoleColor.DarkGray);
        }

        public void SetOnline()
        {
            ClearConnectionStatus();
            PrintConnectionStatus("ONLINE", ConsoleColor.Green, ConsoleColor.DarkGreen);
        }

        public void SetState(bool[] state)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            for (var i = 0; i < state.Length; i++)
            {
                Console.SetCursorPosition(2, i + 5);
                Console.WriteLine($"DI-{i.ToString("00")}: {Convert.ToInt32(state[i])}");
            }

            Console.ResetColor();
        }

        private void PrintUiBorders()
        {
            Console.ResetColor();

            _consoleExtensions.DrawForm(0, 0, Console.WindowWidth, 5, _windowBorder);

            _consoleExtensions.DrawForm(0, 4, 12, Console.WindowHeight - 5, _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                _windowBorder.VerticalAndRightSymbol, _windowBorder.TopRightSymbol, _windowBorder.BottomLeftSymbol, _windowBorder.BottomRightSymbol);

            _consoleExtensions.DrawForm(11, 4, Console.WindowWidth - 11, Console.WindowHeight - 5, _windowBorder.HorizontalSymbol, _windowBorder.VerticalSymbol,
                _windowBorder.HorizontalAndBottomSymbol, _windowBorder.VerticalAndLeftSymbol, _windowBorder.HorizontalAndTopSymbol, _windowBorder.BottomRightSymbol);
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

        private void PrintConnectionText(string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
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
            _consoleExtensions.ClearBox(12, 5, Console.WindowWidth - 13, Console.WindowHeight - 7);
        }

        private void ClearConnectionStatus()
        {
            // TODO: const for online\offline and max
            _consoleExtensions.ClearBox(8, 3, "OFFLINE".Length, 1);
        }

        private void PrintConnectionStatus(string status, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.SetCursorPosition(8, 3);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(status);
            Console.ResetColor();
        }
    }
}
