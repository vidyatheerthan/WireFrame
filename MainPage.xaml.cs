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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WireFrame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const double CANVAS_DIVISOR = 100;

        private const double CANVAS_WIDTH = 6000;
        private const double CANVAS_HEIGHT = 4000;

        private double zoom = 1.0;

        public MainPage()
        {
            this.InitializeComponent();

            PointerWheelChanged += OnPointerWheelChanged;
            SizeChanged += OnSizeChanged;
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            double delta = e.GetCurrentPoint(this).Properties.MouseWheelDelta / 120.0;

            zoom = Math.Max(1, Math.Min(100, zoom + delta));

            UpdateCanvasSize();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCanvasSize();
        }

        private void UpdateCanvasSize()
        {
            _canvas.Width = (CANVAS_WIDTH > CANVAS_DIVISOR ? (CANVAS_WIDTH / CANVAS_DIVISOR) : CANVAS_WIDTH) * this.zoom;
            _canvas.Height = (CANVAS_HEIGHT > CANVAS_DIVISOR ? (CANVAS_HEIGHT / CANVAS_DIVISOR) : CANVAS_HEIGHT) * this.zoom;
        }
    }
}
