using deVoid.Utils;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Controls.Gizmo;
using WireFrame.Misc;
using WireFrame.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class MoveResizeControl : UserControl, INotifyPropertyChanged
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

        ///-------------------------------------------------------------------

        private IGizmo activeGizmo = null;

        private IGizmo[] gizmos;

        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        public event PropertyChangedEventHandler PropertyChanged;

        ///-------------------------------------------------------------------

        public MoveResizeControl()
        {
            this.InitializeComponent();



            // --

            this.gizmos = new IGizmo[]
            {
                // corners
                new CornerResizeGizmo(10.0, _top_left_circle, CornerResizeGizmo.Gizmo.TopLeft),
                new CornerResizeGizmo(10.0, _top_right_circle, CornerResizeGizmo.Gizmo.TopRight),
                new CornerResizeGizmo(10.0, _bottom_left_circle, CornerResizeGizmo.Gizmo.BottomLeft),
                new CornerResizeGizmo(10.0, _bottom_right_circle, CornerResizeGizmo.Gizmo.BottomRight),
                // fixed sided
                new FixedSideResizeGizmo(10.0, _top_bar, FixedSideResizeGizmo.Gizmo.Top),
                new FixedSideResizeGizmo(10.0, _bottom_bar, FixedSideResizeGizmo.Gizmo.Bottom),
                new FixedSideResizeGizmo(10.0, _left_bar, FixedSideResizeGizmo.Gizmo.Left),
                new FixedSideResizeGizmo(10.0, _right_bar, FixedSideResizeGizmo.Gizmo.Right),
                // free sided
                new FreeSideResizeGizmo(10.0, _top_sqr, FreeSideResizeGizmo.Gizmo.Top),
                new FreeSideResizeGizmo(10.0, _bottom_sqr, FreeSideResizeGizmo.Gizmo.Bottom),
                new FreeSideResizeGizmo(10.0, _left_sqr, FreeSideResizeGizmo.Gizmo.Left),
                new FreeSideResizeGizmo(10.0, _right_sqr, FreeSideResizeGizmo.Gizmo.Right),
                // box
                new MoveGizmo(_box),
            };

            foreach (IGizmo gizmo in this.gizmos)
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

        ///-------------------------------------------------------------------

        private void OnGizmoActivated(IGizmo gizmo)
        {
            this.activeGizmo = gizmo;

            if (gizmo is CornerResizeGizmo || gizmo is FixedSideResizeGizmo || gizmo is FreeSideResizeGizmo)
            {
                Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Resize);
            }
        }
        
        ///-------------------------------------------------------------------
        
        public void StartResize(Point pointer)
        {
            this.activeGizmo.StartTrackingPointer(_box, pointer);
        }

        public void Resize(Point pointer)
        {
            this.activeGizmo.TrackPointer(_box, pointer);
            Update();
        }

        public void StopResize(Point pointer)
        {
            this.activeGizmo.StopTrackingPointer(_box, pointer);
            this.activeGizmo = null;
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectMoveResize_Pan_Focus);
        }

        ///-------------------------------------------------------------------


        public void Update()
        {
            var rect = GetRect();

            foreach (IGizmo gizmo in this.gizmos)
            {
                gizmo.Update(rect);
            }
        }

        ///-------------------------------------------------------------------

        public Viewbox AddView(Viewbox refView, Point position)
        {
            var view = ViewboxCloner.CreateNewViewbox(refView, fillBrush, strokeBrush);
            ViewboxCloner.UpdateViewbox(refView, ref view, position);
            _canvas.Children.Add(view);
            return view;
        }

        public void RemoveView(Viewbox viewbox)
        {
            _canvas.Children.Remove(viewbox);
        }

        public void RemoveAllViews()
        {
            _canvas.Children.Clear();
        }

        public void UpdateView(Viewbox refView, Viewbox cloneView, Point position, float zoomFactor)
        {
            if (!this._canvas.Children.Contains(cloneView))
            {
                return;
            }

            var path = cloneView.Child as Path;
            ViewboxCloner.UpdateViewbox(refView, ref cloneView, position);
            ViewboxCloner.UpdatePath(refView, ref path, zoomFactor);
        }

        ///-------------------------------------------------------------------

        public Rect GetRect()
        {
            return new Rect(Left, Top, Length, Breath);
        }
    }
}
