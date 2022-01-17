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
    public sealed partial class Canvas : UserControl
    {
        private const int MAX_ZOOM_PIXELS = 1000; // in pixels
        private const int MIN_ZOOM_PIXELS = 10; // in pixels
        private int zoom = 0; // in pixels

        public Canvas()
        {
            this.InitializeComponent();

            zoom = MIN_ZOOM_PIXELS;

            X_HorizontalRuler.PixelsPerUnit = zoom;
            X_VerticalRuler.PixelsPerUnit = zoom;

            X_HorizontalRuler.Zoom(zoom);
            X_VerticalRuler.Zoom(zoom);

            SizeChanged += WindowSizeChanged;
            PointerWheelChanged += PointerWheelChangedHandler;
        }

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            X_Grid.GridWidth = (int)(X_ContainerGrid.ActualWidth);
            X_Grid.GridHeight = (int)(X_ContainerGrid.ActualHeight);

            X_HorizontalRuler.RulerLength = (int)(X_ContainerGrid.ActualWidth);
            X_VerticalRuler.RulerLength = (int)(X_ContainerGrid.ActualHeight);
        }

        private void PointerWheelChangedHandler(object sender, PointerRoutedEventArgs args)
        {
            int zoom = 0;
            var value = args.GetCurrentPoint(this).Properties.MouseWheelDelta;

            zoom = GetZoomFactor(value); // -120 (or) +120

            X_HorizontalRuler.Zoom(zoom);
            X_VerticalRuler.Zoom(zoom);
        }

        private int GetZoomFactor(int direction)
        {
            if(direction > 0) zoom = Math.Min(MAX_ZOOM_PIXELS, zoom * 2);
            else if (direction < 0) zoom = Math.Max(MIN_ZOOM_PIXELS, zoom / 2);

            return zoom;
        }
    }
}
