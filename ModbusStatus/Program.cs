using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ModbusStatus.Device;
using ModbusStatus.Device.DeviceStateReader;
using ModbusStatus.UI;
using ModbusStatus.UI.Components;
using ModbusStatus.UI.Shared.WindowBorders;
using ModbusStatus.UI.Shared;
using System;

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

            services.AddTransient<IDeviceCurrentState, DeviceCurrentState>();
            services.AddTransient<IStateMonitor, StateMonitor>();
#if DEBUG
            services.AddTransient<IDeviceStateReader, DeviceStateReaderMoq>();
#else
            services.AddTransient<IDeviceStateReader, DeviceStateReader>();
#endif
            services.AddTransient<IStateDisplay, StateDisplay>();

            services.AddTransient<ILogComponent, LogComponent>();
            services.AddTransient<IValuesComponent, ValuesComponent>();
            services.AddTransient<IStatusComponent, StatusComponent>();

            services.AddTransient<IWindowBorders, WindowBorderFancy>();
            services.AddTransient<IConsoleExtensions, ConsoleExtensions>();

            return services.BuildServiceProvider();
        }
    }
}
