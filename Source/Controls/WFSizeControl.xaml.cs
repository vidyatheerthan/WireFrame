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

        private Dictionary<IShape, Size> shapeSizes = new Dictionary<IShape, Size>(); // each shape and their size contribution in _box
        private FrameworkElement container = null;

        private Point topLeft = new Point(0, 0);
        private Point bottomRight = new Point(0, 0);

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

        
        ///-------------------------------------------------------------------

        private void UpdateCorners(IShape shape, float zoomFactor, bool reset)
        {
            var transform = shape.GetPath().TransformToVisual(this.container);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            if (reset)
            {
                this.topLeft.X = ePoint.X;
                this.topLeft.Y = ePoint.Y;
            }
            else
            {
                if (ePoint.X < this.topLeft.X)
                {
                    this.topLeft.X = ePoint.X;
                }

                if (ePoint.Y < this.topLeft.Y)
                {
                    this.topLeft.Y = ePoint.Y;
                }
            }

            if (ePoint.X + (shape.GetLength() * zoomFactor) > this.bottomRight.X)
            {
                this.bottomRight.X = ePoint.X + (shape.GetLength() * zoomFactor);
            }

            if (ePoint.Y + (shape.GetBreath() * zoomFactor) > this.bottomRight.Y)
            {
                this.bottomRight.Y = ePoint.Y + (shape.GetBreath() * zoomFactor);
            }
        }

        private void UpdateBox()
        {
            Canvas.SetLeft(_box, this.topLeft.X);
            Canvas.SetTop(_box, this.topLeft.Y);
            _box.Width = this.bottomRight.X - this.topLeft.X;
            _box.Height = this.bottomRight.Y - this.topLeft.Y;
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

        ///-------------------------------------------------------------------


        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public bool AddShape(IShape shape)
        {
            if (shape == null || this.shapeSizes.ContainsKey(shape))
            { 
                return false;
            }

            this.shapeSizes.Add(shape, Size.Empty);

            return true;
        }

        public bool AddShapes(List<IShape> shapes)
        {
            bool newAddition = false;

            foreach (var shape in shapes)
            {
                if (AddShape(shape))
                {
                    newAddition = true;
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            var shapes = this.shapeSizes.Keys.ToList();
            return shapes;
        }

        public void UpdateShapes(float zoomFactor)
        {
            var shapes = GetShapes();

            for (int i = 0; i < shapes.Count; ++i)
            {
                IShape shape = shapes[i];

                UpdateCorners(shape, zoomFactor, i == 0);
                
                double width = shape.GetLength() * zoomFactor;
                double height = shape.GetBreath() * zoomFactor;
                this.shapeSizes[shape] = new Size(width/_box.Width, height/_box.Height);
            }

            UpdateBox();
            UpdateHitBox();
            UpdateCircles();
        }

        public void RemoveAllShapes()
        {
            this.shapeSizes.Clear();
            this.topLeft = new Point(0, 0);
            this.bottomRight = new Point(0, 0);
            _box.Width = 0.0;
            _box.Height = 0.0;
        }

        ///-------------------------------------------------------------------

        public Rect GetRect()
        {
            Rect r = new Rect(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            return r;
        }
    }
}
