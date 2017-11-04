using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Shared
{
    public class CursorPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public CursorPosition(int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
