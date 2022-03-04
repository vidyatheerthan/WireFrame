using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Shapes;
using Point = Windows.Foundation.Point;

namespace WireFrame.Controls.Gizmo
{
    public class ResizeGizmo : IGizmo
    {
        public enum Gizmo
        {
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        double HITBOX_SIZE = 10.0;

        private MoveResizeControl moveResizeControl;

        private CoreCursor westEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private CoreCursor northSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);

        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Dictionary<Gizmo, Shape> gizmos = new Dictionary<Gizmo, Shape>();

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Aqua);
        private SolidColorBrush normalBrush = new SolidColorBrush(Colors.AliceBlue);

        private Gizmo gizmoClicked;
        private Rect boxBeforeResize;
        private Dictionary<IShape, Rect> boxContents = new Dictionary<IShape, Rect>();
        private Point clickPoint;

        // -----------------------------------


        public ResizeGizmo(MoveResizeControl moveResizeControl, double hitboxSize)
        {
            HITBOX_SIZE = hitboxSize;
            this.moveResizeControl = moveResizeControl;
        }

        public void AddGizmo(Shape shape, Gizmo gizmo) 
        {
            this.gizmos[gizmo] = shape;

            shape.PointerEntered += (object sender, PointerRoutedEventArgs e) =>
            {
                switch (gizmo)
                {
                    case Gizmo.Top:
                        Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
                        break;
                    case Gizmo.Bottom:
                        Window.Current.CoreWindow.PointerCursor = this.northSouthCursor;
                        break;
                    case Gizmo.Left:
                        Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
                        break;
                    case Gizmo.Right:
                        Window.Current.CoreWindow.PointerCursor = this.westEastCursor;
                        break;
                    case Gizmo.TopLeft:
                        Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
                        break;
                    case Gizmo.TopRight:
                        Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
                        break;
                    case Gizmo.BottomLeft:
                        Window.Current.CoreWindow.PointerCursor = this.northEastSouthWestCursor;
                        break;
                    case Gizmo.BottomRight:
                        Window.Current.CoreWindow.PointerCursor = this.northWestSouthEastCursor;
                        break;
                }
            };

            shape.PointerExited += (object sender, PointerRoutedEventArgs e) => 
            {
                Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
            };

            shape.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
            {
                this.gizmoClicked = gizmo;
                this.onActivateAction(this);
            };
        }

        // ----------------------------------------------------------

        public void OnActivate(Action<IGizmo> action)
        {
            this.onActivateAction = action;
        }

        public void StartTrackingPointer(Point point)
        {
            this.gizmos[this.gizmoClicked].Fill = this.highlightBrush;
            this.clickPoint = point;
            this.boxBeforeResize = new Rect(this.moveResizeControl.GetLeft(), this.moveResizeControl.GetTop(), this.moveResizeControl.GetLength(), this.moveResizeControl.GetBreath());
            this.boxContents.Clear();
            foreach(IShape shape in this.moveResizeControl.GetShapes())
            {
                this.boxContents.Add(shape, new Rect(shape.GetLeft(), shape.GetTop(), shape.GetLength(), shape.GetBreath()));
            }
        }

        public void TrackPointer(Point point)
        {
            double left = 0.0;
            double top = 0.0;
            double length = 0.0;
            double breath = 0.0;

            double scaleX = 0.0;
            double scaleY = 0.0;

            this.moveResizeControl.GetScale(ref scaleX, ref scaleY);

            Point diff = new Point(point.X - this.clickPoint.X, point.Y - this.clickPoint.Y);

            // move
            //if (this.gizmoClicked == Gizmo.Move)
            //{
            //    left = this.boxBeforeResize.X + diff.X;
            //    top = scaleY > 0 ? this.boxBeforeResize.Y + diff.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height;
            //    length = this.boxBeforeResize.Width;
            //    breath = this.boxBeforeResize.Height;
            //}

            if (this.gizmoClicked == Gizmo.Top)
            {
                left = this.boxBeforeResize.X;
                top = scaleY > 0 ? this.boxBeforeResize.Y + diff.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height;
                length = this.boxBeforeResize.Width;
                breath = this.boxBeforeResize.Height - diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.Bottom)
            {
                left = this.boxBeforeResize.X;
                top = scaleY > 0 ? this.boxBeforeResize.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height + diff.Y;
                length = this.boxBeforeResize.Width;
                breath = this.boxBeforeResize.Height + diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.Left)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X + diff.X : this.boxBeforeResize.X + this.boxBeforeResize.Width;
                top = this.boxBeforeResize.Y;
                length = this.boxBeforeResize.Width - diff.X;
                breath = this.boxBeforeResize.Height;
            }
            else if (this.gizmoClicked == Gizmo.Right)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X : this.boxBeforeResize.X + this.boxBeforeResize.Width + diff.X;
                top = this.boxBeforeResize.Y;
                length = this.boxBeforeResize.Width + diff.X;
                breath = this.boxBeforeResize.Height;
            }
            // --
            else if (this.gizmoClicked == Gizmo.TopLeft) 
            {
                left = scaleX > 0 ? this.boxBeforeResize.X + diff.X : this.boxBeforeResize.X + this.boxBeforeResize.Width;
                top = scaleY > 0 ? this.boxBeforeResize.Y + diff.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height;
                length = this.boxBeforeResize.Width - diff.X;
                breath = this.boxBeforeResize.Height - diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.TopRight)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X : this.boxBeforeResize.X + this.boxBeforeResize.Width + diff.X;
                top = scaleY > 0 ? this.boxBeforeResize.Y + diff.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height;
                length = this.boxBeforeResize.Width + diff.X;
                breath = this.boxBeforeResize.Height - diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.BottomLeft)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X + diff.X : this.boxBeforeResize.X + this.boxBeforeResize.Width;
                top = scaleY > 0 ? this.boxBeforeResize.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height + diff.Y;
                length = this.boxBeforeResize.Width - diff.X;
                breath = this.boxBeforeResize.Height + diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.BottomRight)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X : this.boxBeforeResize.X + this.boxBeforeResize.Width + diff.X;
                top = scaleY > 0 ? this.boxBeforeResize.Y : this.boxBeforeResize.Y + this.boxBeforeResize.Height + diff.Y;
                length = this.boxBeforeResize.Width + diff.X;
                breath = this.boxBeforeResize.Height + diff.Y;
            }

            this.moveResizeControl.SetLeft(left);
            this.moveResizeControl.SetLength(Math.Abs(length));
            this.moveResizeControl.SetTop(top);
            this.moveResizeControl.SetBreath(Math.Abs(breath));

            this.moveResizeControl.SetScale(length > 0 ? 1 : -1, breath > 0 ? 1 : -1);

            UpdateContainerItemSizes();
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmos[this.gizmoClicked].Fill = this.normalBrush;
        }

        private void UpdateContainerItemSizes()
        {
            double xRatio = this.moveResizeControl.GetLength() / this.boxBeforeResize.Width;
            double yRatio = this.moveResizeControl.GetBreath() / this.boxBeforeResize.Height;

            foreach (var shapeRect in this.boxContents)
            {
                IShape shape = shapeRect.Key;
                Rect rect = shapeRect.Value;

                shape.SetLeft(rect.X * xRatio);
                shape.SetTop(rect.Y * yRatio);
                shape.SetLength(rect.Width * xRatio);
                shape.SetBreath(rect.Height * yRatio);
            }
        }
    }
}
