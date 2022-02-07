using System;
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

namespace WireFrame.Source.States
{
    class PanState : FiniteStateMachine
    {
        private bool isTracking = false;

        private Point clickedPosition;

        CoreCursor handCursor = new CoreCursor(CoreCursorType.Hand, 1);
        CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);


        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count == 3 && (objects[0] is Grid) && (objects[1] is ScrollViewer) && (objects[2] is Canvas))
            {
                return true;
            }

            return false;
        }

        public bool HandleInput(List<object> objects, PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (!ReferenceObjectsAccepted(objects))
            {
                return false;
            }

            Grid grid = objects[0] as Grid;
            ScrollViewer scrollViewer = objects[1] as ScrollViewer;
            Canvas canvas = objects[2] as Canvas;

            PointerPoint pointer = e.GetCurrentPoint(canvas);

            if (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(CoreVirtualKeyStates.Down))
            {
                if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
                {
                    StartPanning(pointer.Position);
                }
                else if (pointerState == PointerState.Moved && isTracking)
                {
                    PanScrollViewer(scrollViewer, pointer.Position);
                }
                else if (pointerState == PointerState.Released)
                {
                    EndPanning();
                }
            }
            else
            {
                EndPanning();
            }

            return this.isTracking;
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
