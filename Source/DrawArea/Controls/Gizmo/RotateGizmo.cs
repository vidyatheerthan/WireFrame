using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    class RotateGizmo : IGizmoHandler
    {
        private RotationControl rotationControl;

        // --

        public RotateGizmo(RotationControl rotationControl)
        {
            this.rotationControl = rotationControl;

            TrackPointer(new Point(1000, 1000)); // test
        }

        // --


        public void StartTrackingPointer(Point pointer)
        {
        }

        public void TrackPointer(Point pointer)
        {
            double startAngle = Math.Atan2(pointer.Y - this.rotationControl.Axis.Y, pointer.X - this.rotationControl.Axis.X);
            double endAngle = 2 * Math.PI - 0.0001;

            DrawArc(startAngle, endAngle);
        }

        public void StopTrackingPointer(Point pointer)
        {
        }

        // --

        private void DrawArc(double start_angle, double end_angle)
        {
            Point center = this.rotationControl.Axis;
            double radius = this.rotationControl.ArcRadius;

            start_angle = SanitizeAngle(start_angle);
            end_angle = SanitizeAngle(end_angle);

            if (end_angle < start_angle)
            {
                double temp_angle = end_angle;
                end_angle = start_angle;
                start_angle = temp_angle;
            }

            double angle_diff = Math.Abs(end_angle - start_angle);

            bool isLargeArc = angle_diff >= Math.PI;
            Point endPoint = PolarToCartesian(end_angle, radius, center);
            Size arcSize = new Size(radius, radius);
            SweepDirection dir = SweepDirection.Clockwise;

            Point startPoint = PolarToCartesian(start_angle, radius, center);

            rotationControl.SetArc(isLargeArc, startPoint, endPoint, arcSize, dir);
        }

        private Point PolarToCartesian(double angle, double radius, Point center)
        {
            return new Point((center.X + (radius * Math.Cos(angle))), (center.Y + (radius * Math.Sin(angle))));
        }

        private Double SanitizeAngle(double angle)
        {
            return ((angle % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);
        }
    }
}
