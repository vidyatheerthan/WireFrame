using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WireFrame.Controls;
using WireFrame.Shapes;

namespace WireFrame.Selection
{
    public class HighlightHandler : ISelectionHandler
    {
        private HighlightControl control;
        private Dictionary<IShape, Viewbox> shapes = null;
        private FrameworkElement container = null;

        // --------------------------------------------------------

        public HighlightHandler(HighlightControl control)
        {
            this.control = control;
            this.shapes = new Dictionary<IShape, Viewbox>();
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public void Show(bool show)
        {
            this.control.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool AddShape(IShape shape)
        {
            if (shape == null || this.shapes.ContainsKey(shape)) { return false; }

            var view = this.control.AddNewShape(this.container, shape);

            this.shapes.Add(shape, view);

            return true;
        }

        public bool AddShapes(List<IShape> newShapes)
        {
            bool newAddition = false;

            foreach (var shape in newShapes)
            {
                if (AddShape(shape))
                {
                    newAddition = true;
                }
            }

            foreach (var shape in this.shapes.Keys.ToList())
            {
                if (!newShapes.Contains(shape))
                {
                    this.control.RemoveShape(this.shapes[shape]);
                    this.shapes.Remove(shape);
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            return this.shapes.Keys.ToList();
        }

        public void UpdateShapes(float zoomFactor)
        {
            if (this.shapes == null || this.container == null) { return; }

            foreach (var shape in this.shapes.Keys)
            {
                this.control.UpdateShape(this.container, shape, this.shapes[shape], zoomFactor);
            }
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapes.ContainsKey(shape))
            {
                this.control.RemoveShape(this.shapes[shape]);
                this.shapes.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.control.RemoveAllShapes();
            this.shapes.Clear();
        }

        // ----------------------
    }
}
