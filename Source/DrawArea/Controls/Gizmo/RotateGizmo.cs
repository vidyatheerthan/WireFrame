using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    class RotateGizmo : IGizmoHandler
    {
        private RotationControl rotationControl;
        private Action<IGizmoHandler> onActivateAction;
        private Panel gizmoElement;

        // --

        public RotateGizmo(RotationControl rotationControl, Panel gizmoElement)
        {
            this.rotationControl = rotationControl;
            this.gizmoElement = gizmoElement;

            this.gizmoElement.PointerPressed += (object sender, PointerRoutedEventArgs e) => {
                this.onActivateAction(this);
            };
        }

        // --

        public void OnActivate(Action<IGizmoHandler> action)
        {
            this.onActivateAction = action;
        }

        public void StartTrackingPointer(Point pointer)
        {
            this.rotationControl.EnableArc(true);
        }

        public void TrackPointer(Point pointer)
        {
            double startAngle = Math.Atan2(pointer.Y - this.rotationControl.Axis.Y, pointer.X - this.rotationControl.Axis.X);
            double endAngle = 2 * Math.PI - 0.0001;

            DrawArc(startAngle, endAngle);
            this.rotationControl.SetRotationAngle(startAngle * 180.0 / Math.PI);
        }

        public void StopTrackingPointer(Point pointer)
        {
            this.rotationControl.EnableArc(false);
        }

        // --

        private void DrawArc(double start_angle, double end_angle)
        {
            Point center = this.rotationControl.Axis;
            Size radius = this.rotationControl.ArcRadius;

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
            Point endPoint = PolarToCartesian(end_angle, radius.Height, center);
            Size arcSize = radius;
            SweepDirection dir = SweepDirection.Clockwise;

            Point startPoint = PolarToCartesian(start_angle, radius.Width, center);

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
