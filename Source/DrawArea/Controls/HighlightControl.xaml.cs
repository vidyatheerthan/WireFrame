using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WireFrame.DrawArea.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class HighlightControl : UserControl, IContainer
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Colors.Aqua);

        // -----------------------------------------------------

        public HighlightControl()
        {
            this.InitializeComponent();

            fillBrush.Opacity = 0.75;
        }

        public IShape AddShape(IShape refShape, Point position)
        {
            var cloneShape = ShapeCloner.Clone(refShape);
            ShapeCloner.Update(refShape, cloneShape, position, 1.0f);
            cloneShape.SetFill(fillBrush);
            cloneShape.SetStroke(fillBrush);
            cloneShape.SetStrokeThickness(0);
            cloneShape.SetPathStretch(Stretch.Fill);
            cloneShape.SetViewStretch(Stretch.Fill);
            cloneShape.SetFillRule(FillRule.Nonzero);
            _canvas.Children.Add(cloneShape.GetControl());
            return cloneShape;
        }

        public void RemoveShape(IShape cloneShape)
        {
            _canvas.Children.Remove(cloneShape.GetControl());
        }

        public void RemoveShapes()
        {
            _canvas.Children.Clear();
        }

        public void UpdateShape(IShape refShape, IShape cloneShape, Point position, float zoomFactor)
        {
            if (!this._canvas.Children.Contains(cloneShape.GetControl()))
            {
                return;
            }

            ShapeCloner.Update(refShape, cloneShape, position, zoomFactor);
        }

        public List<IShape> GetShapes()
        {
            return _canvas.Children.Where(item => item is IShape).Cast<IShape>().ToList();
        }
    }
}
