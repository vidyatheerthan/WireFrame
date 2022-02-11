using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WireFrame.Shapes
{
    public sealed partial class EllipseShape : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(EllipseShape), new PropertyMetadata(null));
        public Point Center { get => (Point)GetValue(CenterProperty); set => SetValue(CenterProperty, value); }

        // --

        public static readonly DependencyProperty XRadiusProperty = DependencyProperty.Register(nameof(XRadius), typeof(double), typeof(EllipseShape), new PropertyMetadata(null));
        public double XRadius { get => (double)GetValue(XRadiusProperty); set => SetValue(XRadiusProperty, value); }

        // --

        public static readonly DependencyProperty YRadiusProperty = DependencyProperty.Register(nameof(YRadius), typeof(double), typeof(EllipseShape), new PropertyMetadata(null));
        public double YRadius { get => (double)GetValue(YRadiusProperty); set => SetValue(YRadiusProperty, value); }

        // --

        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public EllipseShape()
        {
            this.InitializeComponent();
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
