using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateMonitoring.StateEvents
{
    public class InputChange : IStateEvent
    {
        public DateTime Date { get; private set; }
        public string Message => $"DI-{InputNumber.ToString("00")} -> {Convert.ToInt32(Value)}";

        private int InputNumber { get; set; }
        private bool Value { get; set; }

        public InputChange(int inputNumber, bool value, DateTime date)
        {
            InputNumber = inputNumber;
            Value = value;
            Date = date;
        }
    }
}
