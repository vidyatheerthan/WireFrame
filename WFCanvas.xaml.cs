using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class WFCanvas : UserControl, INotifyPropertyChanged
    {
        private const double ZOOM_SPEED = 2.0;
        private const double MIN_ZOOM_FACTOR = 0.90;
        private const double MAX_ZOOM_FACTOR = 10.0;

        private CanvasProfile profile;

        private List<WFElement> elements;

        private double zoom = 1.0;

        public event PropertyChangedEventHandler PropertyChanged;

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
            set
            {
                SetValue(GridWidthDependencyProperty, value);
                OnPropertyChanged();
            }
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
            set
            {
                SetValue(GridHeightDependencyProperty, value);
                OnPropertyChanged();
            }
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
            set
            {
                SetValue(CanvasWidthDependencyProperty, value);
                OnPropertyChanged();
            }
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
            set {
                SetValue(CanvasHeightDependencyProperty, value);
                OnPropertyChanged();
            }
        }

        //---------------------------------


        private double ZoomFactor
        {
            get => MIN_ZOOM_FACTOR + (MAX_ZOOM_FACTOR * (this.zoom / 100.0));
        }

        //--

        public WFCanvas()
        {
            this.elements = new List<WFElement>();

            this.InitializeComponent();

            this.Loaded += OnLoaded;

            _grid.PointerWheelChanged += OnGridPointerWheelChanged;
        }


        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeElements();
        }

        

        public void SetCanvasProfile(CanvasProfile profile)
        {
            this.profile = profile;

            UpdateGridAndCanvasSize();
        }

        private void OnGridPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if(!Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.LeftControl).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down)) { return; }

            double delta = e.GetCurrentPoint(this).Properties.MouseWheelDelta / 120.0;

            this.zoom = Math.Max(1, Math.Min(100.0, this.zoom + (ZOOM_SPEED * delta)));

            //-----

            UpdateGridAndCanvasSize();
        }

       

        private void UpdateGridAndCanvasSize()
        {
            var screenSize = Utility.GetScreenResolution();

            GridWidth = screenSize.Width * ZoomFactor;
            GridHeight = screenSize.Height * ZoomFactor;

            var canvasSize = this.profile.Resize(screenSize);

            CanvasWidth = canvasSize.Width * ZoomFactor;
            CanvasHeight = canvasSize.Height * ZoomFactor;

            UpdateCanvasChildren();
        }






        private void InitializeElements()
        {
            foreach (FrameworkElement element in _canvas.Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                double width = element.Width;
                double height = element.Height;

                var e = new WFElement(left, top, width, height, element);
                this.elements.Add(e);
            }
        }

        private void UpdateCanvasChildren()
        {
            double widthRatio = CanvasWidth / this.profile.Width;
            double heightRatio = CanvasHeight / this.profile.Height;

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
