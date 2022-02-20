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
    public class FixedSideResizeGizmo : IGizmo
    {
        public enum Gizmo
        {
            Top,
            Bottom,
            Left,
            Right
        }

        double HITBOX_SIZE = 10.0;

        private CoreCursor westEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private CoreCursor northSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Shape gizmoElement = null;

        private Gizmo gizmo;

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush, normalBrush;

        // -----------------------------------


        public FixedSideResizeGizmo(double hitboxSize, Shape gizmoElement, Gizmo gizmo)
        {
            HITBOX_SIZE = hitboxSize;

            this.gizmoElement = gizmoElement;

            this.gizmo = gizmo;

            this.gizmoElement.PointerEntered += OnPointerEntered;
            this.gizmoElement.PointerExited += OnPointerExited;
            this.gizmoElement.PointerPressed += OnPointerPressed;

            this.highlightBrush = new SolidColorBrush(Colors.Aqua);
            this.normalBrush = new SolidColorBrush(Colors.Transparent);
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

            Canvas.SetLeft(this.gizmoElement, rect.X - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = rect.Height;
        }

        private void UpdateRight(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + rect.Width - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = rect.Height;
        }

        private void UpdateTop(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X);
            Canvas.SetTop(this.gizmoElement, rect.Y - HALF_HIT);
            this.gizmoElement.Width = rect.Width;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateBottom(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X);
            Canvas.SetTop(this.gizmoElement, rect.Y + rect.Height - HALF_HIT);
            this.gizmoElement.Width = rect.Width;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        // ----------------------------------------------------------

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            switch (this.gizmo)
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

        public void TrackPointer(Point point)
        {
            
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmoElement.Fill = this.normalBrush;
        }
    }
}
