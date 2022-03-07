using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WireFrame.DrawArea.Controls;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.States
{
    class DrawRectangleState : IFiniteStateMachine
    {
        private class Data
        {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas container;
            public Canvas hud;
            public ActionTip actionTip;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, ActionTip actionTip)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.container = container;
                this.hud = hud;
                this.actionTip = actionTip;
            }
        }

        // --

        private Data data = null;
        private RectangleShape activeRectangle = null;
        private bool isTracking = false;

        // --

        public DrawRectangleState(List<object> objects)
        {
            if (objects != null &&
                objects.Count == 6 &&
                (objects[0] is Grid) &&
                (objects[1] is ScrollViewer) &&
                (objects[2] is Canvas) &&
                (objects[3] is Canvas) &&
                (objects[4] is Canvas) &&
                (objects[5] is ActionTip))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as ActionTip);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (this.data == null)
            {
                return false;
            }

            PointerPoint pointer = e.GetCurrentPoint(this.data.grid);

            Point canvasPoint = e.GetCurrentPoint(this.data.canvas).Position;
            Point hudPoint = e.GetCurrentPoint(this.data.hud).Position;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                this.activeRectangle = AddNewPrimitive(this.data.container, canvasPoint.X, canvasPoint.Y, 1, 1);
                ShowActionTip(this.data.actionTip, true, hudPoint.X, hudPoint.Y);
                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved && isTracking)
            {
                ResizePrimitive(this.activeRectangle, canvasPoint.X, canvasPoint.Y);
                UpdateActionTip(this.data.actionTip, hudPoint.X, hudPoint.Y);
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeRectangle = null;
                this.isTracking = false;
                ShowActionTip(this.data.actionTip, false, hudPoint.X, hudPoint.Y);
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

        private void ShowActionTip(ActionTip actionTip, bool show, double left, double top)
        {
            actionTip.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

            Canvas.SetLeft(actionTip, left);
            Canvas.SetTop(actionTip, top);
        }

        private void UpdateActionTip(ActionTip actionTip, double left, double top)
        {
            Canvas.SetLeft(actionTip, left);
            Canvas.SetTop(actionTip, top);

            string tip = "Width: " + ((int)this.activeRectangle.GetLength()).ToString() + "\n" + "Height: " + ((int)this.activeRectangle.GetBreath()).ToString();
            actionTip.SetTip(tip);
        }

        private RectangleShape AddNewPrimitive(Canvas container, double left, double top, double width, double height)
        {
            RectangleShape rect = new RectangleShape();
            rect.SetLeft(left);
            rect.SetTop(top);
            rect.SetLength(width);
            rect.SetBreath(height);

            rect.PathStretch = Stretch.Fill;
            rect.ViewStretch = Stretch.Fill;

            container.Children.Insert(container.Children.Count, rect);
            return rect;
        }

        private void ResizePrimitive(RectangleShape rectangle, double x, double y)
        {
            double width = x - rectangle.GetLeft();
            double height = y - rectangle.GetTop();

            rectangle.SetLength(width > 0 ? width : 1);
            rectangle.SetBreath(height > 0 ? height : 1);
        }
    }
}
