using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus
{
    public class GoneOnline : IStateEvent
    {
        public DateTime Date { get; set; }

        public GoneOnline()
        {

        }

        public GoneOnline(DateTime date)
        {
            Date = date;
        }
    }
}
