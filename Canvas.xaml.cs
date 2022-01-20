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
        public Canvas()
        {
            this.InitializeComponent();

            X_HorizontalRuler.Zoom(X_ScrollViewer.ZoomFactor);
            X_VerticalRuler.Zoom(X_ScrollViewer.ZoomFactor);

            SizeChanged += WindowSizeChanged;
            X_ScrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, ZoomHandler);
        }

        private async void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            X_Grid.GridWidth = (int)(X_ContainerGrid.ActualWidth);
            X_Grid.GridHeight = (int)(X_ContainerGrid.ActualHeight);

            X_HorizontalRuler.RulerLength = (int)(X_ContainerGrid.ActualWidth);
            X_VerticalRuler.RulerLength = (int)(X_ContainerGrid.ActualHeight);
        }

        private void ZoomHandler(DependencyObject sender, DependencyProperty dp)
        {
            X_HorizontalRuler.Zoom(X_ScrollViewer.ZoomFactor);
            X_VerticalRuler.Zoom(X_ScrollViewer.ZoomFactor);
        }
    }
}
