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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Source.States
{
    class HighLightElementState : FiniteStateMachine
    {
        private FrameworkElement activeElement = null;

        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null &&
                objects.Count == 6 &&
                (objects[0] is Grid) &&
                (objects[1] is ScrollViewer) &&
                (objects[2] is Canvas) &&
                (objects[3] is Canvas) &&
                (objects[4] is Canvas) &&
                (objects[5] is WFTitleBox))
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
            WFTitleBox titleBox = objects[5] as WFTitleBox;

            PointerPoint pointer = e.GetCurrentPoint(canvas);

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
                DrawHighLightBox(scrollViewer, container, titleBox, pointer.Position);
            }
            else if (pointerState == PointerState.Moved)
            {
                HighlightElement(scrollViewer, container, pointer.Position);
            }

            return false;
        }

        private IEnumerable<UIElement> GetElementsUnderPointer(ScrollViewer scrollViewer, Panel container, Point position)
        {
            GeneralTransform transform = container.TransformToVisual(scrollViewer);
            Point transformedPoint = transform.TransformPoint(position);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
            return elements;
        }

        private void DrawHighLightBox(ScrollViewer scrollViewer, Canvas container, WFTitleBox titleBox, Point position)
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
