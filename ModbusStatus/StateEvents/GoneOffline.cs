using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.StateEvents
{
    public class GoneOffline : IStateEvent
    {
        public DateTime Date { get; set; }
        public string Message => "GONE OFFLINE";

        public GoneOffline(DateTime date)
        {
            Date = date;
        }
    }
}
