using ModbusStatus.UI.WindowBorders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI
{
    public interface IConsoleExtensions
    {
        void ClearBox(int left, int top, int width, int height);

        void DrawForm(int left, int top, int width, int height, IWindowBorders windowBorders);
        void DrawForm(int left, int top, int width, int height,
            char horizontalChar = '-', char verticalChar = '|',
            char topLeftChar = '#', char topRightChar = '#',
            char bottomLeftChar = '#', char bottomRightChar = '#');
    }
}
