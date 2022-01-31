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
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Source.States
{
    class HighLightElementState : FiniteStateMachine
    {
        private FrameworkElement activeElement = null;

        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count >= 3 && (objects[0] is ScrollViewer) && (objects[1] is Panel) && (objects[2] is WFTitleBox))
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

            var scrollViewer = objects[0] as ScrollViewer;
            var container = objects[1] as Panel; // _container panel where all elements reside
            var titleBox = objects[2] as WFTitleBox;

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                DrawHighLightBox(scrollViewer, container, titleBox, pointer.Position);
            }
            else if (pointerState == PointerState.Moved)
            {
                HighlightElement(scrollViewer, container, pointer.Position);
            }

            return this;
        }

        private IEnumerable<UIElement> GetElementsUnderPointer(ScrollViewer scrollViewer, Panel container, Point position)
        {
            GeneralTransform transform = container.TransformToVisual(scrollViewer);
            Point transformedPoint = transform.TransformPoint(position);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
            return elements;
        }

        private void DrawHighLightBox(ScrollViewer scrollViewer, Panel container, WFTitleBox titleBox, Point position)
        {
            var elements = GetElementsUnderPointer(scrollViewer, container, position);
            if (elements != null && elements.Count() > 0)
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

        private void HighlightElement(ScrollViewer scrollViewer, Panel container, Point position)
        {
            var elements = GetElementsUnderPointer(scrollViewer, container, position);
            if (elements != null && elements.Count() > 0)
            {
                this.activeElement = elements.First() as FrameworkElement;
                HighlightShape(this.activeElement, true);
            }
            else
            {
                HighlightShape(this.activeElement, false);
            }
        }

        private void HighlightShape(FrameworkElement element, bool highlight)
        {
            if (element != null && element is Shape)
            {
                var shape = element as Shape;
                shape.StrokeThickness = highlight ? 3.0 : 1.0;
                shape.StrokeDashArray = highlight ? new DoubleCollection() { 1.0, 2.0, 0.0 } : null;
            }
        }
    }
}
