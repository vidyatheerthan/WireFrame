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
            var cloneShape = new CompoundShape();
            cloneShape.GetViewbox().Stretch = refShape.GetViewbox().Stretch;
            CopyPath(refShape.GetPath(), cloneShape.GetPath());

            double scaleX = 0.0, scaleY = 0.0;

            refShape.GetScale(ref scaleX, ref scaleY);
            cloneShape.SetScale(scaleX, scaleY);

            return cloneShape;
        }

        public static void Update(IShape refShape, IShape cloneShape, Point refPos, float zoomFactor)
        {
            cloneShape.SetLeft(refPos.X);
            cloneShape.SetTop(refPos.Y);
            cloneShape.SetLength(refShape.GetLength() * zoomFactor);
            cloneShape.SetBreath(refShape.GetBreath() * zoomFactor);
        }

        private static void CopyPath(Path srcPath, Path dstPath)
        {
            dstPath.Stretch = srcPath.Stretch;
            dstPath.Fill = srcPath.Fill;
            dstPath.Stroke = srcPath.Stroke;
            dstPath.Data = CloneGeometryGroup(srcPath);
        }

        private static GeometryGroup CloneGeometryGroup(Path srcPath)
        {
            GeometryGroup gg = new GeometryGroup();
            gg.Children = new GeometryCollection();

            var geoGroup = srcPath.Data as GeometryGroup;
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
