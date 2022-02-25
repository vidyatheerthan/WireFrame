using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Shapes;

namespace WireFrame.Controls
{
    public static class ViewboxCloner
    {
        public static Viewbox CreateNewViewbox(IShape shape, SolidColorBrush fillBrush, SolidColorBrush strokeBrush)
        {
            Viewbox v = new Viewbox();
            v.Child = CreateNewPath(shape.GetViewbox(), fillBrush, strokeBrush);
            return v;
        }

        private static Path CreateNewPath(Viewbox cloneView, SolidColorBrush fillBrush, SolidColorBrush strokeBrush)
        {
            Path p = new Path();
            p.Fill = fillBrush;
            p.Stroke = strokeBrush;
            UpdatePath(ref p, cloneView, 1.0f);
            p.Data = CloneGeometryGroup(cloneView);
            return p;
        }

        public static void UpdateViewbox(ref Viewbox childView, Viewbox cloneView, Point position)
        {
            Canvas.SetLeft(childView, position.X);
            Canvas.SetTop(childView, position.Y);
            childView.Stretch = cloneView.Stretch;
        }

        public static void UpdatePath(ref Path childViewPath, Viewbox cloneView, float zoomFactor)
        {
            childViewPath.Width = cloneView.ActualWidth * zoomFactor;
            childViewPath.Height = cloneView.ActualHeight * zoomFactor;
            childViewPath.Stretch = (cloneView.Child as Path).Stretch;
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
