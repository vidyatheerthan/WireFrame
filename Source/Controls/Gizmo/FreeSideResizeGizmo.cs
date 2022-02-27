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
using WireFrame.States;
using Point = Windows.Foundation.Point;

namespace WireFrame.Controls.Gizmo
{
    public class FreeSideResizeGizmo : IGizmo
    {
        public enum Gizmo
        {
            Top,
            Bottom,
            Left,
            Right
        }

        double HITBOX_SIZE = 10.0;

        private CoreCursor upArrowCursor = new CoreCursor(CoreCursorType.UpArrow, 1);
        private CoreCursor downArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private CoreCursor leftArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private CoreCursor rightArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Shape gizmoElement = null;

        private Gizmo gizmo;

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush, normalBrush;

        // -----------------------------------


        public FreeSideResizeGizmo(double hitboxSize, Shape gizmoElement, Gizmo gizmo)
        {
            HITBOX_SIZE = hitboxSize;

            this.gizmoElement = gizmoElement;

            this.gizmo = gizmo;

            this.gizmoElement.PointerEntered += OnPointerEntered;
            this.gizmoElement.PointerExited += OnPointerExited;
            this.gizmoElement.PointerPressed += OnPointerPressed;

            this.highlightBrush = new SolidColorBrush(Colors.Aqua);
            this.normalBrush = new SolidColorBrush(Colors.AliceBlue);
        }

        // ----------------------------------------------------------

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            switch (this.gizmo)
            {
                case Gizmo.Top:
                    Window.Current.CoreWindow.PointerCursor = this.upArrowCursor;
                    break;
                case Gizmo.Bottom:
                    Window.Current.CoreWindow.PointerCursor = this.downArrowCursor;
                    break;
                case Gizmo.Left:
                    Window.Current.CoreWindow.PointerCursor = this.leftArrowCursor;
                    break;
                case Gizmo.Right:
                    Window.Current.CoreWindow.PointerCursor = this.rightArrowCursor;
                    break;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = this.arrowCursor;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.onActivateAction(this);
        }

        // ----------------------------------------------------------

        public void OnActivate(Action<IGizmo> action)
        {
            this.onActivateAction = action;
        }

        public void StartTrackingPointer(Point point)
        {
            this.gizmoElement.Fill = this.highlightBrush;
        }

        public void TrackPointer(Point pointer)
        {
            //double diffX = bottomRight.X - topLeft.X;
            //double diffY = bottomRight.Y - topLeft.Y;

            //double tlx = ((pointer.Y - topLeft.Y) / diffY) * diffX * 0.5;
            //double tly = ((pointer.X - topLeft.X) / diffX) * diffY * 0.5;

            //double brx = ((pointer.Y - bottomRight.Y) / diffY) * diffX * 0.5;
            //double bry = ((pointer.X - bottomRight.X) / diffX) * diffY * 0.5;

            //switch (this.gizmo)
            //{
            //    case Gizmo.Left:
            //        topLeft.X = pointer.X;
            //        topLeft.Y += tly;
            //        bottomRight.Y -= tly;
            //        break;
            //    case Gizmo.Right:
            //        bottomRight.X = pointer.X;
            //        bottomRight.Y += bry;
            //        topLeft.Y -= bry;
            //        break;
            //    case Gizmo.Top:
            //        topLeft.Y = pointer.Y;
            //        topLeft.X += tlx;
            //        bottomRight.X -= tlx;
            //        break;
            //    case Gizmo.Bottom:
            //        bottomRight.Y = pointer.Y;
            //        bottomRight.X += brx;
            //        topLeft.X -= brx;
            //        break;
            //}
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmoElement.Fill = this.normalBrush;
        }
    }
}
