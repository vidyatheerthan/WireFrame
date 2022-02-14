﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WireFrame.Shapes;

namespace WireFrame
{
    interface ISelector
    {
        void Show(bool show);
        void SetSelectedShape(IShape shape, Canvas container, float zoomFactor);
        IShape GetSelectedShape();
        void UpdateSelectedShape(float zoomFactor);
    }
}
