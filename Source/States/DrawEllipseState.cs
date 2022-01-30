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
    class DrawEllipseState : FiniteStateMachine
    {
        private FrameworkElement activeElement = null;
        private bool tracking = false;


        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count > 0 && (objects[0] is Panel))
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

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                this.activeElement = AddNewEllipse(objects[0] as Panel, pointer.Position.X, pointer.Position.Y, 1, 1);
                this.tracking = true;
            }
            else if (pointerState == PointerState.Moved && tracking)
            {
                double width = pointer.Position.X - Canvas.GetLeft(this.activeElement);
                double height = pointer.Position.Y - Canvas.GetTop(this.activeElement);

                this.activeElement.Width = width > 0 ? width : 1;
                this.activeElement.Height = height > 0 ? height : 1;
            }
            else if (pointerState == PointerState.Released)
            {
                this.activeElement = null;
                this.tracking = false;
                return null;
            }

            return this;
        }

        

        private FrameworkElement AddNewEllipse(Panel parent, double left, double top, double width, double height)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Stroke = new SolidColorBrush(Colors.Red);
            ellipse.Fill = new SolidColorBrush(Colors.Orange);

            parent.Children.Insert(0, ellipse);
            return ellipse;
        }
    }
}
