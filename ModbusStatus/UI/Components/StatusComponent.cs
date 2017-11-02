using System;
using System.Collections.Generic;
using System.Text;
using ModbusStatus.UI.Shared;

namespace ModbusStatus.UI.Components
{
    public class StatusComponent : IStatusComponent
    {
        private IConsoleExtensions _consoleExtensions;

        private FormPosition _statusTextForm;

        private const string ONLINE_TEXT = "ONLINE";
        private const string OFFLINE_TEXT = "OFFLINE";
        private int CONNECTION_STATUS_MAX_LENGTH = Math.Max(ONLINE_TEXT.Length, OFFLINE_TEXT.Length);

        private const ConsoleColor ONLINE_COLOR = ConsoleColor.Green;
        private const ConsoleColor ONLINE_BACKGROUND_COLOR = ConsoleColor.DarkGreen;

        private const ConsoleColor OFFLINE_COLOR = ConsoleColor.Gray;
        private const ConsoleColor OFFLINE_BACKGROUND_COLOR = ConsoleColor.DarkGray;

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

        public StatusComponent(IConsoleExtensions consoleExtensions)
        {
            _consoleExtensions = consoleExtensions;
        }

        public void Initialize(FormPosition formPosition, string ip, int port, int slaveAddress, int startAddress, int numberOfInputs)
        {
            _statusTextForm = formPosition;

            // first row
            textIpPosition = new CursorPosition(_statusTextForm.ContentLeft, _statusTextForm.ContentTop);
            textPortPosition = new CursorPosition(_statusTextForm.ContentLeft + STATUS_COLUMN_WIDTH, _statusTextForm.ContentTop);

            // second row
            textSlaveAddressPosition = new CursorPosition(_statusTextForm.ContentLeft, _statusTextForm.ContentTop + 1);
            textStartAddressPosition = new CursorPosition(_statusTextForm.ContentLeft + STATUS_COLUMN_WIDTH, _statusTextForm.ContentTop + 1);
            textNumberOfInputsPosition = new CursorPosition(_statusTextForm.ContentLeft + STATUS_COLUMN_WIDTH * 2, _statusTextForm.ContentTop + 1);

            // third row
            textConnectionStatusPosition = new CursorPosition(_statusTextForm.ContentLeft, _statusTextForm.ContentTop + 2);

            PrintUiText();
            PrintConnectionText(ip, port, slaveAddress, startAddress, numberOfInputs);
        }

        public void SetOffline()
        {
            PrintConnectionStatus(OFFLINE_TEXT, OFFLINE_COLOR, OFFLINE_BACKGROUND_COLOR);
        }

        public void SetOnline()
        {
            PrintConnectionStatus(ONLINE_TEXT, ONLINE_COLOR, ONLINE_BACKGROUND_COLOR);
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
    }
}
