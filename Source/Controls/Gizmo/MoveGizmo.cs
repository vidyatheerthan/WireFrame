﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Controls.Gizmo
{
    class MoveGizmo : IGizmo
    {
        private FrameworkElement gizmoElement;

        private Action<IGizmo> onActivateAction;

        // ------------------------------

        public MoveGizmo(FrameworkElement gizmoElement)
        {
            this.gizmoElement = gizmoElement;

            this.gizmoElement.PointerPressed += OnPointerPressed;
        }

        // ------------------------------

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.onActivateAction(this);
        }

        // ------------------------------

        public void Update(Rect rect)
        {
            Canvas.SetLeft(gizmoElement, rect.X);
            Canvas.SetTop(gizmoElement, rect.Y);
            gizmoElement.Width = rect.Width;
            gizmoElement.Height = rect.Height;
        }

        // ------------------------------

        public void OnActivate(Action<IGizmo> action)
        {
            this.onActivateAction = action;
        }

        public void StartTrackingPointer(ref Point topLeft, ref Point bottomRight, Point point)
        {
            throw new NotImplementedException();
        }

        public void TrackPointer(ref Point topLeft, ref Point bottomRight, Point pointer)
        {
            throw new NotImplementedException();
        }

        public void StopTrackingPointer(ref Point topLeft, ref Point bottomRight, Point point)
        {
            throw new NotImplementedException();
        }

        // ------------------------------
    }
}