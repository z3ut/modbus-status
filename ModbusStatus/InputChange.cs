using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus
{
    public class InputChange : IStateEvent
    {
        public int InputNumber { get; set; }
        public bool Value { get; set; }
        public DateTime Date { get; set; }

        public InputChange()
        {

        }

        public InputChange(int inputNumber, bool value, DateTime date)
        {
            InputNumber = inputNumber;
            Value = value;
            Date = date;
        }
    }
}
