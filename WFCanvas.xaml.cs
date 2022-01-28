using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class WFCanvas : UserControl
    {
        private CanvasProfile profile;

        private List<WFElement> elements;

        private double zoom = 1.0;

        //---------------------------------

        public static readonly DependencyProperty GridWidthDependencyProperty = DependencyProperty.Register(
            nameof(GridWidth),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double GridWidth
        {
            get => (double)GetValue(GridWidthDependencyProperty);
            set => SetValue(GridWidthDependencyProperty, value);
        }

        //---------------------------------

        public static readonly DependencyProperty GridHeightDependencyProperty = DependencyProperty.Register(
            nameof(GridHeight),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double GridHeight
        {
            get => (double)GetValue(GridHeightDependencyProperty);
            set => SetValue(GridHeightDependencyProperty, value);
        }

        //---------------------------------

        public static readonly DependencyProperty CanvasWidthDependencyProperty = DependencyProperty.Register(
            nameof(CanvasWidth),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double CanvasWidth
        {
            get => (double)GetValue(CanvasWidthDependencyProperty);
            set => SetValue(CanvasWidthDependencyProperty, value);
        }

        //---------------------------------

        public static readonly DependencyProperty CanvasHeightDependencyProperty = DependencyProperty.Register(
            nameof(CanvasHeight),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double CanvasHeight
        {
            get => (double)GetValue(CanvasHeightDependencyProperty);
            set => SetValue(CanvasHeightDependencyProperty, value);
        }

        //---------------------------------

        public WFCanvas()
        {
            this.elements = new List<WFElement>();

            this.InitializeComponent();

            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            _grid.PointerWheelChanged += OnPointerWheelChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeElements();
        }

        private void InitializeElements()
        {
            foreach(FrameworkElement element in _canvas.Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                double width = 500.0;//element.Width;
                double height = 500.0;//element.Height;

                var e = new WFElement(left, top, width, height, element);
                this.elements.Add(e);
            }
        }

        public void SetCanvasProfile(CanvasProfile profile)
        {
            this.profile = profile;

            var gridSize = Utility.GetScreenResolution();

            GridWidth = gridSize.Width;
            GridHeight = gridSize.Height;

            var canvasSize = this.profile.Resize(gridSize);

            CanvasWidth = canvasSize.Width;
            CanvasHeight = canvasSize.Height;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double widthRatio = _canvas.ActualWidth / this.profile.Width;
            double heightRatio = _canvas.ActualHeight / this.profile.Height;

            AdjustElements(widthRatio, heightRatio);
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            double delta = e.GetCurrentPoint(this).Properties.MouseWheelDelta / 120.0;

            zoom = Math.Max(1, Math.Min(100, zoom + delta));
        }

        private void AdjustElements(double widthRatio, double heightRatio)
        {
            foreach (WFElement element in this.elements)
            {
                element.Element.Width = element.Width * widthRatio;
                element.Element.Height = element.Height * heightRatio;

                Canvas.SetLeft(element.Element, element.Left * widthRatio);
                Canvas.SetTop(element.Element, element.Top * heightRatio);
            }
        }
    }
}
