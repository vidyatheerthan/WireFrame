using System;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.DrawArea.Controls.Gizmo;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class RotationControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ArcRadiusProperty = DependencyProperty.Register(nameof(ArcRadius), typeof(double), typeof(RotationControl), new PropertyMetadata(null));

        public double ArcRadius { 
            get => (double)GetValue(ArcRadiusProperty); 
            set => SetValue(ArcRadiusProperty, value); 
        }

        // --

        public static readonly DependencyProperty AxisProperty = DependencyProperty.Register(nameof(Axis), typeof(Point), typeof(RotationControl), new PropertyMetadata(null));

        public Point Axis { 
            get => (Point)GetValue(AxisProperty);
            set { 
                SetValue(AxisProperty, value);
                OnPropertyChanged(nameof(Axis));
            } 
        }

        // --

        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public RotationControl()
        {
            this.InitializeComponent();

            this.DataContext = this; // important: set this to receive change to DependencyProperty from other classes

            PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
                if(e.PropertyName == nameof(Axis))
                {
                    Canvas.SetLeft(_grid, Axis.X - _grid.Width * 0.5);
                    Canvas.SetTop(_grid, Axis.Y - _grid.Height * 0.5);

                    _line1.Point = Axis;
                }
            };

            new RotateGizmo(this);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetArc(bool isLargeArc, Point startPoint, Point endPoint, Size arcSize, SweepDirection dir)
        {
            _line2.Point = startPoint;

            _pathFigure.StartPoint = startPoint;

            _arcSegment.IsLargeArc = isLargeArc;
            _arcSegment.Point = endPoint;
            _arcSegment.Size = arcSize;
            _arcSegment.SweepDirection = dir;
        }

    }
}
