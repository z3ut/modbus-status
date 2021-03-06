﻿using System;
using System.Collections.Generic;
using System.Text;
using ModbusStatus.UI.Shared;

namespace ModbusStatus.UI.Components
{
    public class StatusComponent : IStatusComponent
    {
        private readonly IConsoleExtensions _consoleExtensions;

        private FormPosition _statusTextForm;

        private const int STATUS_COLUMN_WIDTH = 20;
        private const string ONLINE_TEXT = "ONLINE";
        private const string OFFLINE_TEXT = "OFFLINE";

        private int CONNECTION_STATUS_MAX_LENGTH =
            Math.Max(ONLINE_TEXT.Length, OFFLINE_TEXT.Length);

        private readonly ConsoleColor _onlineColor;
        private readonly ConsoleColor _onlineBackgroundColor;

        private readonly ConsoleColor _offlineColor;
        private readonly ConsoleColor _offlineBackgroundColor;

        private CursorPosition _textIpPosition;
        private CursorPosition _textPortPosition;
        private CursorPosition _textSlaveAddressPosition;
        private CursorPosition _textStartAddressPosition;
        private CursorPosition _textNumberOfInputsPosition;
        private CursorPosition _textConnectionStatusPosition;

        public StatusComponent(IConsoleExtensions consoleExtensions,
            ConsoleColor onlineColor,ConsoleColor onlineBackgroundColor,
            ConsoleColor offlineColor,
            ConsoleColor offlineBackgroundColor)
        {
            _consoleExtensions = consoleExtensions;
            _onlineColor = onlineColor;
            _onlineBackgroundColor = onlineBackgroundColor;
            _offlineColor = offlineColor;
            _offlineBackgroundColor = offlineBackgroundColor;
        }

        public void Initialize(FormPosition formPosition, string ip, int port,
            int slaveAddress, int startAddress, int numberOfInputs)
        {
            _statusTextForm = formPosition;

            // first row
            _textIpPosition = new CursorPosition(_statusTextForm.ContentLeft,
                _statusTextForm.ContentTop);
            _textPortPosition = new CursorPosition(_statusTextForm.ContentLeft +
                STATUS_COLUMN_WIDTH, _statusTextForm.ContentTop);

            // second row
            _textSlaveAddressPosition = new CursorPosition(_statusTextForm.ContentLeft,
                _statusTextForm.ContentTop + 1);
            _textStartAddressPosition = new CursorPosition(_statusTextForm.ContentLeft +
                STATUS_COLUMN_WIDTH, _statusTextForm.ContentTop + 1);
            _textNumberOfInputsPosition = new CursorPosition(_statusTextForm.ContentLeft +
                STATUS_COLUMN_WIDTH * 2, _statusTextForm.ContentTop + 1);

            // third row
            _textConnectionStatusPosition = new CursorPosition(_statusTextForm.ContentLeft, _statusTextForm.ContentTop + 2);

            PrintStatusText(ip, port, slaveAddress, startAddress, numberOfInputs);
        }

        public void SetOffline()
        {
            PrintConnectionStatus(OFFLINE_TEXT, _offlineColor, _offlineBackgroundColor);
        }

        public void SetOnline()
        {
            PrintConnectionStatus(ONLINE_TEXT, _onlineColor, _onlineBackgroundColor);
        }

        private void PrintConnectionStatus(string status,
            ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            _consoleExtensions.ClearBox(_textConnectionStatusPosition.Left +
                CONNECTION_STATUS_MAX_LENGTH, _textConnectionStatusPosition.Top,
                CONNECTION_STATUS_MAX_LENGTH, 1);

            Console.SetCursorPosition(_textConnectionStatusPosition.Left +
                CONNECTION_STATUS_MAX_LENGTH, _textConnectionStatusPosition.Top);

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(status);
            Console.ResetColor();
        }

        private void PrintStatusText(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            Console.ResetColor();

            _consoleExtensions.SetCursorPosition(_textIpPosition);
            Console.Write($"IP: {ip}");

            _consoleExtensions.SetCursorPosition(_textPortPosition);
            Console.Write($"PORT: {port}");

            _consoleExtensions.SetCursorPosition(_textSlaveAddressPosition);
            Console.Write($"SLAVE ADDRESS: {slaveAddress}");

            _consoleExtensions.SetCursorPosition(_textStartAddressPosition);
            Console.Write($"START ADDRESS: {startAddress}");

            _consoleExtensions.SetCursorPosition(_textNumberOfInputsPosition);
            Console.Write($"NUMBER OF INPUTS: {numberOfInputs}");

            _consoleExtensions.SetCursorPosition(_textConnectionStatusPosition);
            Console.Write($"STATUS:");
        }
    }
}
