using ModbusStatus.UI.WindowBorders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI
{
    public class ConsoleExtensions : IConsoleExtensions
    {
        public void ClearBox(int left, int top, int width, int height)
        {
            Console.ResetColor();

            var boxEmptyString = new string(' ', width);

            for (var i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(boxEmptyString);
            }
        }

        public void DrawForm(int left, int top, int width, int height,
            IWindowBorders windowBorders)
        {
            char horizontalChar = windowBorders.HorizontalSymbol;
            char verticalChar = windowBorders.VerticalSymbol;
            char topLeftChar = windowBorders.TopLeftSymbol;
            char topRightChar = windowBorders.TopRightSymbol;
            char bottomLeftChar = windowBorders.BottomLeftSymbol;
            char bottomRightChar = windowBorders.BottomRightSymbol;

            DrawForm(left, top, width, height, horizontalChar, verticalChar,
                topLeftChar, topRightChar, bottomLeftChar, bottomRightChar);
        }

        public void DrawForm(int left, int top, int width, int height,
            char horizontalChar = '-', char verticalChar = '|',
            char topLeftChar = '#', char topRightChar = '#',
            char bottomLeftChar = '#', char bottomRightChar = '#')
        {
            Console.ResetColor();

            var horizontalLine = new string(horizontalChar, width - 2);

            Console.SetCursorPosition(left, top);
            Console.Write(topLeftChar);
            Console.Write(horizontalLine);
            Console.Write(topRightChar);

            Console.SetCursorPosition(left, top + height - 1);
            Console.Write(bottomLeftChar);
            Console.Write(horizontalLine);
            Console.Write(bottomRightChar);

            for (var i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(verticalChar);

                Console.SetCursorPosition(left + width - 1, top + i);
                Console.Write(verticalChar);
            }

        }
    }
}
