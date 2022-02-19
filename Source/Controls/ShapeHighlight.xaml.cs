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
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class ShapeHighlight : UserControl, ISelector, INotifyPropertyChanged
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        private Dictionary<IShape, Viewbox> shapes = new Dictionary<IShape, Viewbox>();
        private FrameworkElement container = null;
        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public ShapeHighlight()
        {
            this.InitializeComponent();
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool AddShape(IShape shape)
        {
            if(shape == null || this.shapes.ContainsKey(shape)) { return false; }
            var view = CreateNewViewbox(shape);
            _canvas.Children.Add(view);
            this.shapes.Add(shape, view);
            return true;
        }

        public bool AddShapes(List<IShape> newShapes)
        {
            bool newAddition = false;

            foreach(var shape in newShapes)
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
                    this._canvas.Children.Remove(this.shapes[shape]);
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
            if(this.shapes == null || this.container == null) { return; }

            for(int i=0; i < this._canvas.Children.Count; ++i)
            {
                var shape = this.shapes.ElementAt(i).Key;
                
                var viewbox = this._canvas.Children[i] as Viewbox;
                var path = viewbox.Child as Path;
                UpdateViewbox(ref viewbox, shape);
                UpdatePath(ref path, shape, zoomFactor);
            }
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapes.ContainsKey(shape))
            {
                this._canvas.Children.Remove(this.shapes[shape]);
                this.shapes.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this._canvas.Children.Clear();
            this.shapes.Clear();
        }

        private Viewbox CreateNewViewbox(IShape shape)
        {
            Viewbox v = new Viewbox();
            
            UpdateViewbox(ref v, shape);
         
            v.Child = CreateNewPath(shape);
            return v;
        }

        private void UpdateViewbox(ref Viewbox v, IShape shape)
        {
            var transform = shape.GetViewbox().TransformToVisual(this.container);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(v, ePoint.X);
            Canvas.SetTop(v, ePoint.Y);
            v.Stretch = shape.GetViewbox().Stretch;
        }

        private Path CreateNewPath(IShape shape)
        {
            Path p = new Path();
            UpdatePath(ref p, shape, 1.0f);
            p.Data = CloneGeometryGroup(shape);
            p.Fill = this.fillBrush;
            p.Stroke = this.strokeBrush;
            return p;
        }

        private void UpdatePath(ref Path p, IShape shape, float zoomFactor)
        {
            p.Width = shape.GetLength() * zoomFactor;
            p.Height = shape.GetBreath() * zoomFactor;
            p.Stretch = shape.GetPath().Stretch;
        }

        private GeometryGroup CloneGeometryGroup(IShape shape)
        {
            GeometryGroup gg = new GeometryGroup();
            gg.Children = new GeometryCollection();

            var geoGroup = shape.GetPath().Data as GeometryGroup;
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
