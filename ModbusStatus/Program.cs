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
using CommandLine;

namespace ModbusStatus
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(options => RunAndReturnExitCode(options),
                _ => 1);
        }

        static int RunAndReturnExitCode(Options options)
        {
            var builder = BuildServiceProvider();
            var stateMonitor = builder.GetService<IStateMonitor>();

            stateMonitor.Init(options.Ip, options.Port, options.SlaveAddress,
                options.StartAddress, options.NumberOfInputs);
            stateMonitor.StartSync(options.UpdatePeriod);

            return 0;
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
