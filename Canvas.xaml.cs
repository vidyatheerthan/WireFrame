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
        private int zoom = 0;

        public Canvas()
        {
            this.InitializeComponent();

            SizeChanged += WindowSizeChanged;
            //X_ScrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, ZoomHandler);

            PointerWheelChanged += PointerWheelChangedHandler;
        }

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            X_Grid.GridWidth = (int)(X_ContainerGrid.ActualWidth);
            X_Grid.GridHeight = (int)(X_ContainerGrid.ActualHeight);

            X_HorizontalRuler.RulerLength = (int)(X_ContainerGrid.ActualWidth);
            X_VerticalRuler.RulerLength = (int)(X_ContainerGrid.ActualHeight);
        }

        //private void ZoomHandler(DependencyObject sender, DependencyProperty dp)
        //{
        //    X_HorizontalRuler.Zoom(X_ScrollViewer.ZoomFactor);
        //    X_VerticalRuler.Zoom(X_ScrollViewer.ZoomFactor);
        //}

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
            if (direction > 0) zoom = Math.Max(1, zoom - 1); 
            else if (direction < 0) ++this.zoom;

            return zoom;
        }
    }
}
