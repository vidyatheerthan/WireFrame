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
    public class CornerResizeGizmo : IGizmo
    {
        public enum Gizmo
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        double HITBOX_SIZE = 10.0;

        private IContainer box;

        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Dictionary<Gizmo, Shape> gizmos = new Dictionary<Gizmo, Shape>();

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Aqua);
        private SolidColorBrush normalBrush = new SolidColorBrush(Colors.AliceBlue);

        private Gizmo gizmoClicked;
        private Rect boxBeforeResize;
        private Dictionary<Viewbox, Rect> boxContents = new Dictionary<Viewbox, Rect>();
        private Point clickPoint;

        // -----------------------------------


        public CornerResizeGizmo(IContainer box, double hitboxSize)
        {
            HITBOX_SIZE = hitboxSize;
            this.box = box;
        }

        public void AddGizmo(Shape shape, Gizmo gizmo) 
        {
            this.gizmos[gizmo] = shape;

            shape.PointerEntered += (object sender, PointerRoutedEventArgs e) =>
            {
                switch (gizmo)
                {
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
            this.boxBeforeResize = new Rect(this.box.GetLeft(), this.box.GetTop(), this.box.GetLength(), this.box.GetBreath());
            this.boxContents.Clear();
            foreach(var viewbox in this.box.GetContents())
            {
                this.boxContents.Add(viewbox, new Rect(Canvas.GetLeft(viewbox), Canvas.GetTop(viewbox), viewbox.ActualWidth, viewbox.ActualHeight));
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

            this.box.GetScale(ref scaleX, ref scaleY);

            Point diff = new Point(point.X - this.clickPoint.X, point.Y - this.clickPoint.Y);

            if (this.gizmoClicked == Gizmo.TopLeft) 
            {
                left = scaleX > 0 ? this.boxBeforeResize.X + diff.X : this.box.GetLeft();
                top = this.boxBeforeResize.Y + diff.Y;
                length = this.boxBeforeResize.Width - diff.X;
                breath = this.boxBeforeResize.Height - diff.Y;
            }
            else if (this.gizmoClicked == Gizmo.TopRight)
            {
                left = scaleX > 0 ? this.boxBeforeResize.X : this.boxBeforeResize.X + this.boxBeforeResize.Width + diff.X;
                top = this.boxBeforeResize.Y + diff.Y;
                length = this.boxBeforeResize.Width + diff.X;
                breath = this.boxBeforeResize.Height - diff.Y;
            }

            this.box.SetLeft(left);
            this.box.SetLength(Math.Abs(length));
            this.box.SetTop(top);
            this.box.SetBreath(Math.Abs(breath));

            this.box.SetScale(length > 0 ? 1 : -1, breath > 0 ? 1 : -1);

            UpdateContainerItemSizes();
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmos[this.gizmoClicked].Fill = this.normalBrush;
        }

        private void UpdateContainerItemSizes()
        {
            double xRatio = this.box.GetLength() / this.boxBeforeResize.Width;
            double yRatio = this.box.GetBreath() / this.boxBeforeResize.Height;

            foreach (var viewboxSize in this.boxContents)
            {
                double left = viewboxSize.Value.X;
                Canvas.SetLeft(viewboxSize.Key, left * xRatio);

                double top = viewboxSize.Value.Y;
                Canvas.SetTop(viewboxSize.Key, top * yRatio);

                double width = viewboxSize.Value.Width;
                viewboxSize.Key.Width = width* xRatio;

                double height = viewboxSize.Value.Height;
                viewboxSize.Key.Height = height * yRatio;
            }
        }
    }
}
