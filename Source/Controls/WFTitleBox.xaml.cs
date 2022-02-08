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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame
{
    public sealed partial class WFTitleBox : UserControl, IElementSelector
    {
        public WFTitleBox()
        {
            this.InitializeComponent();
        }

        public void SetSelectedElement(FrameworkElement element, FrameworkElement parent, float zoomFactor)
        {
            var transform = element.TransformToVisual(parent);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(_box, ePoint.X);
            Canvas.SetTop(_box, ePoint.Y);
            _box.Width = element.ActualWidth * zoomFactor;
            _box.Height = element.ActualHeight * zoomFactor;

            UpdateTextBoxSize();
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
