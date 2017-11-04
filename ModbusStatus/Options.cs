using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus
{
    class Options
    {
        [Option("update-period", Default = 500, HelpText = "Update period in ms")]
        public int UpdatePeriod { get; set; }

        [Option("ip", Required = true)]
        public string Ip { get; set; }

        [Option("port", Default = 502)]
        public int Port { get; set; }

        [Option("slave-address", Default = 0)]
        public int SlaveAddress { get; set; }

        [Option("start-address", Default = 0)]
        public int StartAddress { get; set; }

        [Option("number-of-inputs", Default = 16)]
        public int NumberOfInputs { get; set; }
    }
}
