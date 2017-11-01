using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Shared.WindowBorders
{
    public class WindowBorderSimple : IWindowBorders
    {
        public char HorizontalSymbol => '-';
        public char VerticalSymbol => '|';

        public char TopLeftSymbol => '#';
        public char TopRightSymbol => '#';
        public char BottomLeftSymbol => '#';
        public char BottomRightSymbol => '#';

        public char HorizontalAndBottomSymbol => '#';
        public char HorizontalAndTopSymbol => '#';
        public char VerticalAndLeftSymbol => '#';
        public char VerticalAndRightSymbol => '#';

        public char HorizontalAndVerticalSymbol => '#';
    }
}
