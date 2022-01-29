using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Point = Windows.Foundation.Point;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class WFCanvas : UserControl, INotifyPropertyChanged
    {
        private CanvasProfile profile;

        private List<WFElement> elements;

        private Action currentAction = Action.CreateNewEllipse;

        private PointerState pointerState = PointerState.Released;

        private MouseButton mouseButtonPressed = MouseButton.Left;

        private FrameworkElement activeElement;

        public event PropertyChangedEventHandler PropertyChanged;

        //---------------------------------

        public static readonly DependencyProperty FrameWidthDependencyProperty = DependencyProperty.Register(
            nameof(FrameWidth),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double FrameWidth
        {
            get => (double)GetValue(FrameWidthDependencyProperty);
            set
            {
                SetValue(FrameWidthDependencyProperty, value);
                OnPropertyChanged();
            }
        }

        //---------------------------------

        public static readonly DependencyProperty FrameHeightDependencyProperty = DependencyProperty.Register(
            nameof(FrameHeight),
            typeof(double),
            typeof(WFCanvas),
            new PropertyMetadata(null)
        );

        public double FrameHeight
        {
            get => (double)GetValue(FrameHeightDependencyProperty);
            set
            {
                SetValue(FrameHeightDependencyProperty, value);
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

            this._canvas.PointerPressed += OnPointerPressedOnCanvas;
            this._canvas.PointerMoved += OnPointerMovedOnCanvas;
            this._canvas.PointerReleased += OnPointerReleasedOnCanvas;
        }


        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateCanvasAndFrameSize();
        }

        

        public void SetCanvasProfile(CanvasProfile profile)
        {
            this.profile = profile;
        }

       

        private void UpdateCanvasAndFrameSize()
        {
            var canvasSize = Utility.GetScreenResolution();

            CanvasWidth = canvasSize.Width;
            CanvasHeight = canvasSize.Height;

            var frameSize = this.profile.ResizeFrame(canvasSize);

            FrameWidth = frameSize.Width;
            FrameHeight = frameSize.Height;

            double frameX = (CanvasWidth - FrameWidth) * 0.5;
            double frameY = (CanvasHeight - FrameHeight) * 0.5;

            Canvas.SetLeft(_frame, frameX);
            Canvas.SetTop(_frame, frameY);
        }



        private FrameworkElement AddNewEllipse(double left, double top, double width, double height)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Stroke = new SolidColorBrush(Colors.Red);
            ellipse.Fill = new SolidColorBrush(Colors.Orange);

            var e = new WFElement(left, top, width, height, ellipse);
            this.elements.Add(e);

            _canvas.Children.Insert(0, ellipse);
            return ellipse;
        }

        private void OnPointerPressedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            if (this.pointerState == PointerState.Dragging)
            {
                return;
            }

            this.pointerState = PointerState.Pressed;

            var pointer = e.GetCurrentPoint(_canvas);

            if(pointer.Properties.IsLeftButtonPressed) { this.mouseButtonPressed = MouseButton.Left; }
            else if (pointer.Properties.IsMiddleButtonPressed) { this.mouseButtonPressed = MouseButton.Middle; }
            else if (pointer.Properties.IsRightButtonPressed) { this.mouseButtonPressed = MouseButton.Right; }

            DoAction(pointer.Position);
        }

        private void OnPointerMovedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            if (this.pointerState == PointerState.Pressed)
            {
                this.pointerState = PointerState.Dragging;
            }
            else if (this.pointerState == PointerState.Released)
            {
                this.pointerState = PointerState.Moving;
            }
               
            DoAction(e.GetCurrentPoint(_canvas).Position);
        }

        private void OnPointerReleasedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            this.pointerState = PointerState.Released;

            DoAction(e.GetCurrentPoint(_canvas).Position);
        }


        private void DoAction(Point pointerPosition)
        {
            if(this.pointerState == PointerState.Pressed)
            {
                if (this.mouseButtonPressed == MouseButton.Left && this.currentAction == Action.CreateNewEllipse)
                {
                    this.activeElement = AddNewEllipse(pointerPosition.X, pointerPosition.Y, 1, 1);
                }
            }
            else if (this.pointerState == PointerState.Dragging)
            {
                if (this.mouseButtonPressed == MouseButton.Left && this.currentAction == Action.CreateNewEllipse)
                {
                    double width = pointerPosition.X - Canvas.GetLeft(this.activeElement);
                    double height = pointerPosition.Y - Canvas.GetTop(this.activeElement);

                    this.activeElement.Width = width > 0 ? width : 1;
                    this.activeElement.Height = height > 0 ? height : 1;
                }
                else if (this.mouseButtonPressed == MouseButton.Middle)
                {
                    double x = _scrollViewer.ActualWidth - pointerPosition.X;
                    double y = _scrollViewer.ActualHeight - pointerPosition.Y;
                    _scrollViewer.ChangeView(x, y, null, true);
                }
            }
        }
    }
}
