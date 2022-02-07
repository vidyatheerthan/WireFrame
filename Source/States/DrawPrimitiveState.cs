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

namespace WireFrame.Source.States
{
    abstract class DrawPrimitiveState : FiniteStateMachine
    {
        private FrameworkElement activeElement = null;
        private bool isTracking = false;


        public bool ReferenceObjectsAccepted(List<object> objects)
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
            Canvas container = objects[3] as Canvas;
            Canvas hud = objects[4] as Canvas;
            WFActionTip actionTip = objects[5] as WFActionTip;

            PointerPoint pointer = e.GetCurrentPoint(grid);

            Point canvasPoint = e.GetCurrentPoint(canvas).Position;
            Point hudPoint = e.GetCurrentPoint(hud).Position;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                this.activeElement = AddNewPrimitive(container, canvasPoint.X, canvasPoint.Y, 1, 1);
                ShowActionTip(actionTip, true, hudPoint.X, hudPoint.Y);
                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved && isTracking)
            {
                ResizePrimitive(this.activeElement, canvasPoint.X, canvasPoint.Y);
                UpdateActionTip(actionTip, hudPoint.X, hudPoint.Y);
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeElement = null;
                this.isTracking = false;
                ShowActionTip(actionTip, false, hudPoint.X, hudPoint.Y);
            }

            return this.isTracking;
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

        protected abstract FrameworkElement AddNewPrimitive(Canvas container, double left, double top, double width, double height);

        protected abstract void ResizePrimitive(FrameworkElement element, double x, double y);
    }
}
