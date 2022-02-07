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
using Size = Windows.Foundation.Size;

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
            StateExecutor.State panState = new StateExecutor.State(new PanState(), new List<object>() { _grid, _scrollViewer, _canvas });
            StateExecutor.State highlightState = new StateExecutor.State(new HighLightElementState(), new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _titleBox });
            StateExecutor.State drawEllipseState = new StateExecutor.State(new DrawEllipseState(), new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _actionTip });
            StateExecutor.State drawRectangleState = new StateExecutor.State(new DrawRectangleState(), new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _actionTip });

            var stateGroups = new Dictionary<StateExecutor.StateGroup, List<StateExecutor.State>>();
            stateGroups.Add(StateExecutor.StateGroup.HighLight_Pan, new List<StateExecutor.State>() { highlightState, panState });
            stateGroups.Add(StateExecutor.StateGroup.DrawEllipse, new List<StateExecutor.State>() { drawEllipseState });
            stateGroups.Add(StateExecutor.StateGroup.DrawRectangle, new List<StateExecutor.State>() { drawRectangleState });

            this.stateExecutor = new StateExecutor(stateGroups);
            this.stateExecutor.SelectStateGroup(StateExecutor.StateGroup.HighLight_Pan);

            // --
            this.Loaded += OnLoaded;

            // --
            this._grid.PointerPressed += OnPointerPressedOnGrid;
            this._grid.PointerMoved += OnPointerMovedOnGrid;
            this._grid.PointerReleased += OnPointerReleasedOnGrid;

            // --
            this._scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, OnScrollViewerZoomFactorChanged);
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
            var screenSize = Utility.GetScreenResolution();
            var canvasSize = this.profile.GetCanvas(screenSize);

            CanvasWidth = canvasSize.Width;
            CanvasHeight = canvasSize.Height;

            FrameWidth = this.profile.FrameWidth;
            FrameHeight = this.profile.FrameHeight;

            //-- 

            double frameX = (CanvasWidth - FrameWidth) * 0.5;
            double frameY = (CanvasHeight - FrameHeight) * 0.5;

            Canvas.SetLeft(_frame, frameX);
            Canvas.SetTop(_frame, frameY);

            //-- 

            this._scrollViewer.MinZoomFactor = (float)(this.profile.Zoom * 0.9);
            this._scrollViewer.ChangeView(0, 0, (float)this.profile.Zoom, true);
        }

        private void OnPointerPressedOnGrid(object sender, PointerRoutedEventArgs e)
        {
            this.stateExecutor.HandleInput(PointerState.Pressed, e);
        }

        private void OnPointerMovedOnGrid(object sender, PointerRoutedEventArgs e)
        {
            this.stateExecutor.HandleInput(PointerState.Moved, e);
        }

        private void OnPointerReleasedOnGrid(object sender, PointerRoutedEventArgs e)
        {
            this.stateExecutor.HandleInput(PointerState.Released, e);
        }

        private void OnScrollViewerZoomFactorChanged(DependencyObject sender, DependencyProperty dp)
        {
            this.stateExecutor.HandleZoom();
        }
    }
}
