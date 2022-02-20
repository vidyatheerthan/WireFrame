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

        private CoreCursor northEastSouthWestCursor = new CoreCursor(CoreCursorType.SizeNortheastSouthwest, 1);
        private CoreCursor northWestSouthEastCursor = new CoreCursor(CoreCursorType.SizeNorthwestSoutheast, 1);

        private CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        private Shape gizmoElement = null;

        private Gizmo gizmo;

        private Action<IGizmo> onActivateAction;

        private SolidColorBrush highlightBrush, normalBrush;

        // -----------------------------------


        public CornerResizeGizmo(double hitboxSize, Shape gizmoElement, Gizmo gizmo)
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
            switch(this.gizmo)
            {
                case Gizmo.TopLeft:
                    UpdateTopLeft(rect);
                    break;
                case Gizmo.TopRight:
                    UpdateTopRight(rect);
                    break;
                case Gizmo.BottomLeft:
                    UpdateBottomLeft(rect);
                    break;
                case Gizmo.BottomRight:
                    UpdateBottomRight(rect);
                    break;
            }
        }

        private void UpdateTopLeft(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateTopRight(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + rect.Width - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateBottomLeft(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y + rect.Height - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        private void UpdateBottomRight(Rect rect)
        {
            double HALF_HIT = HITBOX_SIZE * 0.5;

            Canvas.SetLeft(this.gizmoElement, rect.X + rect.Width - HALF_HIT);
            Canvas.SetTop(this.gizmoElement, rect.Y + rect.Height - HALF_HIT);
            this.gizmoElement.Width = HITBOX_SIZE;
            this.gizmoElement.Height = HITBOX_SIZE;
        }

        // ----------------------------------------------------------

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            switch(this.gizmo)
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

        public void TrackPointer(ref Point topLeft, ref Point bottomRight, Point pointer)
        {
            switch (this.gizmo)
            {
                case Gizmo.TopLeft:
                    topLeft = pointer;
                    break;
                case Gizmo.TopRight:
                    topLeft.Y = pointer.Y;
                    bottomRight.X = pointer.X;
                    break;
                case Gizmo.BottomLeft:
                    topLeft.X = pointer.X;
                    bottomRight.Y = pointer.Y;
                    break;
                case Gizmo.BottomRight:
                    bottomRight = pointer;
                    break;
            }
        }

        public void StopTrackingPointer(Point point)
        {
            this.gizmoElement.Fill = this.normalBrush;
        }
    }
}
