using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Shapes
{
    public sealed partial class EllipseShape : UserControl, IShape, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        // --

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        // --

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        // --

        public static readonly DependencyProperty BreathProperty = DependencyProperty.Register(nameof(Breath), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double Breath { get => (double)GetValue(BreathProperty); set => SetValue(BreathProperty, value); }

        // --
        
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }

        // --

        public static readonly DependencyProperty ColorFillProperty = DependencyProperty.Register(nameof(Fill), typeof(SolidColorBrush), typeof(CompoundShape), new PropertyMetadata(null));
        public Brush Fill { get => (Brush)GetValue(ColorFillProperty); set => SetValue(ColorFillProperty, value); }

        // --

        public static readonly DependencyProperty PathStretchProperty = DependencyProperty.Register(nameof(PathStretch), typeof(Stretch), typeof(CompoundShape), new PropertyMetadata(null));
        public Stretch PathStretch { get => (Stretch)GetValue(PathStretchProperty); set => SetValue(PathStretchProperty, value); }

        // --

        public static readonly DependencyProperty ViewStretchProperty = DependencyProperty.Register(nameof(ViewStretch), typeof(Stretch), typeof(CompoundShape), new PropertyMetadata(null));
        public Stretch ViewStretch { get => (Stretch)GetValue(ViewStretchProperty); set => SetValue(ViewStretchProperty, value); }

        // --

        public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register(nameof(FillRule), typeof(FillRule), typeof(CompoundShape), new PropertyMetadata(null));
        public FillRule FillRule { get => (FillRule)GetValue(FillRuleProperty); set => SetValue(FillRuleProperty, value); }

        // --

        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double ScaleX { get => (double)GetValue(ScaleXProperty); set => SetValue(ScaleXProperty, value); }

        // --

        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(CompoundShape), new PropertyMetadata(null));
        public double ScaleY { get => (double)GetValue(ScaleYProperty); set => SetValue(ScaleYProperty, value); }


        // --

        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public EllipseShape()
        {
            this.InitializeComponent();

            ScaleX = ScaleY = 1.0;

            Stroke = new SolidColorBrush(Colors.Blue);
            Fill = new SolidColorBrush(Colors.AliceBlue);
            FillRule = FillRule.EvenOdd;
            PathStretch = Stretch.Fill;
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double GetLeft()
        {
            return this.Left;
        }

        public void SetLeft(double left)
        {
            this.Left = left;
        }

        public double GetTop()
        {
            return this.Top;
        }

        public void SetTop(double top)
        {
            this.Top = top;
        }

        public double GetLength()
        {
            return this.Length;
        }

        public void SetLength(double length)
        {
            this.Length = length;
        }

        public double GetBreath()
        {
            return this.Breath;
        }

        public void SetBreath(double breath)
        {
            this.Breath = breath;
        }

        public void GetScale(ref double x, ref double y)
        {
            x = ScaleX;
            y = ScaleY;
        }

        public void SetScale(double x, double y)
        {
            ScaleX = x;
            ScaleY = y;
        }

        public void SetViewbox(Viewbox viewbox)
        {
            this._viewbox = viewbox;
        }

        public Viewbox GetViewbox()
        {
            return this._viewbox;
        }

        public void SetPath(Path path)
        {
            this._path = path;
        }

        public Path GetPath()
        {
            return this._path;
        }
    }
}
