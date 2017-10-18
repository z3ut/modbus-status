using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus
{
    public class GoneOffline : IStateEvent
    {
        public DateTime Date { get; set; }

        public GoneOffline()
        {

        }

        public GoneOffline(DateTime date)
        {
            Date = date;
        }
    }
}
