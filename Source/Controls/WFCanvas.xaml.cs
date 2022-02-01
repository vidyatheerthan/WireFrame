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

        private StateExecutor stateExecutor;

        public event PropertyChangedEventHandler PropertyChanged;

        
        //====================================================================================================


        public WFCanvas()
        {
            this.InitializeComponent();

            // --
            StateExecutor.State panState = new StateExecutor.State(new PanState(), new List<object>() { _scrollViewer });
            StateExecutor.State highlightState = new StateExecutor.State(new HighLightElementState(), new List<object>() { _scrollViewer, _container, _titleBox });
            StateExecutor.State drawEllipseState = new StateExecutor.State(new DrawEllipseState(), new List<object>() { _container, _actionTip });
            StateExecutor.State drawRectangleState = new StateExecutor.State(new DrawRectangleState(), new List<object>() { _container, _actionTip });

            var stateGroups = new Dictionary<StateExecutor.StateGroup, List<StateExecutor.State>>();
            stateGroups.Add(StateExecutor.StateGroup.HighLight_Pan, new List<StateExecutor.State>() { highlightState, panState });
            stateGroups.Add(StateExecutor.StateGroup.DrawEllipse, new List<StateExecutor.State>() { drawEllipseState });
            stateGroups.Add(StateExecutor.StateGroup.DrawRectangle, new List<StateExecutor.State>() { drawRectangleState });

            this.stateExecutor = new StateExecutor(stateGroups);
            this.stateExecutor.SelectStateGroup(StateExecutor.StateGroup.DrawEllipse);

            // --
            this.Loaded += OnLoaded;

            // --
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

            //-- 

            Canvas.SetLeft(_frameBackground, frameX);
            Canvas.SetTop(_frameBackground, frameY);

            Canvas.SetLeft(_frameBorder, frameX);
            Canvas.SetTop(_frameBorder, frameY);
        }

        private void OnPointerPressedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            this.stateExecutor.HandleInput(PointerState.Pressed, pointer);
        }

        private void OnPointerMovedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            this.stateExecutor.HandleInput(PointerState.Moved, pointer);
        }

        private void OnPointerReleasedOnCanvas(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(_canvas);

            this.stateExecutor.HandleInput(PointerState.Released, pointer);
        }
    }
}
