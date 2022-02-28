using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Controls
{
    public static class ShapeCloner
    {
        public static Viewbox CloneViewbox(Viewbox refView, SolidColorBrush fillBrush, SolidColorBrush strokeBrush)
        {
            Viewbox v = new Viewbox();
            v.Child = CreateNewPath(refView, fillBrush, strokeBrush);
            return v;
        }

        private static Path CreateNewPath(Viewbox refView, SolidColorBrush fillBrush, SolidColorBrush strokeBrush)
        {
            Path p = new Path();
            p.Fill = fillBrush;
            p.Stroke = strokeBrush;
            UpdatePath(refView, ref p, 1.0f);
            p.Data = CloneGeometryGroup(refView);
            return p;
        }

        public static void UpdateViewbox(Viewbox refView, ref Viewbox childView, Point refViewPos)
        {
            Canvas.SetLeft(childView, refViewPos.X);
            Canvas.SetTop(childView, refViewPos.Y);
            childView.Stretch = refView.Stretch;
        }

        public static void UpdatePath(Viewbox refView, ref Path cloneViewPath, float zoomFactor)
        {
            cloneViewPath.Width = refView.ActualWidth * zoomFactor;
            cloneViewPath.Height = refView.ActualHeight * zoomFactor;
            cloneViewPath.Stretch = (refView.Child as Path).Stretch;
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
