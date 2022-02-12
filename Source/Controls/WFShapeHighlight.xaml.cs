using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class WFShapeHighlight : UserControl, IElementSelector, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        // --

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        // --

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        // --

        public static readonly DependencyProperty BreathProperty = DependencyProperty.Register(nameof(Breath), typeof(double), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public double Breath { get => (double)GetValue(BreathProperty); set => SetValue(BreathProperty, value); }

        // --
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public Stretch Stretch { get => (Stretch)GetValue(StretchProperty); set => SetValue(StretchProperty, value); }

        // --

        private IShape selectedShape;
        private FrameworkElement shapeParent;
        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public WFShapeHighlight()
        {
            this.InitializeComponent();
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetSelectedShape(IShape shape, FrameworkElement parent, float zoomFactor)
        {
            this.selectedShape = shape;
            this.shapeParent = parent;

            UpdateSelectedShape(zoomFactor);

            UpdateGeometryGroup(shape);

            Stretch = shape.GetPath().Stretch;
        }

        public IShape GetSelectedShape()
        {
            return this.selectedShape;
        }

        public void UpdateSelectedShape(float zoomFactor)
        {
            var transform = this.selectedShape.GetPath().TransformToVisual(this.shapeParent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Left = ePoint.X;
            Top = ePoint.Y;
            Length = this.selectedShape.GetLength() * zoomFactor;
            Breath = this.selectedShape.GetBreath() * zoomFactor;
        }

        private void UpdateGeometryGroup(IShape shape)
        {
            _geometry_group.Children = new GeometryCollection();

            var geoGroup = shape.GetPath().Data as GeometryGroup;
            foreach (var geo in geoGroup.Children)
            {
                if (geo is EllipseGeometry)
                {
                    _geometry_group.Children.Add(CloneEllipseGeometry(geo as EllipseGeometry));
                }
                else if (geo is RectangleGeometry)
                {
                    _geometry_group.Children.Add(CloneRectangleGeometry(geo as RectangleGeometry));
                }
            }
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
