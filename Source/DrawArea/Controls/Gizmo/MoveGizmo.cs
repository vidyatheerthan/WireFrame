using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    class MoveGizmo : IGizmoHandler
    {
        private MoveResizeControl moveResizeControl;
        private Panel gizmoElement;

        private Action<IGizmoHandler> onActivateAction;

        private Rect boxBeforeResize;
        private Dictionary<IShape, Rect> boxContents = new Dictionary<IShape, Rect>();
        private Point clickPoint;

        // ------------------------------

        public MoveGizmo(MoveResizeControl moveResizeControl, Panel gizmoElement)
        {
            this.moveResizeControl = moveResizeControl;
            this.gizmoElement = gizmoElement;

            this.gizmoElement.PointerPressed += (object sender, PointerRoutedEventArgs e) => {
                this.onActivateAction(this);
            };
        }

        // ------------------------------

        public void OnActivate(Action<IGizmoHandler> action)
        {
            this.onActivateAction = action;
        }

        public void StartTrackingPointer(Point point)
        {
            this.clickPoint = point;
            this.boxBeforeResize = new Rect(this.moveResizeControl.GetLeft(), this.moveResizeControl.GetTop(), this.moveResizeControl.GetLength(), this.moveResizeControl.GetBreath());
            this.boxContents.Clear();
            foreach (IShape shape in this.moveResizeControl.GetShapes())
            {
                this.boxContents.Add(shape, new Rect(shape.GetLeft(), shape.GetTop(), shape.GetLength(), shape.GetBreath()));
            }
        }

        public void TrackPointer(Point point)
        {
            double scaleX = 0.0;
            double scaleY = 0.0;

            this.moveResizeControl.GetScale(ref scaleX, ref scaleY);

            Point diff = new Point(point.X - this.clickPoint.X, point.Y - this.clickPoint.Y);

            double left = this.boxBeforeResize.X + diff.X;
            double top = scaleY > 0 ? this.boxBeforeResize.Y + diff.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height;
            double length = this.boxBeforeResize.Width;
            double breath = this.boxBeforeResize.Height;

            this.moveResizeControl.SetLeft(left);
            this.moveResizeControl.SetLength(Math.Abs(length));
            this.moveResizeControl.SetTop(top);
            this.moveResizeControl.SetBreath(Math.Abs(breath));

            this.moveResizeControl.SetScale(length > 0 ? 1 : -1, breath > 0 ? 1 : -1);
        }

        public void StopTrackingPointer(Point point)
        {
            this.boxBeforeResize = Rect.Empty;
            this.boxContents.Clear();
        }

        // ------------------------------
    }
}
