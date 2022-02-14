using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Misc;
using WireFrame.Shapes;

namespace WireFrame.States
{
    class SelectionState : IFiniteStateMachine
    {
        private class Data {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas container;
            public Canvas hud;
            public ISelector highlighter;
            public ISelector selector;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, ISelector highlighter, ISelector selector)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.container = container;
                this.hud = hud;
                this.highlighter = highlighter;
                this.selector = selector;
            }
        }

        // --

        private Data data = null;
        private RectangleShape boundingBox;
        private bool isTracking = false;

        // --

        public SelectionState(List<object> objects)
        {
            if (objects != null &&
                objects.Count == 7 &&
                (objects[0] is Grid) &&
                (objects[1] is ScrollViewer) &&
                (objects[2] is Canvas) &&
                (objects[3] is Canvas) &&
                (objects[4] is Canvas) &&
                (objects[5] is ISelector) &&
                (objects[6] is ISelector))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as ISelector, objects[6] as ISelector);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (this.data == null)
            {
                return false;
            }

            PointerPoint canvasPointer = e.GetCurrentPoint(data.canvas);
            PointerPoint hudPointer = e.GetCurrentPoint(data.hud);

            if (pointerState == PointerState.Pressed &&
                    canvasPointer.Properties.IsLeftButtonPressed &&
                    !Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(CoreVirtualKeyStates.Down))
            {
                DrawNewBoundingBox(data.hud, hudPointer.Position);
                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved)
            {
                if (this.isTracking)
                {
                    ResizeBoundingBox(hudPointer.Position);
                    HighlightShapesUnderBoundingBox(data.container);
                }
                else
                {
                    HighlightShapeUnderPointer(canvasPointer.Position);
                }
            }
            else if (pointerState == PointerState.Released)
            {
                if (this.isTracking)
                {
                    DoBoundingBoxAction(canvasPointer.Position);
                    DestroyBoundingBox(data.hud);
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
            if (this.data == null)
            {
                return;
            }

            data.selector.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
            data.highlighter.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
        }

        public void HandleScroll()
        {
            if (this.data == null)
            {
                return;
            }

            data.selector.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
            data.highlighter.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
        }

        public void ActiveState(IFiniteStateMachine state)
        {
            if (state is PanState)
            {
                if (this.data == null)
                {
                    return;
                }

                data.selector.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
            }
        }

        private void DrawNewBoundingBox(Canvas hud, Point position)
        {
            hud.Children.Remove(this.boundingBox);

            this.boundingBox = new RectangleShape();
            this.boundingBox.SetLeft(position.X);
            this.boundingBox.SetTop(position.Y);
            this.boundingBox.SetLength(1);
            this.boundingBox.SetBreath(1);
            this.boundingBox.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));

            hud.Children.Insert(hud.Children.Count, this.boundingBox);
            Debug.WriteLine("[DrawNewBoundingBox] Hud Children:" + hud.Children.Count);
        }

        private void ResizeBoundingBox(Point position)
        {
            if(this.boundingBox == null) { return; }

            double width = position.X - this.boundingBox.GetLeft();
            double height = position.Y - this.boundingBox.GetTop();

            this.boundingBox.SetLength(width > 0 ? width : 1);
            this.boundingBox.SetBreath(height > 0 ? height : 1);
        }

        private void HighlightShapesUnderBoundingBox(Canvas container)
        {
            Rect bounds = new Rect(this.boundingBox.Left, this.boundingBox.Top, this.boundingBox.Length, this.boundingBox.Breath);
            var shapes = GetShapesUnderBounds(container, bounds);
            foreach(var shape in shapes)
            {
                (shape as IShape).GetPath().Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void DestroyBoundingBox(Canvas hud)
        {
            hud.Children.Remove(this.boundingBox);
            this.boundingBox = null;
            Debug.WriteLine("[DestroyBoundingBox] Hud Children:" + hud.Children.Count);
        }

        private void DoBoundingBoxAction(Point position)
        {
            SelectShapeUnderPointer(position);
        }

        private IEnumerable<UIElement> GetShapesUnderPointer(ScrollViewer scrollViewer, Canvas container, Point position)
        {
            GeneralTransform transform = container.TransformToVisual(scrollViewer);
            Point transformedPoint = transform.TransformPoint(position);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
            elements = elements.Where(item => item is IShape).ToList(); // allow only IShape
            return elements;
        }

        private IEnumerable<UIElement> GetShapesUnderBounds(Canvas container,  Rect bounds)
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(bounds, container);
            elements = elements.Where(item => item is IShape).ToList(); // allow only IShape
            return elements;
        }

        private void SelectShapeUnderPointer(Point position)
        {
            var shapes = GetShapesUnderPointer(data.scrollViewer, data.container, position);

            if (shapes != null && shapes.Count() > 0)
            {
                var shape = shapes.First() as IShape;

                if (data.selector.GetSelectedShape() == shape)
                {
                    data.selector.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
                }
                else
                {
                    data.selector.SetSelectedShape(shape, data.grid, data.scrollViewer.ZoomFactor);
                }

                data.selector.Show(true);
            }
            else
            {
                data.selector.Show(false);
            }
        }

        private void HighlightShapeUnderPointer(Point position)
        {
            var shapes = GetShapesUnderPointer(data.scrollViewer, data.container, position);
            
            if (shapes != null && shapes.Count() > 0)
            {
                var shape = shapes.First() as IShape;
                
                if(data.highlighter.GetSelectedShape() == shape)
                {
                    data.highlighter.UpdateSelectedShape(data.scrollViewer.ZoomFactor);
                }
                else
                {
                    data.highlighter.SetSelectedShape(shape, data.grid, data.scrollViewer.ZoomFactor);
                }

                data.highlighter.Show(true);
            }
            else
            {
                data.highlighter.Show(false);
            }
        }
    }
}
