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
    public sealed partial class WFTitleBox : UserControl
    {
        private double zoomFactor = 1.0;

        private double textBlockInitialWidth, textBlockInitialHeight;
        private double textBlockInitialCornerRadiusTopLeft, textBlockInitialCornerRadiusTopRight, textBlockInitialCornerRadiusBottomLeft, textBlockInitialCornerRadiusBottomRight;
        private double textInitialFontSize;
        private double boxInitialStrokeThickness;

        public WFTitleBox()
        {
            this.InitializeComponent();

            // --
            this.textBlockInitialWidth = _textBlock.Width;
            this.textBlockInitialHeight = _textBlock.Height;

            this.textBlockInitialCornerRadiusTopLeft = _textBlock.CornerRadius.TopLeft;
            this.textBlockInitialCornerRadiusTopRight = _textBlock.CornerRadius.TopRight;
            this.textBlockInitialCornerRadiusBottomLeft = _textBlock.CornerRadius.BottomLeft;
            this.textBlockInitialCornerRadiusBottomRight = _textBlock.CornerRadius.BottomRight;

            this.textInitialFontSize = _text.FontSize;

            this.boxInitialStrokeThickness = _box.StrokeThickness;

            // --
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double zoom = ((11.0 - zoomFactor) / 10.0);

            if (e.NewSize.Width < _textBlock.Width || e.NewSize.Height < _textBlock.Height)
            {
                _textBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                _textBlock.Visibility = Visibility.Visible;

                _textBlock.Width = this.textBlockInitialWidth * zoom;
                _textBlock.Height = this.textBlockInitialHeight * zoom;

                _textBlock.CornerRadius = new CornerRadius (this.textBlockInitialCornerRadiusTopLeft * zoom,
                                                            this.textBlockInitialCornerRadiusTopRight * zoom,
                                                            this.textBlockInitialCornerRadiusBottomLeft * zoom,
                                                            this.textBlockInitialCornerRadiusBottomRight * zoom);

                _text.FontSize = this.textInitialFontSize * zoom;

                Canvas.SetLeft(_textBlock, 0);
                Canvas.SetTop(_textBlock, Canvas.GetTop(_canvas) - _textBlock.Height);
            }

            Canvas.SetLeft(_box, 0);
            Canvas.SetTop(_box, 0);
            _box.Width = e.NewSize.Width;
            _box.Height = e.NewSize.Height;

            _box.StrokeThickness = this.boxInitialStrokeThickness * zoom;
        }

        public void SetTitle(string title)
        {
            _text.Text = title;
        }

        public void SetZoomFactor(double zoomFactor)
        {
            this.zoomFactor = zoomFactor;
        }
    }
}
