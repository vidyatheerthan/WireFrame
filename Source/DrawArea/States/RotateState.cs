using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Controls;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Selection;

namespace WireFrame.DrawArea.States
{
    class RotateState : IFiniteStateMachine
    {
        private class Data
        {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public MoveResizeRotateHandler rotationHandler;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, MoveResizeRotateHandler rotationHandler)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.rotationHandler = rotationHandler;
            }
        }

        // --

        private Data data = null;

        private bool isTracking = false;

        CoreCursor rotationCursor = new CoreCursor(CoreCursorType.Cross, 1);
        CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        // --

        public RotateState(List<object> objects)
        {
            if (objects != null && objects.Count == 4 && (objects[0] is Grid) && (objects[1] is ScrollViewer) && (objects[2] is Canvas) && (objects[3] is MoveResizeRotateHandler))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as MoveResizeRotateHandler);
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
                RotateElement(pointer.Position);
            }
            else if (pointerState == PointerState.Released)
            {
                EndRotation(pointer.Position);
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

        private void StartRotation(Point pos)
        {
            data.rotationHandler.StartTrackingPointer(pos);
            this.isTracking = true;
        }

        private void RotateElement(Point pointerPos)
        {
            data.rotationHandler.TrackPointer(pointerPos);
        }

        private void EndRotation(Point pointerPos)
        {
            this.isTracking = false;
            data.rotationHandler.StopTrackingPointer(pointerPos, data.scrollViewer.ZoomFactor);
        }
    }
}
