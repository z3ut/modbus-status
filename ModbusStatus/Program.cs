using ModbusStatus.StateMonitoring;
using ModbusStatus.UI;
using System;
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
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => RunAndReturnExitCode(options));
        }

        static void RunAndReturnExitCode(Options options)
        {
            Console.CancelKeyPress += (sender, e) => Console.Clear();

            var builder = BuildServiceProvider();
            var stateMonitor = builder.GetService<IStateMonitor>();

            stateMonitor.Initialize(options.Ip, options.Port, options.SlaveAddress,
                options.StartAddress, options.NumberOfInputs);
            stateMonitor.StartSync(options.UpdatePeriod);
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
