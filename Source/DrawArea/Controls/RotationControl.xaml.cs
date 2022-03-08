using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
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
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        // --

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        // --

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        // --

        public static readonly DependencyProperty BreathProperty = DependencyProperty.Register(nameof(Breath), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double Breath { get => (double)GetValue(BreathProperty); set => SetValue(BreathProperty, value); }

        // --

        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(nameof(RotationAngle), typeof(double), typeof(EllipseShape), new PropertyMetadata(null));
        public double RotationAngle { get => (double)GetValue(RotationAngleProperty); set => SetValue(RotationAngleProperty, value); }

        // --

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

        private IGizmoHandler rotateGizmo;
        public event PropertyChangedEventHandler PropertyChanged;

        // --

        public RotationControl()
        {
            this.InitializeComponent();

            this.DataContext = this; // important: set this to receive change to DependencyProperty from other classes

            PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
                if(e.PropertyName == nameof(Axis))
                {
                    Canvas.SetLeft(_gizmo_grid, Axis.X - _gizmo_grid.Width * 0.5);
                    Canvas.SetTop(_gizmo_grid, Axis.Y - _gizmo_grid.Height * 0.5);

                    _gizmo_line_1.Point = Axis;
                }
            };

            this.rotateGizmo = new RotateGizmo(this, _rotate_box);
            this.rotateGizmo.OnActivate(OnGizmoActivated);
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

        public void GetScale(ref double x, ref double y)
        {
        }

        public void SetScale(double x, double y)
        {
        }

        public double GetRotation()
        {
            return this.RotationAngle;
        }

        public void SetRotation(double rotation)
        {
            this.RotationAngle = rotation;
        }

        ///-------------------------------------------------------------------

        public void StartTrackingPointer(Point pointer)
        {
            this.rotateGizmo.StartTrackingPointer(pointer);
        }

        public void TrackPointer(Point pointer)
        {
            this.rotateGizmo.TrackPointer(pointer);
        }

        public void StopTrackingPointer(Point pointer)
        {
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

            Axis = new Point(Left + Length * 0.5, Top + Breath * 0.5);
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
            _gizmo_line_2.Point = startPoint;

            _gizmo_pathFigure.StartPoint = startPoint;

            _gizmo_arcSegment.IsLargeArc = isLargeArc;
            _gizmo_arcSegment.Point = endPoint;
            _gizmo_arcSegment.Size = arcSize;
            _gizmo_arcSegment.SweepDirection = dir;
        }

    }
}
