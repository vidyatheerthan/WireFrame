using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WireFrame.Shapes;
using WireFrame.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class MoveResizeControl : UserControl
    {
        private enum ResizeGizmo
        {
            None,
            TopBar,
            BottomBar,
            LeftBar,
            RightBar,
            TopSqr,
            BottomSqr,
            LeftSqr,
            RightSqr,
            TopLeftCircle,
            TopRightCircle,
            BottomLeftCircle,
            BottomRightCircle
        }

        const double HITBOX_SIZE = 10.0;

        private CoreCursor westEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private CoreCursor northSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);
        private CoreCursor upArrowCursor = new CoreCursor(CoreCursorType.UpArrow, 1);
        private CoreCursor downArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private CoreCursor leftArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private CoreCursor rightArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Point hudTopLeft = new Point(0, 0);
        private Point hudBottomRight = new Point(0, 0);

        private Point canvasTopLeft = new Point(0, 0);
        private Point canvasBottomRight = new Point(0, 0);

        private ResizeGizmo activeResizeGizmo = ResizeGizmo.None;

        // --

        public MoveResizeControl()
        {
            this.InitializeComponent();

            // --

            _left_bar.PointerEntered += OnPointerEnterLeftBar;
            _left_bar.PointerExited += OnPointerExitedLeftBar;

            _right_bar.PointerEntered += OnPointerEnterRightBar;
            _right_bar.PointerExited += OnPointerExitedRightBar;

            _top_bar.PointerEntered += OnPointerEnterTopBar;
            _top_bar.PointerExited += OnPointerExitedTopBar;

            _bottom_bar.PointerEntered += OnPointerEnterBottomBar;
            _bottom_bar.PointerExited += OnPointerExitedBottomBar;

            // --

            _left_sqr.PointerEntered += OnPointerEnterLeftSqr;
            _left_sqr.PointerExited += OnPointerExitedLeftSqr;

            _right_sqr.PointerEntered += OnPointerEnterRightSqr;
            _right_sqr.PointerExited += OnPointerExitedRightSqr;

            _top_sqr.PointerEntered += OnPointerEnterTopSqr;
            _top_sqr.PointerExited += OnPointerExitedTopSqr;

            _bottom_sqr.PointerEntered += OnPointerEnterBottomSqr;
            _bottom_sqr.PointerExited += OnPointerExitedBottomSqr;

            // --

            _top_left_circle.PointerEntered += OnPointerEnterTopLeftCircle;
            _top_left_circle.PointerExited += OnPointerExitedTopLeftCircle;

            _top_right_circle.PointerEntered += OnPointerEnterTopRightCircle;
            _top_right_circle.PointerExited += OnPointerExitedTopRightCircle;

            _bottom_left_circle.PointerEntered += OnPointerEnterBottomLeftCircle;
            _bottom_left_circle.PointerExited += OnPointerExitedBottomLeftCircle;

            _bottom_right_circle.PointerEntered += OnPointerEnterBottomRightCircle;
            _bottom_right_circle.PointerExited += OnPointerExitedBottomRightCircle;

            _box.PointerPressed += OnPointerPressedOnBox;
            _box.PointerMoved += OnPointerMovedOnBox;
            _box.PointerReleased += OnPointerReleasedOnBox;
        }


        ///-------------------------------------------------------------------

        private void OnPointerEnterLeftBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
        }

        private void OnPointerExitedLeftBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterRightBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
        }

        private void OnPointerExitedRightBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
        }

        private void OnPointerExitedTopBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
        }

        private void OnPointerExitedBottomBar(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        ///-------------------------------------------------------------------

        private void OnPointerEnterLeftSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.leftArrowCursor;
        }

        private void OnPointerExitedLeftSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterRightSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.rightArrowCursor;
        }

        private void OnPointerExitedRightSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.upArrowCursor;
        }

        private void OnPointerExitedTopSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.downArrowCursor;
        }

        private void OnPointerExitedBottomSqr(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        ///-------------------------------------------------------------------

        //
        private void OnPointerEnterTopLeftCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
        }

        private void OnPointerExitedTopLeftCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopRightCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
        }

        private void OnPointerExitedTopRightCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomLeftCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
        }

        private void OnPointerExitedBottomLeftCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomRightCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
        }

        private void OnPointerExitedBottomRightCircle(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        private void OnPointerPressedOnBox(object sender, PointerRoutedEventArgs e)
        {
            //Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Selection_Pan);
        }

        private void OnPointerMovedOnBox(object sender, PointerRoutedEventArgs e)
        {

        }

        private void OnPointerReleasedOnBox(object sender, PointerRoutedEventArgs e)
        {
            //Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Selection_Pan_Focus);
        }

        ///-------------------------------------------------------------------

        public void UpdateCorners(FrameworkElement container, IShape shape, float zoomFactor, bool reset)
        {
            var transform = shape.GetPath().TransformToVisual(container);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            if (reset)
            {
                this.hudTopLeft.X = ePoint.X;
                this.hudTopLeft.Y = ePoint.Y;

                this.canvasTopLeft.X = shape.GetLeft();
                this.canvasTopLeft.Y = shape.GetTop();
            }
            else
            {
                if (ePoint.X < this.hudTopLeft.X)
                {
                    this.hudTopLeft.X = ePoint.X;
                    this.canvasTopLeft.X = shape.GetLeft();
                }

                if (ePoint.Y < this.hudTopLeft.Y)
                {
                    this.hudTopLeft.Y = ePoint.Y;
                    this.canvasTopLeft.Y = shape.GetTop();
                }
            }

            if (ePoint.X + (shape.GetLength() * zoomFactor) > this.hudBottomRight.X)
            {
                this.hudBottomRight.X = ePoint.X + (shape.GetLength() * zoomFactor);
                this.canvasBottomRight.X = shape.GetLeft() + shape.GetLength();
            }

            if (ePoint.Y + (shape.GetBreath() * zoomFactor) > this.hudBottomRight.Y)
            {
                this.hudBottomRight.Y = ePoint.Y + (shape.GetBreath() * zoomFactor);
                this.canvasBottomRight.Y = shape.GetTop() + shape.GetBreath();
            }
        }

        public void UpdateBox()
        {
            Canvas.SetLeft(_box, this.hudTopLeft.X);
            Canvas.SetTop(_box, this.hudTopLeft.Y);
            _box.Width = this.hudBottomRight.X - this.hudTopLeft.X;
            _box.Height = this.hudBottomRight.Y - this.hudTopLeft.Y;
        }

        public void UpdateBars()
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(_left_bar, Canvas.GetLeft(_box) - HALF_HIT);
            Canvas.SetTop(_left_bar, Canvas.GetTop(_box));
            _left_bar.Width = HITBOX_SIZE;
            _left_bar.Height = _box.ActualHeight;

            Canvas.SetLeft(_right_bar, Canvas.GetLeft(_box) + _box.ActualWidth - HALF_HIT);
            Canvas.SetTop(_right_bar, Canvas.GetTop(_box));
            _right_bar.Width = HITBOX_SIZE;
            _right_bar.Height = _box.ActualHeight;

            Canvas.SetLeft(_top_bar, Canvas.GetLeft(_box));
            Canvas.SetTop(_top_bar, Canvas.GetTop(_box) - HALF_HIT);
            _top_bar.Width = _box.ActualWidth;
            _top_bar.Height = HITBOX_SIZE;

            Canvas.SetLeft(_bottom_bar, Canvas.GetLeft(_box));
            Canvas.SetTop(_bottom_bar, Canvas.GetTop(_box) + _box.ActualHeight - HALF_HIT);
            _bottom_bar.Width = _box.ActualWidth;
            _bottom_bar.Height = HITBOX_SIZE;
        }

        public void UpdateSqrs()
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;
            double HALF_WIDTH = _box.ActualWidth * 0.5;
            double HALF_HEIGHT = _box.ActualHeight * 0.5;

            Canvas.SetLeft(_left_sqr, Canvas.GetLeft(_box) - HALF_HIT);
            Canvas.SetTop(_left_sqr, Canvas.GetTop(_box) + HALF_HEIGHT - HALF_HIT);
            _left_sqr.Width = HITBOX_SIZE;
            _left_sqr.Height = HITBOX_SIZE;

            Canvas.SetLeft(_right_sqr, Canvas.GetLeft(_box) + _box.ActualWidth - HALF_HIT);
            Canvas.SetTop(_right_sqr, Canvas.GetTop(_box) + HALF_HEIGHT- HALF_HIT);
            _right_sqr.Width = HITBOX_SIZE;
            _right_sqr.Height = HITBOX_SIZE;

            Canvas.SetLeft(_top_sqr, Canvas.GetLeft(_box) + HALF_WIDTH - HALF_HIT);
            Canvas.SetTop(_top_sqr, Canvas.GetTop(_box) - HALF_HIT);
            _top_sqr.Width = HITBOX_SIZE;
            _top_sqr.Height = HITBOX_SIZE;

            Canvas.SetLeft(_bottom_sqr, Canvas.GetLeft(_box) + HALF_WIDTH - HALF_HIT);
            Canvas.SetTop(_bottom_sqr, Canvas.GetTop(_box) + _box.ActualHeight - HALF_HIT);
            _bottom_sqr.Width = HITBOX_SIZE;
            _bottom_sqr.Height = HITBOX_SIZE;
        }

        public void UpdateCircles()
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            // top left
            Canvas.SetLeft(_top_left_circle, Canvas.GetLeft(_box) - HALF_HIT);
            Canvas.SetTop(_top_left_circle, Canvas.GetTop(_box) - HALF_HIT);
            _top_left_circle.Width = HITBOX_SIZE;
            _top_left_circle.Height = HITBOX_SIZE;

            // top right
            Canvas.SetLeft(_top_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF_HIT);
            Canvas.SetTop(_top_right_circle, Canvas.GetTop(_box) - HALF_HIT);
            _top_right_circle.Width = HITBOX_SIZE;
            _top_right_circle.Height = HITBOX_SIZE;

            // bottom left
            Canvas.SetLeft(_bottom_left_circle, Canvas.GetLeft(_box) - HALF_HIT);
            Canvas.SetTop(_bottom_left_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF_HIT);
            _bottom_left_circle.Width = HITBOX_SIZE;
            _bottom_left_circle.Height = HITBOX_SIZE;

            // bottom right
            Canvas.SetLeft(_bottom_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF_HIT);
            Canvas.SetTop(_bottom_right_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF_HIT);
            _bottom_right_circle.Width = HITBOX_SIZE;
            _bottom_right_circle.Height = HITBOX_SIZE;
        }

        public void ResetBounds()
        {
            this.hudTopLeft = new Point(0, 0);
            this.hudBottomRight = new Point(0, 0);

            this.canvasTopLeft = new Point(0, 0);
            this.canvasBottomRight = new Point(0, 0);

            _box.Width = 0.0;
            _box.Height = 0.0;
        }

        ///-------------------------------------------------------------------

        public Rect GetHudRect()
        {
            Rect r = new Rect(hudTopLeft.X, hudTopLeft.Y, hudBottomRight.X - hudTopLeft.X, hudBottomRight.Y - hudTopLeft.Y);
            return r;
        }

        public Rect GetCanvasRect()
        {
            Rect r = new Rect(canvasTopLeft.X, canvasTopLeft.Y, canvasBottomRight.X - canvasTopLeft.X, canvasBottomRight.Y - canvasTopLeft.Y);
            return r;
        }
    }
}
