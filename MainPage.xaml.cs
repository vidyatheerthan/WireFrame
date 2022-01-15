using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
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
        public MainPage()
        {
            this.InitializeComponent();

            Action NotifySizeChange = () =>
            {
                var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
                var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

                double Width = bounds.Width * scaleFactor;
                double Height = bounds.Height * scaleFactor;

                Resize(Width, Height);
            };

            Window.Current.CoreWindow.SizeChanged += async (ss, ee) =>
            {
                NotifySizeChange();
                ee.Handled = true;
            };

            NotifySizeChange();
        }

        private void Resize(double Width, double Height)
        {
            X_CanvasPanel_Grid.GridWidth = (int) X_CanvasPanel.ActualWidth;
            X_CanvasPanel_Grid.GridHeight = (int) X_CanvasPanel.ActualHeight;

            X_CanvasPanel_HorizontalRuler.RulerLength = (int) X_CanvasPanel.ActualWidth;
            X_CanvasPanel_VerticalRuler.RulerLength = (int) X_CanvasPanel.ActualHeight;
        }
    }
}
