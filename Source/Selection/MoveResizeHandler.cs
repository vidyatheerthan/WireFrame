using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WireFrame.Controls;
using WireFrame.Shapes;

namespace WireFrame.Selection
{
    public class MoveResizeHandler : ISelectionHandler
    {
        private MoveResizeControl control = null;
        private Dictionary<IShape, Viewbox> shapes = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeHandler(MoveResizeControl control)
        {
            this.control = control;
            this.shapes = new Dictionary<IShape, Viewbox>();
        }

        public void Show(bool show)
        {
            this.control.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public bool AddShape(IShape shape)
        {
            if (shape == null || this.shapes.ContainsKey(shape))
            {
                return false;
            }

            this.shapes.Add(shape, this.control.AddShapeToHighlight(this.container, shape));

            return true;
        }

        public bool AddShapes(List<IShape> shapes)
        {
            bool newAddition = false;

            foreach (var shape in shapes)
            {
                if (AddShape(shape))
                {
                    newAddition = true;
                }
            }

            foreach (var shape in GetShapes())
            {
                if (!shapes.Contains(shape))
                {
                    this.control.RemoveShapeFromHighlight(this.shapes[shape]);
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
            this.control.ResetBounds();

            var shapes = GetShapes();

            for (int i = 0; i < shapes.Count; ++i)
            {
                this.control.UpdateCorners(this.container, shapes[i], zoomFactor, i == 0);
                this.control.UpdateHighlights(this.container, shapes[i], this.shapes[shapes[i]], zoomFactor);
            }

            this.control.Update();
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapes.ContainsKey(shape))
            {
                this.control.RemoveShapeFromHighlight(this.shapes[shape]);
                this.shapes.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.shapes.Clear();
            this.control.RemoveAllShapesFromHighlight();
            this.control.ResetBounds();
        }

        public void StartResize(Point pointer)
        {
            this.control.StartResize(pointer);            
        }

        public void Resize(Point pointer)
        {
            this.control.Resize(pointer);
        }

        public void StopResize(Point pointer)
        {
            this.control.StopResize(pointer);
        }
    }
}
