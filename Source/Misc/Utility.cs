﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace WireFrame
{
    public static class Utility
    {
        public static Size GetScreenResolution()
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            var screenSize = new Size((int)displayInformation.ScreenWidthInRawPixels,
                                      (int)displayInformation.ScreenHeightInRawPixels);

            return screenSize;
        }
    }
}