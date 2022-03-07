using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Selection;

namespace WireFrame.DrawArea.States
{
    class MoveState : IFiniteStateMachine
    {
        private class Data
        {
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas hud;
            public MoveResizeHandler moveHandler;

            public Data(ScrollViewer scrollViewer, Canvas canvas, Canvas hud, MoveResizeHandler resizeHandler)
            {
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.hud = hud;
                this.moveHandler = resizeHandler;
            }
        }

        // --

        private Data data = null;
        private bool isTracking = false;

        // --

        public MoveState(List<object> objects)
        {
            if (objects != null && objects.Count == 4 && (objects[0] is ScrollViewer) && (objects[1] is Canvas) && (objects[2] is Canvas) && (objects[3] is MoveResizeHandler))
            {
                this.data = new Data(objects[0] as ScrollViewer, objects[1] as Canvas, objects[2] as Canvas, objects[3] as MoveResizeHandler);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            PointerPoint hudPointer = e.GetCurrentPoint(data.hud);

            if (pointerState == PointerState.Pressed &&
                    hudPointer.Properties.IsLeftButtonPressed &&
                    !Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(CoreVirtualKeyStates.Down))
            {
                data.moveHandler.StartTrackingPointer(hudPointer.Position);

                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved)
            {
                if (this.isTracking)
                {
                    data.moveHandler.TrackPointer(hudPointer.Position);
                }
            }
            else if (pointerState == PointerState.Released)
            {
                if (this.isTracking)
                {
                    data.moveHandler.StopTrackingPointer(hudPointer.Position, data.scrollViewer.ZoomFactor);
                    this.isTracking = false;
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
    }
}
