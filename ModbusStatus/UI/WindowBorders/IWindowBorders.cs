using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.WindowBorders
{
    public interface IWindowBorders
    {
        char HorizontalSymbol { get; }
        char VerticalSymbol { get; }

        char TopLeftSymbol { get; }
        char TopRightSymbol { get; }
        char BottomLeftSymbol { get; }
        char BottomRightSymbol { get; }

        char HorizontalAndBottomSymbol { get; }
        char HorizontalAndTopSymbol { get; }
        char VerticalAndLeftSymbol { get; }
        char VerticalAndRightSymbol { get; }

        char HorizontalAndVerticalSymbol { get; }
    }
}
