using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Shapes;
using Path = Windows.UI.Xaml.Shapes.Path;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class RotationControl : UserControl
    {
        private Point axisPoint = new Point(1000, 1000);
        private double arcRadius = 10.0;
        private double innerRingRadius = 1.0;
        private double outerRing1Radius = 10.0;
        private double outerRing2Radius = 11.0;

        public Point AxisPoint { get => this.axisPoint; }

        public RotationControl()
        {
            this.InitializeComponent();

            UpdateRing(_inner_ring, this.axisPoint, this.innerRingRadius);
            UpdateRing(_outer_ring_1, this.axisPoint, this.outerRing1Radius);
            UpdateRing(_outer_ring_2, this.axisPoint, this.outerRing2Radius);
            DrawArc(this.axisPoint, this.arcRadius, 0.0, 0.0);
        }

        private void UpdateRing(Ellipse ring, Point center, double radius)
        {
            ring.Width = ring.Height = 2 * radius;
            Canvas.SetLeft(ring, center.X - radius);
            Canvas.SetTop(ring, center.Y - radius);
        }

        public void Rotate(double startAngle, double endAngle)
        {
            DrawArc(this.axisPoint, this.arcRadius, startAngle, endAngle);
        }


        private void DrawArc(Point center, double radius, double start_angle, double end_angle)
        {
            Canvas.SetLeft(_path, 0);
            Canvas.SetTop(_path, 0);

            start_angle = SanitizeAngle(start_angle);
            end_angle = SanitizeAngle(end_angle);

            if (end_angle < start_angle)
            {
                double temp_angle = end_angle;
                end_angle = start_angle;
                start_angle = temp_angle;
            }

            double angle_diff = Math.Abs(end_angle - start_angle) ;

            _arcSegment.IsLargeArc = angle_diff >= Math.PI;
            _arcSegment.Point = PolarToCartesian(end_angle, radius, center);
            _arcSegment.Size = new Size(radius, radius);
            _arcSegment.SweepDirection = SweepDirection.Clockwise;

            _pathFigure.StartPoint = PolarToCartesian(start_angle, radius, center);

            _line1.Point = center;
            _line2.Point = _pathFigure.StartPoint;
        }

        public static Point PolarToCartesian(double angle, double radius, Point center)
        {
            return new Point((center.X + (radius * Math.Cos(angle))), (center.Y + (radius * Math.Sin(angle))));
        }

        private static Double SanitizeAngle(double angle)
        {
            return ((angle % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);
        }
    }
}
