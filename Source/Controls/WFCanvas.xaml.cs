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
using WireFrame.Source.States;
using Point = Windows.Foundation.Point;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class WFCanvas : UserControl, INotifyPropertyChanged
    {
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

        //====================================================================================================



        private CanvasProfile profile;

        private FiniteStateMachine state = new DrawEllipseState();

        public event PropertyChangedEventHandler PropertyChanged;

        private List<object> drawEllipseStateRefs, panStateRefs;

        
        //====================================================================================================


        public WFCanvas()
        {
            this.InitializeComponent();

            this.drawEllipseStateRefs = new List<object>() { _canvas };
            this.panStateRefs = new List<object>() { _scrollViewer };

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

        private void OnPointerPressedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            List<object> refs = null;

            if (this.state is DrawEllipseState)
            {
                refs = this.drawEllipseStateRefs;
            }
            else if (this.state is PanState)
            {
                refs = this.panStateRefs;
            }

            this.state.HandleInput(refs, PointerState.Pressed, pointer);
        }

        private void OnPointerMovedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            List<object> refs = null;

            if (this.state is DrawEllipseState)
            {
                refs = this.drawEllipseStateRefs;
            }
            else if (this.state is PanState)
            {
                refs = this.panStateRefs;
            }

            this.state.HandleInput(refs, PointerState.Moved, pointer);
        }

        private void OnPointerReleasedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            List<object> refs = null;

            if (this.state is DrawEllipseState)
            {
                refs = this.drawEllipseStateRefs;
            }
            else if (this.state is PanState)
            {
                refs = this.panStateRefs;
            }

            this.state.HandleInput(refs, PointerState.Released, pointer);
        }
    }
}
