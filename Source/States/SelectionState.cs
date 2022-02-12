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
            public IElementSelector highlighter;
            public IElementSelector selector;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, IElementSelector highlighter, IElementSelector selector)
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
        private IShape shapeUnderCursor = null;
        private IShape shapeSelected = null;

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
                (objects[5] is IElementSelector) &&
                (objects[6] is IElementSelector))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as IElementSelector, objects[6] as IElementSelector);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            if (this.data == null)
            {
                return false;
            }

            PointerPoint pointer = e.GetCurrentPoint(data.canvas);

            if (!Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(CoreVirtualKeyStates.Down))
            {
                if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
                {
                    DrawSelector(data.grid, data.scrollViewer, data.container, data.selector, pointer.Position);
                }
                else if (pointerState == PointerState.Moved)
                {
                    HighlightShape(data.scrollViewer, data.container, pointer.Position);
                }
            }
            return false;
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

            UpdateSelector(data.grid, data.scrollViewer, data.selector);
        }

        public void ActiveState(IFiniteStateMachine state)
        {
            if (state is PanState)
            {
                if (this.data == null)
                {
                    return;
                }

                UpdateSelector(data.grid, data.scrollViewer, data.selector);
            }
        }

        private IEnumerable<UIElement> GetShapesUnderPointer(ScrollViewer scrollViewer, Panel container, Point position)
        {
            GeneralTransform transform = container.TransformToVisual(scrollViewer);
            Point transformedPoint = transform.TransformPoint(position);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
            elements = elements.Where(item => item is IShape).ToList(); // allow only IShape
            return elements;
        }

        private void DrawSelector(Grid grid, ScrollViewer scrollViewer, Canvas container, IElementSelector selector, Point position)
        {
            var shapes = GetShapesUnderPointer(scrollViewer, container, position);
            if (shapes != null && shapes.Count() > 0)
            {
                this.shapeSelected = shapes.First() as IShape;
            }
            else
            {
                this.shapeSelected = null;
            }

            UpdateSelector(grid, scrollViewer, selector);
        }

        private void UpdateSelector(Grid grid, ScrollViewer scrollViewer, IElementSelector selector) { 
            if(this.shapeSelected != null) 
            {
                selector.Show(true);

                selector.SetSelectedShape(this.shapeSelected, grid, scrollViewer.ZoomFactor);
                //selector.SetTitle(this.elementSelected.GetType().Name);
            }
            else
            {
                selector.Show(false);
            }
        }

        private void HighlightShape(ScrollViewer scrollViewer, Canvas container, Point position)
        {
            var shapes = GetShapesUnderPointer(scrollViewer, container, position);
            if (shapes != null && shapes.Count() > 0)
            {
                this.shapeUnderCursor = shapes.First() as IShape;
                HighlightShape(this.shapeUnderCursor, true);
            }
            else
            {
                HighlightShape(this.shapeUnderCursor, false);
                this.shapeUnderCursor = null;
            }
        }

        private void HighlightShape(IShape shape, bool highlight)
        {
            if (shape != null && shape is IShape)
            {
                // highlight this shape
            }
        }
    }
}
