using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ModbusStatus.StateMonitoring.DeviceStateReader
{
    public class DeviceStateReader : IDeviceStateReader
    {
        private readonly string _ip;
        private readonly int _port;
        private readonly int _slaveAddress;
        private readonly int _startAddress;
        private readonly int _numberOfInputs;

        public DeviceStateReader(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            _ip = ip;
            _port = port;
            _slaveAddress = slaveAddress;
            _startAddress = startAddress;
            _numberOfInputs = numberOfInputs;
        }

        public bool[] ReadValues()
        {
            using (var client = new TcpClient(_ip, _port))
            {
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);
                return master.ReadInputs((byte)_slaveAddress,
                    (ushort)_startAddress, (ushort)_numberOfInputs);
            }
        }
    }
}
