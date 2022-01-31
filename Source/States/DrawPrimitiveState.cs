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
            // _container, _sizeBox
            if (objects != null && objects.Count >= 2 && (objects[0] is Panel) && (objects[1] is WFSizeBox))
            {
                return true;
            }

            return false;
        }

        public bool HandleInput(List<object> objects, PointerState pointerState, PointerPoint pointer)
        {
            if (!ReferenceObjectsAccepted(objects))
            {
                return false;
            }

            var canvas = objects[0] as Panel;
            var sizeBox = objects[1] as WFSizeBox;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                this.activeElement = AddNewPrimitive(canvas, pointer.Position.X, pointer.Position.Y, 1, 1);
                ShowSizeBox(sizeBox, true, pointer.Position.X, pointer.Position.Y);
                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved && isTracking)
            {
                ResizePrimitive(this.activeElement, pointer.Position.X, pointer.Position.Y);
                UpdateSizeBox(sizeBox, pointer.Position.X, pointer.Position.Y);
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeElement = null;
                this.isTracking = false;
                ShowSizeBox(sizeBox, false, pointer.Position.X, pointer.Position.Y);
            }

            return this.isTracking;
        }

        private void ShowSizeBox(WFSizeBox sizeBox, bool show, double left, double top)
        {
            sizeBox.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

            Canvas.SetLeft(sizeBox, left);
            Canvas.SetTop(sizeBox, top);
        }

        private void UpdateSizeBox(WFSizeBox sizeBox, double left, double top)
        {
            Canvas.SetLeft(sizeBox, left);
            Canvas.SetTop(sizeBox, top);

            sizeBox.SetSize(this.activeElement.Width, this.activeElement.Height);
        }

        protected abstract FrameworkElement AddNewPrimitive(Panel canvas, double left, double top, double width, double height);

        protected abstract void ResizePrimitive(FrameworkElement element, double x, double y);
    }
}
