using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }

        // --

        public static readonly DependencyProperty ColorFillProperty = DependencyProperty.Register(nameof(Fill), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Fill { get => (Brush)GetValue(ColorFillProperty); set => SetValue(ColorFillProperty, value); }

        // --

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(EllipseShape), new PropertyMetadata(null));
        public Stretch Stretch { get => (Stretch)GetValue(StretchProperty); set => SetValue(StretchProperty, value); }

        // --

        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public EllipseShape()
        {
            this.InitializeComponent();

            Stroke = new SolidColorBrush(Colors.Blue);
            Fill = new SolidColorBrush(Colors.AliceBlue);
            Stretch = Stretch.Fill;
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
