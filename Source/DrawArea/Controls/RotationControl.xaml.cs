using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.DrawArea.Controls.Gizmo;
using WireFrame.DrawArea.Shapes;
using WireFrame.DrawArea.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class RotationControl : UserControl, INotifyPropertyChanged, IGizmo
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        // --

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        // --

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        // --

        public static readonly DependencyProperty BreathProperty = DependencyProperty.Register(nameof(Breath), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double Breath { get => (double)GetValue(BreathProperty); set => SetValue(BreathProperty, value); }

        // --

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double CenterX { get => (double)GetValue(CenterXProperty); set => SetValue(CenterXProperty, value); }

        // --

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double CenterY { get => (double)GetValue(CenterYProperty); set => SetValue(CenterYProperty, value); }

        // --

        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(nameof(RotationAngle), typeof(double), typeof(RotationControl), new PropertyMetadata(null));
        public double RotationAngle { get => (double)GetValue(RotationAngleProperty); set => SetValue(RotationAngleProperty, value); }

        // --

        public static readonly DependencyProperty TransformOriginProperty = DependencyProperty.Register(nameof(TransformOrigin), typeof(Point), typeof(RotationControl), new PropertyMetadata(null));
        public Point TransformOrigin { get => (Point)GetValue(TransformOriginProperty); set => SetValue(TransformOriginProperty, value); }

        // --

        public Size ArcRadius { get => _gizmo_arcSegment.Size; }

        // --

        public static readonly DependencyProperty GizmoCenterProperty = DependencyProperty.Register(nameof(GizmoCenter), typeof(Point), typeof(RotationControl), new PropertyMetadata(null));

        public Point GizmoCenter
        { 
            get => (Point)GetValue(GizmoCenterProperty);
            set => SetValue(GizmoCenterProperty, value);
        }

        // --

        private IGizmoHandler rotateGizmo;
        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public RotationControl()
        {
            this.InitializeComponent();

            TransformOrigin = new Point(0.5, 0.5);

            GizmoCenter = new Point(0, 0);

            this.DataContext = this; // important: set this to receive change to DependencyProperty from other classes

            this.rotateGizmo = new RotateGizmo(this, _rotate_box);
            this.rotateGizmo.OnActivate(OnGizmoActivated);

            EnableArc(false);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnGizmoActivated(IGizmoHandler gizmo)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Rotate);
        }

        ///-------------------------------------------------------------------

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

        public void GetCenter(ref double x, ref double y)
        {
            x = CenterX;
            y = CenterY;
        }

        public void SetCenter(double x, double y)
        {
            CenterX = x;
            CenterY = y;
        }

        public void GetScale(ref double x, ref double y)
        {
        }

        public void SetScale(double x, double y)
        {
        }

        public void SetTransformOrigin(Point point)
        {
            TransformOrigin = point;
        }

        public Point GetTransformOrigin()
        {
            return TransformOrigin;
        }

        public double GetRotationAngle()
        {
            return this.RotationAngle;
        }

        public void SetRotationAngle(double angle)
        {
            this.RotationAngle = angle;
        }

        ///-------------------------------------------------------------------

        public void StartTrackingPointer(Point pointer)
        {
            _rotate_box.BorderBrush = new SolidColorBrush(Colors.Aqua);
            this.rotateGizmo.StartTrackingPointer(pointer);
        }

        public void TrackPointer(Point pointer)
        {
            this.rotateGizmo.TrackPointer(pointer);
        }

        public void StopTrackingPointer(Point pointer)
        {
            _rotate_box.BorderBrush = new SolidColorBrush(Colors.Blue);
            this.rotateGizmo.StopTrackingPointer(pointer);
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectRotate_Pan_Focus);
        }

        ///-------------------------------------------------------------------

        public IShape AddShape(IShape refShape, Point position)
        {
            if (refShape == null) { return null; }

            var cloneShape = ShapeCloner.Clone(refShape);
            ShapeCloner.Update(refShape, cloneShape, position, 1.0f);

            cloneShape.SetOpacity(0.5);
            cloneShape.SetFill(refShape.GetFill());
            cloneShape.SetStroke(refShape.GetStroke());

            _container_canvas.Children.Add(cloneShape.GetControl());

            return cloneShape;
        }

        public void RemoveShape(IShape cloneShape)
        {
            _container_canvas.Children.Remove(cloneShape.GetControl());
        }

        public void RemoveShapes()
        {
            _container_canvas.Children.Clear();
        }

        public void UpdateShape(IShape refShape, IShape cloneShape, Point position, float zoomFactor)
        {
            if (!this._container_canvas.Children.Contains(cloneShape.GetControl()))
            {
                return;
            }

            ShapeCloner.Update(refShape, cloneShape, position, zoomFactor);

            GizmoCenter = new Point(Left + Length * 0.5, Top + Breath * 0.5);
            var tp = _rotate_box.TransformToVisual(null).Inverse.TransformPoint(GizmoCenter);
            SetCenter(tp.X, tp.Y);
        }

        public List<IShape> GetShapes()
        {
            return _container_canvas.Children.Where(item => item is IShape).Cast<IShape>().ToList();
        }

        public void Activate(bool activate)
        {
            _canvas.Visibility = activate ? Visibility.Visible : Visibility.Collapsed;
        }

        ///-------------------------------------------------------------------

        public void SetArc(bool isLargeArc, Point startPoint, Point endPoint, Size arcSize, SweepDirection dir)
        {
            _gizmo_line_1.Point = GizmoCenter;
            _gizmo_line_2.Point = startPoint;

            _gizmo_pathFigure.StartPoint = startPoint;

            _gizmo_arcSegment.IsLargeArc = isLargeArc;
            _gizmo_arcSegment.Point = endPoint;
            _gizmo_arcSegment.Size = arcSize;
            _gizmo_arcSegment.SweepDirection = dir;
        }

        public void EnableArc(bool enable)
        {
            this._gizmo_path.Visibility = enable ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
