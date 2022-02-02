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
        public WFTitleBox()
        {
            this.InitializeComponent();

            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Width < _textBorder.Width || e.NewSize.Height < _textBorder.Height)
            {
                _textBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                _textBorder.Visibility = Visibility.Visible;

                Canvas.SetLeft(_textBorder, 0);
                Canvas.SetTop(_textBorder, Canvas.GetTop(_canvas) - _textBorder.Height);
            }

            Canvas.SetLeft(_box, 0);
            Canvas.SetTop(_box, 0);
            _box.Width = e.NewSize.Width;
            _box.Height = e.NewSize.Height;
        }

        public void SetTitle(string title)
        {
            _textBlock.Text = title;
        }
    }
}
