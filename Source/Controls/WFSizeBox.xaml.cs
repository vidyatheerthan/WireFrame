using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class WFSizeBox : UserControl, IElementSelector
    {
        const double STROKE_THICKNESS = 10.0;

        public WFSizeBox()
        {
            this.InitializeComponent();

            _hit_box.StrokeThickness = STROKE_THICKNESS;
        }

        public void SetSelectedElement(FrameworkElement element, FrameworkElement parent, float zoomFactor)
        {
            UpdateBox(element, parent, zoomFactor);
            UpdateHitBox();
            UpdateCircles();
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateBox(FrameworkElement element, FrameworkElement parent, float zoomFactor)
        {
            var transform = element.TransformToVisual(parent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(_box, ePoint.X);
            Canvas.SetTop(_box, ePoint.Y);
            _box.Width = element.ActualWidth * zoomFactor;
            _box.Height = element.ActualHeight * zoomFactor;
        }

        private void UpdateHitBox()
        {
            const double HALF = STROKE_THICKNESS * 0.5;

            Canvas.SetLeft(_hit_box, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_hit_box, Canvas.GetTop(_box) - HALF);
            _hit_box.Width = _box.ActualWidth + STROKE_THICKNESS;
            _hit_box.Height = _box.ActualHeight + STROKE_THICKNESS;
        }

        private void UpdateCircles()
        {
            const double HALF = STROKE_THICKNESS * 0.5;

            // top left
            Canvas.SetLeft(_top_left_circle, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_top_left_circle, Canvas.GetTop(_box) - HALF);

            // top right
            Canvas.SetLeft(_top_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF);
            Canvas.SetTop(_top_right_circle, Canvas.GetTop(_box) - HALF);

            // bottom left
            Canvas.SetLeft(_bottom_left_circle, Canvas.GetLeft(_box) - HALF);
            Canvas.SetTop(_bottom_left_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF);

            // bottom right
            Canvas.SetLeft(_bottom_right_circle, Canvas.GetLeft(_box) + _box.ActualWidth - HALF);
            Canvas.SetTop(_bottom_right_circle, Canvas.GetTop(_box) + _box.ActualHeight - HALF);
        }
    }
}
