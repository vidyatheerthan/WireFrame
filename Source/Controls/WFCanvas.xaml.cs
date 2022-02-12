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
using Windows.System;
using Size = Windows.Foundation.Size;
using WireFrame.States;
using WireFrame.Misc;
using FocusState = WireFrame.States.FocusState;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
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
            IFiniteStateMachine panState = new PanState(new List<object>() { _grid, _scrollViewer, _canvas });
            IFiniteStateMachine selectionState = new SelectionState(new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _shapeHighlight });
            IFiniteStateMachine rotateElementState = new RotateElementState(new List<object>() { _grid, _scrollViewer, _canvas, _rotationControl });
            IFiniteStateMachine focusState = new FocusState(new List<object>() { _scrollViewer, _canvas, _sizeControl, VirtualKey.F });
            IFiniteStateMachine drawEllipseState = new DrawEllipseState(new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _actionTip });
            IFiniteStateMachine drawRectangleState = new DrawRectangleState(new List<object>() { _grid, _scrollViewer, _canvas, _container, _HUD, _actionTip });

            var stateGroups = new Dictionary<StateExecutor.StateGroup, List<IFiniteStateMachine>>();
            stateGroups.Add(StateExecutor.StateGroup.Selection_Pan_Focus, new List<IFiniteStateMachine>() { selectionState, panState, focusState });
            stateGroups.Add(StateExecutor.StateGroup.RotateElement, new List<IFiniteStateMachine>() { rotateElementState });
            stateGroups.Add(StateExecutor.StateGroup.DrawEllipse, new List<IFiniteStateMachine>() { drawEllipseState });
            stateGroups.Add(StateExecutor.StateGroup.DrawRectangle, new List<IFiniteStateMachine>() { drawRectangleState });

            this.stateExecutor = new StateExecutor(stateGroups);
            this.stateExecutor.SelectStateGroup(StateExecutor.StateGroup.Selection_Pan_Focus);

            // --
            this.Loaded += OnLoaded;

            // --
            this._grid.PointerPressed += OnPointerPressedOnGrid;
            this._grid.PointerMoved += OnPointerMovedOnGrid;
            this._grid.PointerReleased += OnPointerReleasedOnGrid;

            // --
            Window.Current.CoreWindow.KeyDown += OnKeyDown;

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
            float zoomFactor = 0.0f;

            var screenSize = Utility.GetScreenResolution();
            var canvasSize = this.profile.GetCanvas(screenSize, out zoomFactor);

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

            this._scrollViewer.MinZoomFactor = zoomFactor * 0.9f;
            this._scrollViewer.ChangeView(0, 0, zoomFactor, true);
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

        private void OnKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            this.stateExecutor.HandleInput(KeyBoardState.KeyDown, args);
        }

        private void OnScrollViewerZoomFactorChanged(DependencyObject sender, DependencyProperty dp)
        {
            this.stateExecutor.HandleZoom();
        }
    }
}
