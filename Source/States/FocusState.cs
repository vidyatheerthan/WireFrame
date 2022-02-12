﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
using WireFrame.Controls;
using WireFrame.Misc;

namespace WireFrame.States
{
    class FocusState : IFiniteStateMachine
    {
        private class Data
        {
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public WFSizeControl sizeControl;
            public VirtualKey key;
            
            public Data(ScrollViewer scrollViewer, Canvas canvas, WFSizeControl sizeControl, VirtualKey key)
            {
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.sizeControl = sizeControl;
                this.key = key;
            }
        }

        // --

        private Data data = null;

        // --

        public FocusState(List<object> objects)
        {
            if (objects != null && objects.Count == 4 && (objects[0] is ScrollViewer) && (objects[1] is Canvas) && (objects[2] is WFSizeControl) && (objects[3].GetType().IsEnum))
            {
                this.data = new Data(objects[0] as ScrollViewer, objects[1] as Canvas, objects[2] as WFSizeControl, (VirtualKey)objects[3]);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            return false;
        }

        public bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
            if (this.data == null)
            {
                return false;
            }

            if (args.VirtualKey == this.data.key && this.data.sizeControl.GetSelectedShape() != null)
            {
                double width = this.data.sizeControl.GetSelectedShape().GetLength();
                double height = this.data.sizeControl.GetSelectedShape().GetBreath();

                float zoomFactor = 0.0f;
                (new CanvasProfile(width, height)).GetCanvas(new Size(this.data.scrollViewer.ActualWidth, this.data.scrollViewer.ActualHeight), out zoomFactor);
                zoomFactor = Math.Max(this.data.scrollViewer.MinZoomFactor, Math.Min(zoomFactor, this.data.scrollViewer.MaxZoomFactor));

                double x = (this.data.sizeControl.GetSelectedShape().GetLeft() * zoomFactor) - ((this.data.scrollViewer.ActualWidth - (width * zoomFactor)) * 0.5);
                double y = (this.data.sizeControl.GetSelectedShape().GetTop() * zoomFactor) - ((this.data.scrollViewer.ActualHeight - (height * zoomFactor)) * 0.5);

                this.data.scrollViewer.ChangeView(x, y, zoomFactor, true);
            }

            return false;
        }

        public void HandleZoom()
        {
        }

        public void ActiveState(IFiniteStateMachine state)
        {
        }
    }
}
