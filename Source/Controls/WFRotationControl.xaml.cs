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
using Path = Windows.UI.Xaml.Shapes.Path;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class WFRotationControl : UserControl
    {
        private Point axisPoint = new Point(1000, 1000);

        public Point AxisPoint { get => this.axisPoint; }

        public WFRotationControl()
        {
            this.InitializeComponent();
        }

        public void Rotate(double startAngle, double endAngle)
        {
            DrawArc(this.axisPoint, 100.0, startAngle, endAngle);
        }


        private void DrawArc(Point center, double radius, double start_angle, double end_angle)
        {
            Canvas.SetLeft(_path, 0);
            Canvas.SetTop(_path, 0);

            start_angle = ((start_angle % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);
            end_angle = ((end_angle % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);
            if (end_angle < start_angle)
            {
                double temp_angle = end_angle;
                end_angle = start_angle;
                start_angle = temp_angle;
            }
            double angle_diff = Math.Abs(end_angle - start_angle) ;
            _arcSegment.IsLargeArc = angle_diff >= Math.PI;
            //Set start of arc
            _pathFigure.StartPoint = new Point(center.X + radius * Math.Cos(start_angle), center.Y + radius * Math.Sin(start_angle));
            //set end point of arc.
            _arcSegment.Point = new Point(center.X + radius * Math.Cos(end_angle), center.Y + radius * Math.Sin(end_angle));
            _arcSegment.Size = new Size(radius, radius);
            _arcSegment.SweepDirection = SweepDirection.Clockwise;

            _line1.StartPoint = center;
            _line1.EndPoint = _pathFigure.StartPoint;

            _line2.StartPoint = center;
            _line2.EndPoint = _arcSegment.Point;
        }
    }
}
