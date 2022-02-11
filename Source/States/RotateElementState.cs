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
using WireFrame.Controls;
using WireFrame.Misc;

namespace WireFrame.States
{
    class RotateElementState : IFiniteStateMachine
    {
        private class Data
        {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public WFRotationControl rotationControl;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, WFRotationControl rotationControl)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.rotationControl = rotationControl;
            }
        }

        // --

        private Data data = null;

        private bool isTracking = false;

        CoreCursor rotationCursor = new CoreCursor(CoreCursorType.Cross, 1);
        CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        // --

        public RotateElementState(List<object> objects)
        {
            if (objects != null && objects.Count == 4 && (objects[0] is Grid) && (objects[1] is ScrollViewer) && (objects[2] is Canvas) && (objects[3] is WFRotationControl))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as WFRotationControl);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (this.data == null)
            {
                return false;
            }

            PointerPoint pointer = e.GetCurrentPoint(this.data.grid);

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                StartRotation(pointer.Position);
            }
            else if (pointerState == PointerState.Moved && isTracking)
            {
                RotateElement(this.data.scrollViewer, pointer.Position);
            }
            else if (pointerState == PointerState.Released)
            {
                EndRotation();
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

        public void ActiveState(IFiniteStateMachine state)
        {
        }

        private void StartRotation(Point pos)
        {
            Window.Current.CoreWindow.PointerCursor = this.rotationCursor;
            this.isTracking = true;
        }

        private void RotateElement(ScrollViewer scrollViewer, Point pointerPos)
        {
            Point axis = this.data.rotationControl.AxisPoint;

            double startAngle = Math.Atan2(pointerPos.Y - axis.Y, pointerPos.X - axis.X); 
            double endAngle = 2 * Math.PI - 0.0001;

            this.data.rotationControl.Rotate(startAngle, endAngle);
        }

        private void EndRotation()
        {
            this.isTracking = false;
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }
    }
}
