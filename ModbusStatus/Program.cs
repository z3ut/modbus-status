using ModbusStatus.StateMonitoring;
using ModbusStatus.StateMonitoring.StateEvents;
using ModbusStatus.UI;
using ModbusStatus.UI.WindowBorders;
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
        static int updatePeriod = 500;
        static string deviceIp;
        static int devicePort = 502;
        static int slaveAddress = 0;
        static int startAddress = 0;
        static int numberOfInputs = 16;
        static bool[] previousState;
        static bool isFirstTimeRequest = true;
        static bool isOnline = false;

        static List<IStateEvent> stateEvents = new List<IStateEvent>();

        static IConsoleExtensions consoleExtensions = new ConsoleExtensions();
        static IWindowBorders windowBorder = new WindowBorderFancy();

        static IStateDisplay stateDisplay = new StateDisplay(consoleExtensions, windowBorder);

        static void Main(string[] args)
        {
            ValidateAndParseUserInput(args);

            var stateMonitor = new StateMonitor(updatePeriod, deviceIp, devicePort, slaveAddress, startAddress, numberOfInputs);
            stateMonitor.Start();
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
                    slaveAddress = int.Parse(args[2]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Slave address must be integer number");
                    PrintUsageFormat();
                    Environment.Exit(0);
                }
            }

            if (args.Length >= 4)
            {
                try
                {
                    startAddress = int.Parse(args[3]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Start address must be integer number");
                    PrintUsageFormat();
                    Environment.Exit(0);
                }
            }

            if (args.Length >= 5)
            {
                try
                {
                    numberOfInputs = int.Parse(args[4]);
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
            Console.WriteLine("Use format: modbus-status IP [PORT] [SLAVE ADDRESS] [START ADDRESS] [NUMBER OF INPUTS]");
            Console.WriteLine("Example:");
            Console.WriteLine("modbus-status 12.34.56.78 22 0 16");
        }
    }
}
