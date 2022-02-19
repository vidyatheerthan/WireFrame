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
        const double HITBOX_SIZE = 10.0;

        private CoreCursor westEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private CoreCursor northSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);
        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Point hudTopLeft = new Point(0, 0);
        private Point hudBottomRight = new Point(0, 0);

        private Point canvasTopLeft = new Point(0, 0);
        private Point canvasBottomRight = new Point(0, 0);

        // --

        public MoveResizeControl()
        {
            this.InitializeComponent();

            _left_box.PointerEntered += OnPointerEnterLeftHitBox;
            _left_box.PointerExited += OnPointerExitedLeftHitBox;

            _right_box.PointerEntered += OnPointerEnterRightHitBox;
            _right_box.PointerExited += OnPointerExitedRightHitBox;

            _top_box.PointerEntered += OnPointerEnterTopHitBox;
            _top_box.PointerExited += OnPointerExitedTopHitBox;

            _bottom_box.PointerEntered += OnPointerEnterBottomHitBox;
            _bottom_box.PointerExited += OnPointerExitedBottomHitBox;

            _top_left_circle.PointerEntered += OnPointerEnterTopLeftHitBox;
            _top_left_circle.PointerExited += OnPointerExitedTopLeftHitBox;

            _top_right_circle.PointerEntered += OnPointerEnterTopRightHitBox;
            _top_right_circle.PointerExited += OnPointerExitedTopRightHitBox;

            _bottom_left_circle.PointerEntered += OnPointerEnterBottomLeftHitBox;
            _bottom_left_circle.PointerExited += OnPointerExitedBottomLeftHitBox;

            _bottom_right_circle.PointerEntered += OnPointerEnterBottomRightHitBox;
            _bottom_right_circle.PointerExited += OnPointerExitedBottomRightHitBox;

            _box.PointerPressed += OnPointerPressedOnBox;
            _box.PointerMoved += OnPointerMovedOnBox;
            _box.PointerReleased += OnPointerReleasedOnBox;
        }


        ///-------------------------------------------------------------------

        private void OnPointerEnterLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
        }

        private void OnPointerExitedLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterRightHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
        }

        private void OnPointerExitedRightHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
        }

        private void OnPointerExitedTopHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
        }

        private void OnPointerExitedBottomHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
        }

        private void OnPointerExitedTopLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterTopRightHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
        }

        private void OnPointerExitedTopRightHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
        }

        private void OnPointerExitedBottomLeftHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        //
        private void OnPointerEnterBottomRightHitBox(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
        }

        private void OnPointerExitedBottomRightHitBox(object sender, PointerRoutedEventArgs e)
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

        public void UpdateHitBox()
        {
            const double HALF = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(_left_box, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_left_box, Canvas.GetTop(_box));
            _left_box.Width = HITBOX_SIZE;
            _left_box.Height = _box.ActualHeight;

            Canvas.SetLeft(_right_box, Canvas.GetLeft(_box) + _box.ActualWidth - HALF);
            Canvas.SetTop(_right_box, Canvas.GetTop(_box));
            _right_box.Width = HITBOX_SIZE;
            _right_box.Height = _box.ActualHeight;

            Canvas.SetLeft(_top_box, Canvas.GetLeft(_box));
            Canvas.SetTop(_top_box, Canvas.GetTop(_box) - HALF);
            _top_box.Width = _box.ActualWidth;
            _top_box.Height = HITBOX_SIZE;

            Canvas.SetLeft(_bottom_box, Canvas.GetLeft(_box));
            Canvas.SetTop(_bottom_box, Canvas.GetTop(_box) + _box.ActualHeight - HALF);
            _bottom_box.Width = _box.ActualWidth;
            _bottom_box.Height = HITBOX_SIZE;
        }

        public void UpdateCircles()
        {
            const double HALF = HITBOX_SIZE * 0.5;

            // top left
            Canvas.SetLeft(_top_left_circle, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_top_left_circle, Canvas.GetTop(_box) - HALF);
            _top_left_circle.Width = HITBOX_SIZE;
            _top_left_circle.Height = HITBOX_SIZE;

            // top right
            Canvas.SetLeft(_top_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF);
            Canvas.SetTop(_top_right_circle, Canvas.GetTop(_box) - HALF);
            _top_right_circle.Width = HITBOX_SIZE;
            _top_right_circle.Height = HITBOX_SIZE;

            // bottom left
            Canvas.SetLeft(_bottom_left_circle, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_bottom_left_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF);
            _bottom_left_circle.Width = HITBOX_SIZE;
            _bottom_left_circle.Height = HITBOX_SIZE;

            // bottom right
            Canvas.SetLeft(_bottom_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF);
            Canvas.SetTop(_bottom_right_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF);
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
