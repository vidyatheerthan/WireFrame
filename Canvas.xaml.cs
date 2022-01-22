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

            X_HorizontalRuler.Scroll(X_ScrollViewer.HorizontalOffset);
            X_VerticalRuler.Scroll(X_ScrollViewer.VerticalOffset);

            X_ScrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, ZoomHandler);
            X_ScrollViewer.ViewChanged += ViewChangeHandler;
        }

        

        private void ZoomHandler(DependencyObject sender, DependencyProperty dp)
        {
            X_HorizontalRuler.Zoom(X_ScrollViewer.ZoomFactor);
            X_VerticalRuler.Zoom(X_ScrollViewer.ZoomFactor);
        }

        private void ViewChangeHandler(object sender, ScrollViewerViewChangedEventArgs e)
        {
            X_HorizontalRuler.Scroll(X_ScrollViewer.HorizontalOffset);
            X_VerticalRuler.Scroll(X_ScrollViewer.VerticalOffset);
        }
    }
}
