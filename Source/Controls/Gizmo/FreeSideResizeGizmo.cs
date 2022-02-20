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

        private Point diff;

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

        public void Update(Rect rect)
        {
            switch (this.gizmo)
            {
                case Gizmo.Top:
                    UpdateTop(rect);
                    break;
                case Gizmo.Bottom:
                    UpdateBottom(rect);
                    break;
                case Gizmo.Left:
                    UpdateLeft(rect);
                    break;
                case Gizmo.Right:
                    UpdateRight(rect);
                    break;
            }
        }

        private void UpdateLeft(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;
            double HALF_HEIGHT = rect.Height * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y + HALF_HEIGHT - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateRight(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;
            double HALF_HEIGHT = rect.Height * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + rect.Width - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y + HALF_HEIGHT - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateTop(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;
            double HALF_WIDTH = rect.Width * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + HALF_WIDTH - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateBottom(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;
            double HALF_WIDTH = rect.Width * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + HALF_WIDTH - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y + rect.Height - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
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

        public void StartTrackingPointer(ref Point topLeft, ref Point bottomRight, Point point)
        {
            this.gizmoElement.Fill = this.highlightBrush;

            this.diff.X = bottomRight.X - topLeft.X;
            this.diff.Y = bottomRight.Y - topLeft.Y;
        }

        public void TrackPointer(ref Point topLeft, ref Point bottomRight, Point pointer)
        {
            switch (this.gizmo)
            {
                case Gizmo.Left:
                    if(pointer.X - topLeft.X > 0)
                    {
                        double dec = ((pointer.X - topLeft.X) / this.diff.X) * this.diff.Y * 0.5;
                        topLeft.Y += dec;
                        bottomRight.Y -= dec;
                    }
                    topLeft.X = pointer.X;
                    break;
                case Gizmo.Right:
                    bottomRight.X = pointer.X;
                    break;
                case Gizmo.Top:
                    topLeft.Y = pointer.Y;
                    break;
                case Gizmo.Bottom:
                    bottomRight.Y = pointer.Y;
                    break;
            }
        }

        public void StopTrackingPointer(ref Point topLeft, ref Point bottomRight, Point point)
        {
            this.gizmoElement.Fill = this.normalBrush;
        }
    }
}
