using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WireFrame.Shapes;
using WireFrame.States;
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

        private IBox box;

        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Dictionary<Gizmo, Shape> gizmos = new Dictionary<Gizmo, Shape>();

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Aqua);
        private SolidColorBrush normalBrush = new SolidColorBrush(Colors.AliceBlue);

        private Gizmo gizmoClicked;
        private Rect boxBeforeResize;
        private Point clickPoint;

        // -----------------------------------


        public CornerResizeGizmo(IBox box, double hitboxSize)
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
        }

        public void TrackPointer(Point pointer)
        {
            Point diff = new Point(pointer.X - this.clickPoint.X, pointer.Y - this.clickPoint.Y);

            switch (this.gizmoClicked)
            {
                case Gizmo.TopLeft:
                    this.box.SetLeft(this.boxBeforeResize.X + diff.X);
                    this.box.SetTop(this.boxBeforeResize.Y + diff.Y);
                    double length = this.boxBeforeResize.Width - diff.X;
                    double breath = this.boxBeforeResize.Height - diff.Y;
                    this.box.SetLength(Math.Abs(length));
                    this.box.SetBreath(Math.Abs(breath));
                    this.box.SetScale(length > 0 ? 1 : -1, breath > 0 ? 1 : -1);
                    if(length < 0)
                    {
                        this.gizmoClicked = Gizmo.TopRight;
                    }
                    if (breath < 0)
                    {
                        this.gizmoClicked = Gizmo.BottomRight;
                    }
                    break;
                case Gizmo.TopRight:
                    break;
                case Gizmo.BottomLeft:
                    break;
                case Gizmo.BottomRight:
                    break;
            }
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmos[this.gizmoClicked].Fill = this.normalBrush;
        }
    }
}
