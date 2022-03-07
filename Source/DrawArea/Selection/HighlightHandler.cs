using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using WireFrame.DrawArea.Controls;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.Selection
{
    public class HighlightHandler : ISelectionHandler
    {
        private HighlightControl control;
        private Dictionary<IShape, IShape> shapeClones = null;
        private FrameworkElement container = null;

        // --------------------------------------------------------

        public HighlightHandler(HighlightControl control)
        {
            this.control = control;
            this.shapeClones = new Dictionary<IShape, IShape>();
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public void Show(bool show)
        {
            this.control.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool AddShape(IShape refShape)
        {
            if (refShape == null || this.shapeClones.ContainsKey(refShape)) { return false; }

            var cloneShape = this.control.AddShape(refShape, Utility.GetTopLeft(refShape, container));

            this.shapeClones.Add(refShape, cloneShape);

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

            foreach (var shape in this.shapeClones.Keys.ToList())
            {
                if (!newShapes.Contains(shape))
                {
                    this.control.RemoveShape(this.shapeClones[shape]);
                    this.shapeClones.Remove(shape);
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            return this.shapeClones.Keys.ToList();
        }

        public void UpdateShapes(float zoomFactor)
        {
            if (this.shapeClones == null || this.container == null) { return; }

            foreach (var shape in this.shapeClones.Keys)
            {
                this.control.UpdateShape(shape, this.shapeClones[shape], Utility.GetTopLeft(shape, container), zoomFactor);
            }
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapeClones.ContainsKey(shape))
            {
                this.control.RemoveShape(this.shapeClones[shape]);
                this.shapeClones.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.control.RemoveShapes();
            this.shapeClones.Clear();
        }

        // ----------------------
    }
}
