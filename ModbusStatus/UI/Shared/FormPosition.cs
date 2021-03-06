﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusStatus.UI.Shared
{
    public class FormPosition
    {
        public int BorderLeft { get; set; }
        public int BorderTop { get; set; }
        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }

        public int ContentLeft { get; set; }
        public int ContentTop { get; set; }
        public int ContentWidth { get; set; }
        public int ContentHeight { get; set; }

        public FormPosition(int borderLeft, int borderTop,
            int totalWidth, int totalHeight, int borderWidth)
        {
            BorderLeft = borderLeft;
            BorderTop = borderTop;
            TotalWidth = totalWidth;
            TotalHeight = totalHeight;

            ContentLeft = BorderLeft + borderWidth;
            ContentTop = BorderTop + borderWidth;
            ContentWidth = TotalWidth - 2 * borderWidth;
            ContentHeight = TotalHeight - 2 * borderWidth;
        }

        public FormPosition(int borderLeft, int borderTop,
            int totalWidth, int totalHeight,
            int contentLeft, int contentTop,
            int contentWidth, int contentHeight)
        {
            BorderLeft = borderLeft;
            BorderTop = borderTop;
            TotalWidth = totalWidth;
            TotalHeight = totalHeight;

            ContentLeft = contentLeft;
            ContentTop = contentTop;
            ContentWidth = contentWidth;
            ContentHeight = contentHeight;
        }
    }
}
