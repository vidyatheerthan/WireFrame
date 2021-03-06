using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Controls;
using WireFrame.DrawArea.Misc;

namespace WireFrame.DrawArea.States
{
    class FocusState : IFiniteStateMachine
    {
        private class Data
        {
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public MoveResizeControl sizeControl;
            public VirtualKey key;
            
            public Data(ScrollViewer scrollViewer, Canvas canvas, MoveResizeControl sizeControl, VirtualKey key)
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
            if (objects != null && objects.Count == 4 && (objects[0] is ScrollViewer) && (objects[1] is Canvas) && (objects[2] is MoveResizeControl) && (objects[3].GetType().IsEnum))
            {
                this.data = new Data(objects[0] as ScrollViewer, objects[1] as Canvas, objects[2] as MoveResizeControl, (VirtualKey)objects[3]);
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

            if (args.VirtualKey == this.data.key)
            {
                Rect r = this.data.sizeControl.GetRect(this.data.canvas);
                double width = r.Width;
                double height = r.Height;

                float zoomFactor = 0.0f;
                (new CanvasProfile(width, height)).GetCanvas(new Size(this.data.scrollViewer.ActualWidth, this.data.scrollViewer.ActualHeight), out zoomFactor);
                zoomFactor = Math.Max(this.data.scrollViewer.MinZoomFactor, Math.Min(zoomFactor, this.data.scrollViewer.MaxZoomFactor));

                double x = (r.X * zoomFactor) - ((this.data.scrollViewer.ActualWidth - (width * zoomFactor)) * 0.5);
                double y = (r.Y * zoomFactor) - ((this.data.scrollViewer.ActualHeight - (height * zoomFactor)) * 0.5);

                this.data.scrollViewer.ChangeView(x, y, zoomFactor, true);
            }

            return false;
        }

        public void HandleZoom()
        {
        }

        public void HandleScroll()
        {
        }

        public void ActiveState(IFiniteStateMachine state)
        {
        }
    }
}
