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
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class WFTitleBox : UserControl, IElementSelector
    {
        private IShape selectedShape;

        // --

        public WFTitleBox()
        {
            this.InitializeComponent();
        }

        public void SetSelectedShape(IShape shape, FrameworkElement parent, float zoomFactor)
        {
            this.selectedShape = shape;

            var transform = shape.GetPath().TransformToVisual(parent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(_box, ePoint.X);
            Canvas.SetTop(_box, ePoint.Y);
            _box.Width = shape.GetLength() * zoomFactor;
            _box.Height = shape.GetBreath() * zoomFactor;

            UpdateTextBoxSize();
        }

        public IShape GetSelectedShape()
        {
            return this.selectedShape;
        }

        private void UpdateTextBoxSize()
        {
            if (_box.ActualWidth < _textBorder.Width || _box.ActualHeight < _textBorder.Height)
            {
                _textBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                _textBorder.Visibility = Visibility.Visible;

                Canvas.SetLeft(_textBorder, Canvas.GetLeft(_box));
                Canvas.SetTop(_textBorder, Canvas.GetTop(_box) - _textBorder.Height);
            }
        }

        public void SetTitle(string title)
        {
            _textBlock.Text = title;
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
