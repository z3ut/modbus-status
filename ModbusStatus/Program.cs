using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace ModbusStatus
{
    class Program
    {
        static string deviceIp;
        static int devicePort = 22;
        static int startAddress = 0;
        static int numberOfInputs = 16;
        static bool[] previousState;
        static bool isOnline = false;

        static List<IStateEvent> stateEvents = new List<IStateEvent>();

        static void Main(string[] args)
        {
            ValidateAndParseUserInput(args);

            previousState = new bool[numberOfInputs];

            Console.CursorVisible = false;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            PrintUiBorders();
            PrintUiText();

            PrintConnectionText();

            for (; ; )
            {
                try
                {
                    var currentState = GetValues(deviceIp, devicePort, startAddress, numberOfInputs);

                    Console.ResetColor();


                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                    for (var i = 0; i < currentState.Length; i++)
                    {
                        Console.SetCursorPosition(1, i + 5);
                        Console.WriteLine($"DI-{i.ToString("00")}: {Convert.ToInt32(currentState[i])}");
                    }

                    Console.ResetColor();

                    var shouldRedrawLog = false;

                    for (var i = 0; i < currentState.Length; i++)
                    {
                        if (previousState[i] != currentState[i])
                        {
                            stateEvents.Add(new InputChange(i, currentState[i], DateTime.Now));
                            shouldRedrawLog = true;
                        }
                    }

                    previousState = currentState;

                    if (shouldRedrawLog)
                    {
                        RedrawLog();
                    }

                    if (!isOnline)
                    {
                        SetOnline();
                    }
                }
                catch (Exception)
                {
                    if (isOnline)
                    {
                        SetOffline();

                        RedrawLog();
                    }
                }
                

                Thread.Sleep(500);
            }
        }

        static void RedrawLog()
        {
            ClearLog();

            var numberOfLogsToShow = Console.WindowHeight - 2 - 5;
            stateEvents = stateEvents.TakeLast(numberOfLogsToShow).ToList();

            for (var i = 0; i < stateEvents.Count; i++)
            {
                var stateEvent = stateEvents[i];

                if (stateEvent is InputChange)
                {
                    var inputChangeEvent = stateEvent as InputChange;
                    Console.SetCursorPosition(11, 5 + i);
                    Console.Write($"{inputChangeEvent.Date.ToString("HH.mm.ss")} DI-{inputChangeEvent.InputNumber.ToString("00")} -> {Convert.ToInt32(inputChangeEvent.Value)}");
                    continue;
                }

                if (stateEvent is GoneOffline)
                {
                    var inputChangeEvent = stateEvent as GoneOffline;
                    Console.SetCursorPosition(11, 5 + i);
                    Console.Write($"{inputChangeEvent.Date.ToString("HH.mm.ss")} GONE OFFLINE");
                    continue;
                }

                if (stateEvent is GoneOnline)
                {
                    var inputChangeEvent = stateEvent as GoneOnline;
                    Console.SetCursorPosition(11, 5 + i);
                    Console.Write($"{inputChangeEvent.Date.ToString("HH.mm.ss")} GONE ONLINE");
                    continue;
                }
            }
        }

        static void ClearLog()
        {
            for (var i = 5; i < Console.WindowHeight - 2; i++)
            {
                Console.ResetColor();
                Console.SetCursorPosition(11, i);
                var emptyLogString = new string(' ', Console.WindowWidth - 12);
                Console.Write(emptyLogString);
            }
        }

        static void ValidateAndParseUserInput(string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsageFormat();
                Environment.Exit(0);
            }

            deviceIp = args[0];

            if (args.Length >= 2)
            {
                try
                {
                    devicePort = int.Parse(args[1]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Port must be integer number");
                    PrintUsageFormat();
                    Environment.Exit(0);
                }
            }

            if (args.Length >= 3)
            {
                try
                {
                    startAddress = int.Parse(args[2]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Start address must be integer number");
                    PrintUsageFormat();
                    Environment.Exit(0);
                }
            }

            if (args.Length >= 3)
            {
                try
                {
                    numberOfInputs = int.Parse(args[3]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Number of inputs must be integer number");
                    PrintUsageFormat();
                    Environment.Exit(0);
                }
            }
        }

        static void PrintUsageFormat()
        {
            Console.WriteLine("Use format: modbus-status IP [PORT] [START ADDRESS] [NUMBER OF INPUTS]");
            Console.WriteLine("Example:");
            Console.WriteLine("modbus-status 12.34.56.78 22 0 16");
        }

        static void PrintUiBorders()
        {
            Console.ResetColor();

            Console.SetCursorPosition(0, 0);
            Console.Write(new string('-', Console.WindowWidth));

            Console.SetCursorPosition(0, 4);
            Console.Write(new string('-', Console.WindowWidth));

            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write(new string('-', Console.WindowWidth));

            for (var i = 0; i < Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('|');
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write('|');
            }

            for (var i = 5; i < Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(10, i);
                Console.Write('|');
            }
        }

        static void PrintUiText()
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

        static void PrintConnectionText()
        {
            Console.ResetColor();

            Console.SetCursorPosition(4, 1);
            Console.Write(deviceIp);

            Console.SetCursorPosition(25, 1);
            Console.Write(devicePort);

            Console.SetCursorPosition(15, 2);
            Console.Write(startAddress);

            Console.SetCursorPosition(37, 2);
            Console.Write(numberOfInputs);
        }

        static bool[] GetValues(string ip, int port, int startAddress, int numberOfInputs)
        {
            //using (var client = new TcpClient(ip, port))
            //{
            //    var factory = new ModbusFactory();
            //    var master = factory.CreateMaster(client);
            //    return master.ReadInputs(0, (ushort)startAddress, (ushort)numberOfInputs);
            //}

            var gen = new Random();
            if (gen.Next(100) > 90)
            {
                throw new Exception();
            }

            return Enumerable.Range(0, numberOfInputs)
                .Select(s => gen.Next(100) > 50)
                .ToArray(); ;
        }

        static void SetOnline()
        {
            isOnline = true;
            stateEvents.Add(new GoneOnline(DateTime.Now));
            ClearConnectionStatus();
            PrintConnectionStatus("ONLINE", ConsoleColor.Green, ConsoleColor.DarkGreen);
        }

        static void SetOffline()
        {
            isOnline = false;
            stateEvents.Add(new GoneOffline(DateTime.Now));
            ClearConnectionStatus();
            PrintConnectionStatus("OFFLINE", ConsoleColor.Gray, ConsoleColor.DarkGray);
        }

        static void ClearConnectionStatus()
        {
            Console.SetCursorPosition(8, 3);
            Console.ResetColor();
            Console.WriteLine(new string(' ', "OFFLINE".Length));
        }

        static void PrintConnectionStatus(string status, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.SetCursorPosition(8, 3);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(status);
            Console.ResetColor();
        }
    }
}
