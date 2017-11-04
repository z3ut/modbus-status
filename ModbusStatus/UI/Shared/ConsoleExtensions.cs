using ModbusStatus.UI.Shared.WindowBorders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Shared
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
            char horizontalSymbol = '-', char verticalSymbol = '|',
            char topLeftSymbol = '#', char topRightSymbol = '#',
            char bottomLeftSymbol = '#', char bottomRightSymbol = '#')
        {
            Console.ResetColor();

            var horizontalLine = new string(horizontalSymbol, width - 2);

            Console.SetCursorPosition(left, top);
            Console.Write(topLeftSymbol);
            Console.Write(horizontalLine);
            Console.Write(topRightSymbol);

            Console.SetCursorPosition(left, top + height - 1);
            Console.Write(bottomLeftSymbol);
            Console.Write(horizontalLine);
            Console.Write(bottomRightSymbol);

            for (var i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(verticalSymbol);

                Console.SetCursorPosition(left + width - 1, top + i);
                Console.Write(verticalSymbol);
            }
        }

        public void DrawForm(int left, int top, int width, int height,
            char horizontalSymbol = '-', char verticalSymbol = '|', char cornerSymbol = '#')
        {
            DrawForm(left, top, width, height, horizontalSymbol, verticalSymbol,
                cornerSymbol, cornerSymbol, cornerSymbol, cornerSymbol);
        }

        public void DrawForm(int left, int top, int width, int height,
            IWindowBorders windowBorders,
            char? horizontalSymbol = null, char? verticalSymbol = null,
            char? topLeftSymbol = null, char? topRightSymbol = null,
            char? bottomLeftSymbol = null, char? bottomRightSymbol = null)
        {
            char horizontalSymbolValue = horizontalSymbol ?? windowBorders.HorizontalSymbol;
            char verticalSymbolValue = verticalSymbol ?? windowBorders.VerticalSymbol;
            char topLeftSymbolValue = topLeftSymbol ?? windowBorders.TopLeftSymbol;
            char topRightValue = topRightSymbol ?? windowBorders.TopRightSymbol;
            char bottomLeftValue = bottomLeftSymbol ?? windowBorders.BottomLeftSymbol;
            char bottomRightValue = bottomRightSymbol ?? windowBorders.BottomRightSymbol;

            DrawForm(left, top, width, height, horizontalSymbolValue, verticalSymbolValue,
                topLeftSymbolValue, topRightValue, bottomLeftValue, bottomRightValue);
        }
    }
}
