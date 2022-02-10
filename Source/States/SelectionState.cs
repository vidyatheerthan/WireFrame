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
            public IElementSelector selector;

            public Data(Grid grid, ScrollViewer scrollViewer, Canvas canvas, Canvas container, Canvas hud, IElementSelector selector)
            {
                this.grid = grid;
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.container = container;
                this.hud = hud;
                this.selector = selector;
            }
        }

        // --

        private Data data = null;
        private FrameworkElement elementUnderCursor = null;
        private FrameworkElement elementSelected = null;

        // --

        public SelectionState(List<object> objects)
        {
            if (objects != null &&
                objects.Count == 6 &&
                (objects[0] is Grid) &&
                (objects[1] is ScrollViewer) &&
                (objects[2] is Canvas) &&
                (objects[3] is Canvas) &&
                (objects[4] is Canvas) &&
                (objects[5] is IElementSelector))
            {
                this.data = new Data(objects[0] as Grid, objects[1] as ScrollViewer, objects[2] as Canvas, objects[3] as Canvas, objects[4] as Canvas, objects[5] as IElementSelector);
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
                    DrawSelectorBox(data.grid, data.scrollViewer, data.container, data.selector, pointer.Position);
                }
                else if (pointerState == PointerState.Moved)
                {
                    SelectElement(data.scrollViewer, data.container, pointer.Position);
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

            UpdateSelectorBox(data.grid, data.scrollViewer, data.selector);
        }

        public void ActiveState(IFiniteStateMachine state)
        {
            if (state is PanState)
            {
                if (this.data == null)
                {
                    return;
                }

                UpdateSelectorBox(data.grid, data.scrollViewer, data.selector);
            }
        }

        private IEnumerable<UIElement> GetElementsUnderPointer(ScrollViewer scrollViewer, Panel container, Point position)
        {
            GeneralTransform transform = container.TransformToVisual(scrollViewer);
            Point transformedPoint = transform.TransformPoint(position);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(transformedPoint, container);
            return elements;
        }

        private void DrawSelectorBox(Grid grid, ScrollViewer scrollViewer, Canvas container, IElementSelector selector, Point position)
        {
            var elements = GetElementsUnderPointer(scrollViewer, container, position);
            if (elements != null && elements.Count() > 0)
            {
                this.elementSelected = elements.First() as FrameworkElement;
            }
            else
            {
                this.elementSelected = null;
            }

            UpdateSelectorBox(grid, scrollViewer, selector);
        }

        private void UpdateSelectorBox(Grid grid, ScrollViewer scrollViewer, IElementSelector selector) { 
            if(this.elementSelected != null) 
            {
                selector.Show(true);

                selector.SetSelectedElement(this.elementSelected, grid, scrollViewer.ZoomFactor);
                //selector.SetTitle(this.elementSelected.GetType().Name);
            }
            else
            {
                selector.Show(false);
            }
        }

        private void SelectElement(ScrollViewer scrollViewer, Canvas container, Point position)
        {
            var elements = GetElementsUnderPointer(scrollViewer, container, position);
            if (elements != null && elements.Count() > 0)
            {
                this.elementUnderCursor = elements.First() as FrameworkElement;
                SelectShape(this.elementUnderCursor, true);
            }
            else
            {
                SelectShape(this.elementUnderCursor, false);
                this.elementUnderCursor = null;
            }
        }

        private void SelectShape(FrameworkElement element, bool highlight)
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
