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
    class SelectionState : IFiniteStateMachine
    {
        private struct Data {
            public Grid grid;
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public Canvas container;
            public Canvas hud;
            public IElementSelector selector;

            public Data(List<object> objects)
            {
                this.grid = objects[0] as Grid;
                this.scrollViewer = objects[1] as ScrollViewer;
                this.canvas = objects[2] as Canvas;
                this.container = objects[3] as Canvas;
                this.hud = objects[4] as Canvas;
                this.selector = objects[5] as IElementSelector;
            }
        }

        private FrameworkElement elementUnderCursor = null;
        private FrameworkElement elementSelected = null;

        public bool ReferenceObjectsAccepted(List<object> objects)
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

            Data data = new Data(objects);

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

        public void HandleZoom(List<object> objects)
        {
            if (!ReferenceObjectsAccepted(objects))
            {
                return;
            }

            Data data = new Data(objects);

            UpdateSelectorBox(data.grid, data.scrollViewer, data.selector);
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

        public void ActiveState(List<object> objects, IFiniteStateMachine state)
        {
            if (state is PanState)
            {
                if (!ReferenceObjectsAccepted(objects))
                {
                    return;
                }

                Data data = new Data(objects);

                UpdateSelectorBox(data.grid, data.scrollViewer, data.selector);
            }
        }
    }
}
