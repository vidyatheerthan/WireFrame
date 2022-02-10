using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;
using WireFrame.Controls;
using WireFrame.Misc;

namespace WireFrame.States
{
    class DrawEllipseState : IFiniteStateMachine
    {
        private class Data
        {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas container;
            public Canvas hud;
            public WFActionTip actionTip;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, WFActionTip actionTip)
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
        private FrameworkElement activeElement = null;
        private bool isTracking = false;

        // --

        public DrawEllipseState(List<object> objects)
        {
            if (objects != null &&
                objects.Count == 6 &&
                (objects[0] is Grid) &&
                (objects[1] is ScrollViewer) &&
                (objects[2] is Canvas) &&
                (objects[3] is Canvas) &&
                (objects[4] is Canvas) &&
                (objects[5] is WFActionTip))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as WFActionTip);
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
                this.activeElement = AddNewPrimitive(this.data.container, canvasPoint.X, canvasPoint.Y, 1, 1);
                ShowActionTip(this.data.actionTip, true, hudPoint.X, hudPoint.Y);
                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved && isTracking)
            {
                ResizePrimitive(this.activeElement, canvasPoint.X, canvasPoint.Y);
                UpdateActionTip(this.data.actionTip, hudPoint.X, hudPoint.Y);
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeElement = null;
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

        public void ActiveState(IFiniteStateMachine state)
        {
        }

        private void ShowActionTip(WFActionTip actionTip, bool show, double left, double top)
        {
            actionTip.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

            Canvas.SetLeft(actionTip, left);
            Canvas.SetTop(actionTip, top);
        }

        private void UpdateActionTip(WFActionTip actionTip, double left, double top)
        {
            Canvas.SetLeft(actionTip, left);
            Canvas.SetTop(actionTip, top);

            string tip = "Width: " + ((int)this.activeElement.Width).ToString() + "\n" + "Height: " + ((int)this.activeElement.Height).ToString();
            actionTip.SetTip(tip);
        }

        private FrameworkElement AddNewPrimitive(Canvas container, double left, double top, double width, double height)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Stroke = new SolidColorBrush(Colors.Orange);
            ellipse.Fill = new SolidColorBrush(Colors.LightGoldenrodYellow);

            container.Children.Insert(container.Children.Count, ellipse);
            return ellipse;
        }

        private void ResizePrimitive(FrameworkElement element, double x, double y)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);

            double width = x - left;
            double height = y - top;

            element.Width = width > 0 ? width : 1;
            element.Height = height > 0 ? height : 1;
        }
    }
}
