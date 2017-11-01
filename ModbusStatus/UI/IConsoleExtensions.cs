using ModbusStatus.UI.WindowBorders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI
{
    public interface IConsoleExtensions
    {
        void ClearBox(int left, int top, int width, int height);

        void DrawForm(int left, int top, int width, int height,
            char horizontalSymbol = '-', char verticalSymbol = '|',
            char topLeftSymbol = '#', char topRightSymbol = '#',
            char bottomLeftSymbol = '#', char bottomRightSymbol = '#');

        void DrawForm(int left, int top, int width, int height,
            char horizontalSymbol = '-', char verticalSymbol = '|',
            char cornerSymbol = '#');

        void DrawForm(int left, int top, int width, int height,
            IWindowBorders windowBorders,
            char? horizontalSymbol = null, char? verticalSymbol = null,
            char? topLeftSymbol = null, char? topRightSymbol = null,
            char? bottomLeftSymbol = null, char? bottomRightSymbol = null);
    }
}
