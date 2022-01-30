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
using WireFrame.Source.Controls;

namespace WireFrame.Source.States
{
    class DrawEllipseState : FiniteStateMachine
    {
        private FrameworkElement activeElement = null;
        private WFSizeBox sizeBox = null;
        private bool tracking = false;


        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count >= 2 && (objects[0] is Panel) && (objects[1] is Panel))
            {
                return true;
            }

            return false;
        }

        public FiniteStateMachine HandleInput(List<object> objects, PointerState pointerState, PointerPoint pointer)
        {
            if(!ReferenceObjectsAccepted(objects))
            {
                return null;
            }

            var canvas = objects[0] as Panel;
            var hud = objects[1] as Panel;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                AddNewEllipse(canvas, pointer.Position.X, pointer.Position.Y, 1, 1);
                AddNewSizeBox(hud, pointer.Position.X, pointer.Position.Y);
                this.tracking = true;
            }
            else if (pointerState == PointerState.Moved && tracking)
            {
                ResizeEllipse(pointer.Position.X, pointer.Position.Y);
                UpdateSizeBox(pointer.Position.X, pointer.Position.Y);
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeElement = null;
                this.tracking = false;
                RemoveSizeBox(hud);
                return null;
            }

            return this;
        }

        

        private void AddNewEllipse(Panel canvas, double left, double top, double width, double height)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Stroke = new SolidColorBrush(Colors.Orange);
            ellipse.Fill = new SolidColorBrush(Colors.LightGoldenrodYellow);

            canvas.Children.Insert(canvas.Children.Count, ellipse);
            this.activeElement = ellipse;
        }

        private void ResizeEllipse(double x, double y)
        {
            double left = Canvas.GetLeft(this.activeElement);
            double top = Canvas.GetTop(this.activeElement);

            double width = x - left;
            double height = y - top;

            this.activeElement.Width = width > 0 ? width : 1;
            this.activeElement.Height = height > 0 ? height : 1;
        }

        private void AddNewSizeBox(Panel hud, double left, double top)
        {
            this.sizeBox = new WFSizeBox();

            Canvas.SetLeft(this.sizeBox, left);
            Canvas.SetTop(this.sizeBox, top);

            hud.Children.Add(this.sizeBox);
        }

        private void RemoveSizeBox(Panel parent)
        {
            parent.Children.Remove(this.sizeBox);
        }

        private void UpdateSizeBox(double left, double top)
        {
            Canvas.SetLeft(this.sizeBox, left);
            Canvas.SetTop(this.sizeBox, top);

            this.sizeBox.SetSize(this.activeElement.Width, this.activeElement.Height);
        }
    }
}
