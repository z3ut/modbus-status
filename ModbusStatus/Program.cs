using ModbusStatus.StateMonitoring;
using ModbusStatus.StateEvents;
using ModbusStatus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using ModbusStatus.UI.Components;
using ModbusStatus.UI.Shared.WindowBorders;
using ModbusStatus.UI.Shared;
using ModbusStatus.StateMonitoring.DeviceStateReader;

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

        static void Main(string[] args)
        {
            ValidateAndParseUserInput(args);

            var builder = BuildServiceProvider();
            var stateMonitor = builder.GetService<IStateMonitor>();

            stateMonitor.Init(deviceIp, devicePort, slaveAddress, startAddress, numberOfInputs);
            stateMonitor.Start(updatePeriod);
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

        static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<ICurrentState, CurrentState>();
            services.AddTransient<IStateMonitor, StateMonitor>();
            services.AddTransient<IDeviceStateReader, DeviceStateReaderMoq>();

            services.AddTransient<IStateDisplay, StateDisplay>();

            services.AddTransient<ILogComponent, LogComponent>();
            services.AddTransient<IStateComponent, StateComponent>();
            services.AddTransient<IStatusComponent, StatusComponent>();

            services.AddTransient<IWindowBorders, WindowBorderFancy>();

            services.AddTransient<IConsoleExtensions, ConsoleExtensions>();

            return services.BuildServiceProvider();
        }
    }
}
