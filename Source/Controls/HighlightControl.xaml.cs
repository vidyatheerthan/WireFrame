using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Controls.Gizmo;
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class HighlightControl : UserControl, IContainer
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        // -----------------------------------------------------

        public HighlightControl()
        {
            this.InitializeComponent();
        }

        public IShape AddShape(IShape refShape, Point position)
        {
            var cloneShape = ShapeCloner.Clone(refShape);
            ShapeCloner.Update(refShape, ref cloneShape, position, 1.0f, 1.0f);
            var clonePath = cloneShape.GetPath();
            clonePath.Fill = fillBrush;
            clonePath.Stroke = strokeBrush;
            _canvas.Children.Add(cloneShape.GetViewbox());
            return cloneShape;
        }

        public void RemoveShape(IShape cloneShape)
        {
            _canvas.Children.Remove(cloneShape.GetViewbox());
        }

        public void RemoveShapes()
        {
            _canvas.Children.Clear();
        }

        public void UpdateShape(IShape refShape, IShape cloneShape, Point position, float zoomFactor)
        {
            if (!this._canvas.Children.Contains(cloneShape.GetViewbox()))
            {
                return;
            }

            ShapeCloner.Update(refShape, ref cloneShape, position, zoomFactor, 1.0f);
        }

        public List<Viewbox> GetViewboxes()
        {
            return _canvas.Children.Cast<Viewbox>().ToList();
        }
    }
}
