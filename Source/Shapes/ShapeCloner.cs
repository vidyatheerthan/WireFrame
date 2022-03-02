using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Shapes
{
    public static class ShapeCloner
    {
        public static IShape Clone(IShape refShape)
        {
            var shape = new CompoundShape();
            var viewbox = CloneViewbox(refShape.GetViewbox());
            shape.SetViewbox(viewbox);
            shape.SetPath(viewbox.Child as Path);
            return shape;
        }

        public static void Update(IShape refShape, ref IShape cloneShape, Point refPos, float zoomFactor, float posFactor)
        {
            var cloneViewbox = cloneShape.GetViewbox();
            UpdateViewbox(refShape.GetViewbox(), ref cloneViewbox, refPos, zoomFactor, posFactor);
            double scaleX = 0.0, scaleY = 0.0;
            refShape.GetScale(ref scaleX, ref scaleY);
            cloneShape.SetScale(scaleX, scaleY);
        }

        private static Viewbox CloneViewbox(Viewbox refView)
        {
            Viewbox v = new Viewbox();
            v.Child = ClonePath(refView);
            return v;
        }

        private static Path ClonePath(Viewbox refView)
        {
            var refPath = refView.Child as Path;
            Path cloneViewPath = new Path();
            cloneViewPath.Stretch = refPath.Stretch;
            cloneViewPath.Fill = refPath.Fill;
            cloneViewPath.Stroke = refPath.Stroke;
            cloneViewPath.Data = CloneGeometryGroup(refView);
            return cloneViewPath;
        }

        private static void UpdateViewbox(Viewbox refView, ref Viewbox cloneView, Point refViewPos, float zoomFactor, float posFactor)
        {
            Canvas.SetLeft(cloneView, refViewPos.X * posFactor);
            Canvas.SetTop(cloneView, refViewPos.Y * posFactor);
            cloneView.Width = refView.ActualWidth * zoomFactor;
            cloneView.Height = refView.ActualHeight * zoomFactor;
            cloneView.Stretch = refView.Stretch;
        }

        private static GeometryGroup CloneGeometryGroup(Viewbox cloneView)
        {
            GeometryGroup gg = new GeometryGroup();
            gg.Children = new GeometryCollection();

            var geoGroup = (cloneView.Child as Path).Data as GeometryGroup;
            foreach (var geo in geoGroup.Children)
            {
                if (geo is EllipseGeometry)
                {
                    gg.Children.Add(CloneEllipseGeometry(geo as EllipseGeometry));
                }
                else if (geo is RectangleGeometry)
                {
                    gg.Children.Add(CloneRectangleGeometry(geo as RectangleGeometry));
                }
            }

            return gg;
        }

        private static EllipseGeometry CloneEllipseGeometry(EllipseGeometry geo)
        {
            var ellipse = new EllipseGeometry();
            ellipse.Center = geo.Center;
            ellipse.RadiusX = geo.RadiusX;
            ellipse.RadiusY = geo.RadiusY;
            return ellipse;
        }

        private static RectangleGeometry CloneRectangleGeometry(RectangleGeometry geo)
        {
            var rect = new RectangleGeometry();
            rect.Rect = new Rect(geo.Rect.X, geo.Rect.Y, geo.Rect.Width, geo.Rect.Height);
            return rect;
        }
    }
}
