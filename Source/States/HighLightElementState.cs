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
using Windows.UI.Xaml.Media;

namespace WireFrame.Source.States
{
    class HighLightElementState : FiniteStateMachine
    {
        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count >= 3 && (objects[0] is Panel) && (objects[1] is Panel) && (objects[2] is WFTitleBox))
            {
                return true;
            }

            return false;
        }

        public FiniteStateMachine HandleInput(List<object> objects, PointerState pointerState, PointerPoint pointer)
        {
            if (!ReferenceObjectsAccepted(objects))
            {
                return null;
            }

            var canvas = objects[0] as Panel;
            var container = objects[1] as Panel; // _container panel where all elements reside
            var titleBox = objects[2] as WFTitleBox;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
            }
            else if (pointerState == PointerState.Moved)
            {
                GeneralTransform transform = container.TransformToVisual(canvas);
                Point transformedPoint = transform.TransformPoint(pointer.Position);
                var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
                if(elements != null && elements.Count() > 0)
                {
                    var element = elements.First() as FrameworkElement;
                    titleBox.Visibility = Visibility.Visible;
                    Canvas.SetLeft(titleBox, Canvas.GetLeft(element));
                    Canvas.SetTop(titleBox, Canvas.GetTop(element));
                    titleBox.Width = element.Width;
                    titleBox.Height = element.Height;
                    titleBox.SetTitle(element.GetType().Name);
                }
                else
                {
                    titleBox.Visibility = Visibility.Collapsed;
                }
            }
            else if (pointerState == PointerState.Released)
            {
            }

            return this;
        }
    }
}
