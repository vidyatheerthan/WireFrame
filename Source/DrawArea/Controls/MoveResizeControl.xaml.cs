using deVoid.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WireFrame.DrawArea.Controls.Gizmo;
using WireFrame.DrawArea.Shapes;
using WireFrame.DrawArea.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class MoveResizeControl : UserControl, INotifyPropertyChanged, IGizmo
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

        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double ScaleX { get => (double)GetValue(ScaleXProperty); set => SetValue(ScaleXProperty, value); }

        // --

        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(MoveResizeControl), new PropertyMetadata(null));
        public double ScaleY { get => (double)GetValue(ScaleYProperty); set => SetValue(ScaleYProperty, value); }

        // --

        public static readonly DependencyProperty TransformOriginProperty = DependencyProperty.Register(nameof(TransformOrigin), typeof(Point), typeof(MoveResizeControl), new PropertyMetadata(null));
        public Point TransformOrigin { get => (Point)GetValue(TransformOriginProperty); set => SetValue(TransformOriginProperty, value); }

        ///-------------------------------------------------------------------

        private IGizmoHandler activeGizmoHandler = null;

        private IGizmoHandler[] gizmos;

        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        public event PropertyChangedEventHandler PropertyChanged;

        ///-------------------------------------------------------------------

        public MoveResizeControl()
        {
            this.InitializeComponent();

            ScaleX = ScaleY = 1.0;
            TransformOrigin = new Point(0.5, 0.5);

            // --

            var resizeGizmo = new ResizeGizmo(this, 10.0);
            resizeGizmo.AddGizmo(_top_bar, ResizeGizmo.Gizmo.Top);
            resizeGizmo.AddGizmo(_bottom_bar, ResizeGizmo.Gizmo.Bottom);
            resizeGizmo.AddGizmo(_left_bar, ResizeGizmo.Gizmo.Left);
            resizeGizmo.AddGizmo(_right_bar, ResizeGizmo.Gizmo.Right);
            resizeGizmo.AddGizmo(_top_left_circle, ResizeGizmo.Gizmo.TopLeft);
            resizeGizmo.AddGizmo(_top_right_circle, ResizeGizmo.Gizmo.TopRight);
            resizeGizmo.AddGizmo(_bottom_left_circle, ResizeGizmo.Gizmo.BottomLeft);
            resizeGizmo.AddGizmo(_bottom_right_circle, ResizeGizmo.Gizmo.BottomRight);
            resizeGizmo.AddGizmo(_top_sqr, ResizeGizmo.Gizmo.FreeTop);
            resizeGizmo.AddGizmo(_bottom_sqr, ResizeGizmo.Gizmo.FreeBottom);
            resizeGizmo.AddGizmo(_left_sqr, ResizeGizmo.Gizmo.FreeLeft);
            resizeGizmo.AddGizmo(_right_sqr, ResizeGizmo.Gizmo.FreeRight);

            var moveGizmo = new MoveGizmo(this, _move_box);

            this.gizmos = new IGizmoHandler[] { resizeGizmo, moveGizmo };

            foreach (IGizmoHandler gizmo in this.gizmos)
            {
                gizmo.OnActivate(OnGizmoActivated);
            }

            // --
        }

        ///-------------------------------------------------------------------

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
            return 0;
        }

        public void SetRotationAngle(double angle)
        {
        }

        ///-------------------------------------------------------------------

        private void OnGizmoActivated(IGizmoHandler gizmo)
        {
            this.activeGizmoHandler = gizmo;

            if (gizmo is ResizeGizmo)
            {
                Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Resize);
            }
            else if (gizmo is MoveGizmo)
            {
                Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Move);
            }
        }
        
        ///-------------------------------------------------------------------
        
        public void StartTrackingPointer(Point pointer)
        {
            this.activeGizmoHandler.StartTrackingPointer(pointer);
        }

        public void TrackPointer(Point pointer)
        {
            this.activeGizmoHandler.TrackPointer(pointer);
        }

        public void StopTrackingPointer(Point pointer)
        {
            this.activeGizmoHandler.StopTrackingPointer(pointer);
            this.activeGizmoHandler = null;
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectMoveResize_Pan_Focus);
        }

        ///-------------------------------------------------------------------

        public IShape AddShape(IShape refShape, Point position)
        {
            if(refShape == null) { return null; }

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
            SetScale(1.0, 1.0);
        }

        public void UpdateShape(IShape refShape, IShape cloneShape, Point position, float zoomFactor)
        {
            if (!this._container_canvas.Children.Contains(cloneShape.GetControl()))
            {
                return;
            }

            ShapeCloner.Update(refShape, cloneShape, position, zoomFactor);
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

        public Rect GetRect()
        {
            return new Rect(Left, Top, Length, Breath);
        }

        public Rect GetRect(Canvas canvas)
        {
            var transform = _box.TransformToVisual(canvas);
            var tl = transform.TransformPoint(new Point(0, 0));
            var br = transform.TransformPoint(new Point(_box.ActualWidth, _box.ActualHeight));

            return new Rect(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }
    }
}
