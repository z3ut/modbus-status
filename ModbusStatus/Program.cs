using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ModbusStatus.Device;
using ModbusStatus.Device.DeviceStateReader;
using ModbusStatus.UI;
using ModbusStatus.UI.Components;
using ModbusStatus.UI.Shared.WindowBorders;
using ModbusStatus.UI.Shared;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;

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
            var serviceCollection = new ServiceCollection();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);


            containerBuilder.RegisterType<DeviceCurrentState>().As<IDeviceCurrentState>();
            containerBuilder.RegisterType<StateMonitor>().As<IStateMonitor>();

#if DEBUG
            containerBuilder.RegisterType<DeviceStateReaderMoq>().As<IDeviceStateReader>();
#else
            containerBuilder.RegisterType<DeviceStateReader>().As<IDeviceStateReader>();
#endif

            containerBuilder.RegisterType<StateDisplay>().As<IStateDisplay>();

            containerBuilder.RegisterType<LogComponent>().As<ILogComponent>()
                .WithParameter("storeMaxEventCount", 100)
                .WithParameter("dateFormat", "yyyy-dd-MM HH.mm.ss");

            containerBuilder.RegisterType<ValuesComponent>().As<IValuesComponent>()
                .WithParameter("valueColor", ConsoleColor.DarkMagenta)
                .WithParameter("valueBackgroundColor", ConsoleColor.DarkYellow);

            containerBuilder.RegisterType<StatusComponent>().As<IStatusComponent>()
                .WithParameter("onlineColor", ConsoleColor.Green)
                .WithParameter("onlineBackgroundColor", ConsoleColor.DarkGreen)
                .WithParameter("offlineColor", ConsoleColor.Gray)
                .WithParameter("offlineBackgroundColor", ConsoleColor.DarkGray);

            containerBuilder.RegisterType<WindowBorderFancy>().As<IWindowBorders>();
            containerBuilder.RegisterType<ConsoleExtensions>().As<IConsoleExtensions>();


            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }
    }
}
