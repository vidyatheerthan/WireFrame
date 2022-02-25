using System;
using System.Collections.Generic;
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
using WireFrame.Misc;
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class HighlightControl : UserControl
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        // -----------------------------------------------------

        public HighlightControl()
        {
            this.InitializeComponent();
        }

        public Viewbox AddNewShape(FrameworkElement container, IShape shape)
        {
            Viewbox v = new Viewbox();

            UpdateViewbox(ref v, shape.GetViewbox(), Utility.GetPointInContainer(shape, container));

            v.Child = CreateNewPath(shape.GetViewbox());

            _canvas.Children.Add(v);

            return v;
        }

        public void RemoveShape(Viewbox view)
        {
            this._canvas.Children.Remove(view);
        }

        public void RemoveAllShapes()
        {
            this._canvas.Children.Clear();
        }

        public void UpdateShape(FrameworkElement container, IShape shape, Viewbox childView, float zoomFactor)
        {
            if(!this._canvas.Children.Contains(childView))
            {
                return;
            }

            var path = childView.Child as Path;
            UpdateViewbox(ref childView, shape.GetViewbox(), Utility.GetPointInContainer(shape, container));
            UpdatePath(ref path, shape.GetViewbox(), zoomFactor);
        }


        private void UpdateViewbox(ref Viewbox childView, Viewbox cloneView, Point position)
        {
            Canvas.SetLeft(childView, position.X);
            Canvas.SetTop(childView, position.Y);
            childView.Stretch = cloneView.Stretch;
        }

        private Path CreateNewPath(Viewbox cloneView)
        {
            Path p = new Path();
            UpdatePath(ref p, cloneView, 1.0f);
            p.Data = CloneGeometryGroup(cloneView);
            p.Fill = this.fillBrush;
            p.Stroke = this.strokeBrush;
            return p;
        }

        private void UpdatePath(ref Path childViewPath, Viewbox cloneView, float zoomFactor)
        {
            childViewPath.Width = cloneView.ActualWidth * zoomFactor;
            childViewPath.Height = cloneView.ActualHeight * zoomFactor;
            childViewPath.Stretch = (cloneView.Child as Path).Stretch;
        }

        private GeometryGroup CloneGeometryGroup(Viewbox cloneView)
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

        private EllipseGeometry CloneEllipseGeometry(EllipseGeometry geo)
        {
            var ellipse = new EllipseGeometry();
            ellipse.Center = geo.Center;
            ellipse.RadiusX = geo.RadiusX;
            ellipse.RadiusY = geo.RadiusY;
            return ellipse;
        }

        private RectangleGeometry CloneRectangleGeometry(RectangleGeometry geo)
        {
            var rect = new RectangleGeometry();
            rect.Rect = new Rect(geo.Rect.X, geo.Rect.Y, geo.Rect.Width, geo.Rect.Height);
            return rect;
        }
    }
}
