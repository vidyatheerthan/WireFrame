using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public static readonly DependencyProperty PathStretchProperty = DependencyProperty.Register(nameof(PathStretch), typeof(Stretch), typeof(CompoundShape), new PropertyMetadata(null));
        public Stretch PathStretch { get => (Stretch)GetValue(PathStretchProperty); set => SetValue(PathStretchProperty, value); }

        // --

        public static readonly DependencyProperty ViewStretchProperty = DependencyProperty.Register(nameof(ViewStretch), typeof(Stretch), typeof(CompoundShape), new PropertyMetadata(null));
        public Stretch ViewStretch { get => (Stretch)GetValue(ViewStretchProperty); set => SetValue(ViewStretchProperty, value); }

        // --

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }

        // --

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(WFShapeHighlight), new PropertyMetadata(null));
        public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }

        // --

        public static readonly DependencyProperty ColorFillProperty = DependencyProperty.Register(nameof(Fill), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Fill { get => (Brush)GetValue(ColorFillProperty); set => SetValue(ColorFillProperty, value); }

        // --

        private IShape selectedShape = null;
        private FrameworkElement shapeParent = null;
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

            UpdateGeometryGroup(shape);
            UpdateSelectedShape(zoomFactor);
        }

        public IShape GetSelectedShape()
        {
            return this.selectedShape;
        }

        public void UpdateSelectedShape(float zoomFactor)
        {
            if(this.selectedShape == null || this.shapeParent == null) { return; }

            var transform = this.selectedShape.GetViewbox().TransformToVisual(this.shapeParent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Left = ePoint.X;
            Top = ePoint.Y;
            Length = this.selectedShape.GetLength() * zoomFactor;
            Breath = this.selectedShape.GetBreath() * zoomFactor;

            PathStretch = this.selectedShape.GetPath().Stretch;
            ViewStretch = this.selectedShape.GetViewbox().Stretch;
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

            Debug.WriteLine("[WFShapeHighlight] geometry group children:" + _geometry_group.Children.Count);
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
