using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WireFrame.Controls.Gizmo;
using WireFrame.Shapes;
using WireFrame.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class MoveResizeControl : UserControl
    {
        private Point hudTopLeft = new Point(0, 0);
        private Point hudBottomRight = new Point(0, 0);

        private Point canvasTopLeft = new Point(0, 0);
        private Point canvasBottomRight = new Point(0, 0);

        private IGizmo activeGizmo = null;

        private IGizmo[] gizmos;

        // --

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
            this.activeGizmo.StartTrackingPointer(ref this.hudTopLeft, ref this.hudBottomRight, pointer);
        }

        public void Resize(Point pointer)
        {
            this.activeGizmo.TrackPointer(ref this.hudTopLeft, ref this.hudBottomRight, pointer);
            this.activeGizmo.TrackPointer(ref this.canvasTopLeft, ref this.canvasBottomRight, pointer);
            Update();
        }

        public void StopResize(Point pointer)
        {
            this.activeGizmo.StopTrackingPointer(ref this.hudTopLeft, ref this.hudBottomRight, pointer);
            this.activeGizmo = null;
            GetSanitizedPoints(this.hudTopLeft, this.hudBottomRight, ref this.hudTopLeft, ref this.hudBottomRight);
            GetSanitizedPoints(this.canvasTopLeft, this.canvasBottomRight, ref this.canvasTopLeft, ref this.canvasBottomRight);
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectMoveResize_Pan_Focus);
        }

        ///-------------------------------------------------------------------

        public void UpdateCorners(FrameworkElement container, IShape shape, float zoomFactor, bool reset)
        {
            var transform = shape.GetPath().TransformToVisual(container);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            if (reset)
            {
                this.hudTopLeft.X = ePoint.X;
                this.hudTopLeft.Y = ePoint.Y;

                this.canvasTopLeft.X = shape.GetLeft();
                this.canvasTopLeft.Y = shape.GetTop();
            }
            else
            {
                if (ePoint.X < this.hudTopLeft.X)
                {
                    this.hudTopLeft.X = ePoint.X;
                    this.canvasTopLeft.X = shape.GetLeft();
                }

                if (ePoint.Y < this.hudTopLeft.Y)
                {
                    this.hudTopLeft.Y = ePoint.Y;
                    this.canvasTopLeft.Y = shape.GetTop();
                }
            }

            if (ePoint.X + (shape.GetLength() * zoomFactor) > this.hudBottomRight.X)
            {
                this.hudBottomRight.X = ePoint.X + (shape.GetLength() * zoomFactor);
                this.canvasBottomRight.X = shape.GetLeft() + shape.GetLength();
            }

            if (ePoint.Y + (shape.GetBreath() * zoomFactor) > this.hudBottomRight.Y)
            {
                this.hudBottomRight.Y = ePoint.Y + (shape.GetBreath() * zoomFactor);
                this.canvasBottomRight.Y = shape.GetTop() + shape.GetBreath();
            }
        }


        public void Update()
        {
            var rect = GetSanitizedRect(this.hudTopLeft, this.hudBottomRight);
            foreach (IGizmo gizmo in this.gizmos)
            {
                gizmo.Update(rect);
            }
        }

        public void ResetBounds()
        {
            this.hudTopLeft = new Point(0, 0);
            this.hudBottomRight = new Point(0, 0);

            this.canvasTopLeft = new Point(0, 0);
            this.canvasBottomRight = new Point(0, 0);

            _box.Width = 0.0;
            _box.Height = 0.0;
        }

        ///-------------------------------------------------------------------

        private void GetSanitizedPoints(Point topLeft, Point bottomRight, ref Point p1, ref Point p2)
        {
            double x1 = topLeft.X < bottomRight.X ? topLeft.X : bottomRight.X;
            double y1 = topLeft.Y < bottomRight.Y ? topLeft.Y : bottomRight.Y;
            double x2 = topLeft.X > bottomRight.X ? topLeft.X : bottomRight.X;
            double y2 = topLeft.Y > bottomRight.Y ? topLeft.Y : bottomRight.Y;

            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
        }

        private Rect GetSanitizedRect(Point topLeft, Point bottomRight)
        {
            Point p1, p2;
            GetSanitizedPoints(topLeft, bottomRight, ref p1, ref p2);

            Rect r = new Rect(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            return r;
        }

        public Rect GetCanvasRect()
        {
            Rect r = new Rect(canvasTopLeft.X, canvasTopLeft.Y, canvasBottomRight.X - canvasTopLeft.X, canvasBottomRight.Y - canvasTopLeft.Y);
            return r;
        }
    }
}
