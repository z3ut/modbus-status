using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ModbusStatus.StateMonitoring.DeviceStateReader
{
    public class DeviceStateReader : IDeviceStateReader
    {
        public bool[] ReadValues(string ip, int port, int slaveAddress,
            int startAddress, int numberOfInputs)
        {
            using (var client = new TcpClient(ip, port))
            {
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);
                return master.ReadInputs((byte)slaveAddress,
                    (ushort)startAddress, (ushort)numberOfInputs);
            }
        }
    }
}
