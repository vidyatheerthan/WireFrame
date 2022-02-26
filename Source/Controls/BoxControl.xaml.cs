﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WireFrame.Misc;
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    public sealed partial class BoxControl : UserControl
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        // -----------------------------------------------------

        public BoxControl()
        {
            this.InitializeComponent();
        }

        public void SetBounds(Point topLeft, Point bottomRight)
        {
            Canvas.SetTop(_grid, topLeft.Y);
            Canvas.SetLeft(_grid, topLeft.X);
            _grid.Width = Math.Max(0, bottomRight.X - topLeft.X);
            _grid.Height = Math.Max(0, bottomRight.Y - topLeft.Y);
        }

        // -----------------------------------------------------

        public Viewbox AddNewView(Viewbox refView)
        {
            var v = ViewboxCloner.CreateNewViewbox(refView, fillBrush, strokeBrush);
            v.Stretch = Stretch.Fill;
            _grid.Children.Add(v);
            return v;
        }

        public void RemoveView(Viewbox view)
        {
            _grid.Children.Remove(view);
        }

        public void RemoveAllViews()
        {
            _grid.Children.Clear();
        }

        public void UpdateView(Viewbox refView, Viewbox cloneView, Point refViewPos, float zoomFactor)
        {
            if (!_grid.Children.Contains(cloneView))
            {
                return;
            }

            var path = cloneView.Child as Path;
            ViewboxCloner.UpdateViewbox(refView, ref cloneView, refViewPos);
            ViewboxCloner.UpdatePath(refView, ref path, zoomFactor);

            var transform = refView.TransformToVisual(_grid);
            var startPoint = transform.TransformPoint(new Point(0, 0));
            var endPoint = transform.TransformPoint(new Point(refView.ActualWidth, refView.ActualHeight));

            cloneView.Margin = new Thickness(startPoint.X, startPoint.Y, _grid.ActualWidth - endPoint.X, _grid.ActualHeight - endPoint.Y);
        }

        // -----------------------------------------------------
    }
}
