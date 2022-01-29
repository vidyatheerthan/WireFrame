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
        private CanvasProfile profile;

        private List<WFElement> elements;

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


        public WFCanvas()
        {
            this.elements = new List<WFElement>();

            this.InitializeComponent();

            this.Loaded += OnLoaded;
        }


        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeElements();

            UpdateGridAndCanvasSize();
        }

        

        public void SetCanvasProfile(CanvasProfile profile)
        {
            this.profile = profile;
        }

       

        private void UpdateGridAndCanvasSize()
        {
            var screenSize = Utility.GetScreenResolution();

            GridWidth = screenSize.Width;
            GridHeight = screenSize.Height;

            var canvasSize = this.profile.Resize(screenSize);

            CanvasWidth = canvasSize.Width;
            CanvasHeight = canvasSize.Height;

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
