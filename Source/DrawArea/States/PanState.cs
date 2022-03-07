using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Misc;

namespace WireFrame.DrawArea.States
{
    class PanState : IFiniteStateMachine
    {
        private class Data
        {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
            }
        }

        // --

        private Data data = null;

        private bool isTracking = false;
        private Point clickedPosition;

        CoreCursor handCursor = new CoreCursor(CoreCursorType.Hand, 1);
        CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        // --

        public PanState(List<object> objects)
        {
            if (objects != null && objects.Count == 3 && (objects[0] is Grid) && (objects[1] is ScrollViewer) && (objects[2] is Canvas))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (this.data == null)
            {
                return false;
            }

            PointerPoint pointer = e.GetCurrentPoint(this.data.canvas);

            if (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(CoreVirtualKeyStates.Down))
            {
                if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
                {
                    StartPanning(pointer.Position);
                }
                else if (pointerState == PointerState.Moved && isTracking)
                {
                    PanScrollViewer(this.data.scrollViewer, pointer.Position);
                }
                else if (pointerState == PointerState.Released)
                {
                    EndPanning();
                }
            }

            return this.isTracking;
        }

        public bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
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

        private void StartPanning(Point pos)
        {
            this.clickedPosition = pos;
            Window.Current.CoreWindow.PointerCursor = this.handCursor;
            this.isTracking = true;
        }

        private void PanScrollViewer(ScrollViewer scrollViewer, Point pointerPos)
        {
            double x = scrollViewer.HorizontalOffset + this.clickedPosition.X - pointerPos.X;
            double y = scrollViewer.VerticalOffset + this.clickedPosition.Y - pointerPos.Y;

            scrollViewer.ChangeView(x, y, null, true);
        }

        private void EndPanning()
        {
            this.isTracking = false;
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }
    }
}
