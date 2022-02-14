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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class WFSizeControl : UserControl, ISelector
    {
        const double HITBOX_SIZE = 10.0;

        private CoreCursor westEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private CoreCursor northSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);
        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private IShape selectedShape;
        private FrameworkElement shapeParent;

        // --

        public WFSizeControl()
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
        }

        public void SetSelectedShape(IShape shape, FrameworkElement parent, float zoomFactor)
        {
            this.selectedShape = shape;
            this.shapeParent = parent;

            UpdateSelectedShape(zoomFactor);
        }

        public void UpdateSelectedShape(float zoomFactor)
        {
            if (this.selectedShape == null || this.shapeParent == null) { return; }

            UpdateBox(this.selectedShape, this.shapeParent, zoomFactor);
            UpdateHitBox();
            UpdateCircles();
        }

        public IShape GetSelectedShape()
        {
            return this.selectedShape;
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateBox(IShape shape, FrameworkElement parent, float zoomFactor)
        {
            var transform = shape.GetPath().TransformToVisual(parent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(_box, ePoint.X);
            Canvas.SetTop(_box, ePoint.Y);
            _box.Width = shape.GetLength() * zoomFactor;
            _box.Height = shape.GetBreath() * zoomFactor;
        }

        private void UpdateHitBox()
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

        private void UpdateCircles()
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

        //
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
    }
}
