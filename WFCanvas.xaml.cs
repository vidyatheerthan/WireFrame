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
    public sealed partial class WFCanvas : UserControl
    {
        private Size size;

        Size es, rs;
        Point ep, rp;

        public WFCanvas()
        {
            this.InitializeComponent();


            
            es.Width = _ellipse.Width;
            es.Height = _ellipse.Height;

            rs.Width = _rectangle.Width;
            rs.Height = _rectangle.Height;

            ep.X = Canvas.GetLeft(_ellipse);
            ep.Y = Canvas.GetTop(_ellipse);

            rp.X = Canvas.GetLeft(_rectangle);
            rp.Y = Canvas.GetTop(_rectangle);

            SizeChanged += OnSizeChanged;
        }

        public void SetCanvasSize(double width, double height)
        {
            this.size.Width = width;
            this.size.Height = height;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double widthRatio = ActualWidth / this.size.Width;
            double heightRatio = ActualHeight / this.size.Height;

            _ellipse.Width = es.Width * widthRatio;
            _ellipse.Height = es.Height * heightRatio;

            _rectangle.Width = rs.Width * widthRatio;
            _rectangle.Height = rs.Height * heightRatio;

            Canvas.SetLeft(_ellipse, ep.X * widthRatio);
            Canvas.SetTop(_ellipse, ep.Y * heightRatio);

            Canvas.SetLeft(_rectangle, rp.X * widthRatio);
            Canvas.SetTop(_rectangle, rp.Y * heightRatio);
        }
    }
}
