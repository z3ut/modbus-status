using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.WindowBorders
{
    public class WindowBorderFancy : IWindowBorders
    {
        public char HorizontalSymbol => '═';
        public char VerticalSymbol => '║';

        public char TopLeftSymbol => '╔';
        public char TopRightSymbol => '╗';
        public char BottomLeftSymbol => '╚';
        public char BottomRightSymbol => '╝';

        public char HorizontalAndBottomSymbol => '╦';
        public char HorizontalAndTopSymbol => '╩';
        public char VerticalAndLeftSymbol => '╣';
        public char VerticalAndRightSymbol => '╠';

        public char HorizontalAndVerticalSymbol => '╬';


        //public char HorizontalSymbol => (char) 205;
        //public char VerticalSymbol => (char)186;

        //public char TopLeftSymbol => (char)201;
        //public char TopRightSymbol => (char)187;
        //public char BottomLeftSymbol => (char)200;
        //public char BottomRightSymbol => (char)188;

        //public char HorizontalAndBottomSymbol => (char)203;
        //public char HorizontalAndTopSymbol => (char)202;
        //public char VerticalAndLeftSymbol => (char)185;
        //public char VerticalAndRightSymbol => (char)204;

        //public char HorizontalAndVerticalSymbol => (char)206;
    }
}
