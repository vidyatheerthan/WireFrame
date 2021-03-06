using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Selection;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.States
{
    class SelectionState : IFiniteStateMachine
    {
        private class Data {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas container;
            public Canvas hud;
            public ISelectionHandler highlighter;
            public ISelectionHandler selector;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, ISelectionHandler highlighter, ISelectionHandler selector)
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
        private BoundingBox boundingBox = null;
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
                (objects[5] is ISelectionHandler) &&
                (objects[6] is ISelectionHandler))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as ISelectionHandler, objects[6] as ISelectionHandler);

                this.data.highlighter.SetContainer(data.grid);
                this.data.selector.SetContainer(data.grid);
            }

            this.boundingBox = new BoundingBox(Color.FromArgb(255, 0, 0, 255), Color.FromArgb(100, 0, 0, 255));
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
                
                bool shiftDown = Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftShift).HasFlag(CoreVirtualKeyStates.Down);
                SelectShapeUnderPointer(hudPointer.Position, shiftDown);

                this.isTracking = true;
            }
            else if (pointerState == PointerState.Moved)
            {
                if (this.isTracking)
                {
                    ResizeBoundingBox(hudPointer.Position);
                    SelectShapesUnderBoundingBox();
                }
                else
                {
                    HighlightShapeUnderPointer(hudPointer.Position);
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

            data.selector.UpdateShapes(data.scrollViewer.ZoomFactor);
            data.highlighter.UpdateShapes(data.scrollViewer.ZoomFactor);
        }

        public void HandleScroll()
        {
            if (this.data == null)
            {
                return;
            }

            data.selector.UpdateShapes(data.scrollViewer.ZoomFactor);
            data.highlighter.UpdateShapes(data.scrollViewer.ZoomFactor);
        }

        public void ActiveState(IFiniteStateMachine state)
        {
            if (state is PanState)
            {
                if (this.data == null)
                {
                    return;
                }

                data.selector.UpdateShapes(data.scrollViewer.ZoomFactor);
            }
        }

        private void DrawNewBoundingBox(Canvas hud, Point position)
        {
            hud.Children.Remove(this.boundingBox.GetRectangle());

            this.boundingBox.StartTracking(position);            

            hud.Children.Insert(hud.Children.Count, this.boundingBox.GetRectangle());
            Debug.WriteLine("[DrawNewBoundingBox] Hud Children:" + hud.Children.Count);
        }

        private void ResizeBoundingBox(Point position)
        {
            this.boundingBox.Track(position);
        }

        private void SelectShapesUnderBoundingBox()
        {
            var shapes = GetShapesUnderBounds(this.boundingBox.GetBounds());
            data.selector.AddShapes(shapes);
            data.selector.UpdateShapes(data.scrollViewer.ZoomFactor);
            data.selector.Show(true);
        }

        private void DestroyBoundingBox(Canvas hud)
        {
            hud.Children.Remove(this.boundingBox.GetRectangle());
            this.boundingBox.StopTracking();
            Debug.WriteLine("[DestroyBoundingBox] Hud Children:" + hud.Children.Count);
        }

        private void DoBoundingBoxAction(Point position)
        {
        }

        private List<IShape> GetShapesUnderPointer(Point hudPosition)
        {
            GeneralTransform transform = data.hud.TransformToVisual(null); // always transform to root
            var tp = transform.TransformPoint(hudPosition);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(tp, data.container);
            var shapes  = elements.Where(item => item is IShape).Cast<IShape>().ToList();
            return shapes;
        }

        private List<IShape> GetShapesUnderBounds(Rect hudBounds)
        {
            GeneralTransform transform = data.hud.TransformToVisual(null); // always transform to root
            var tb = transform.TransformBounds(hudBounds);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(tb, data.container);
            var shapes = elements.Where(item => item is IShape).Cast<IShape>().ToList();
            return shapes;
        }

        private void SelectShapeUnderPointer(Point hudPosition, bool alternateSelection)
        {
            var shapes = GetShapesUnderPointer(hudPosition);

            if (!alternateSelection)
            {
                data.selector.RemoveAllShapes();
            }
            
            if (shapes != null && shapes.Count() > 0)
            {
                var shape = shapes.First();

                if (!data.selector.AddShape(shape) && alternateSelection)
                {
                    data.selector.RemoveShape(shape);
                }

                data.selector.UpdateShapes(data.scrollViewer.ZoomFactor);
                data.selector.Show(true);
            }
            else
            {
                data.selector.RemoveAllShapes();
                data.selector.Show(false);
            }
        }

        private void HighlightShapeUnderPointer(Point hudPosition)
        {
            var shapes = GetShapesUnderPointer(hudPosition);
            
            if (shapes != null && shapes.Count() > 0)
            {
                var shape = shapes.First();

                data.highlighter.AddShape(shape);
                data.highlighter.UpdateShapes(data.scrollViewer.ZoomFactor);
                data.highlighter.Show(true);
            }
            else
            {
                data.highlighter.RemoveAllShapes();
                data.highlighter.Show(false);
            }
        }
    }
}
