using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateEvents
{
    public class GoneOnline : IStateEvent
    {
        public DateTime Date { get; set; }
        public string Message => "GONE ONLINE";

        public GoneOnline(DateTime date)
        {
            Date = date;
        }
    }
}
