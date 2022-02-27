using System;
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
    public sealed partial class HighlightControl : UserControl
    {
        private SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
        private SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

        // -----------------------------------------------------

        public HighlightControl()
        {
            this.InitializeComponent();
        }

        public Viewbox AddView(Viewbox refView, Point position)
        {
            var view = ViewboxCloner.CreateNewViewbox(refView, fillBrush, strokeBrush);
            ViewboxCloner.UpdateViewbox(refView, ref view, position);
            _canvas.Children.Add(view);
            return view;
        }

        public void RemoveView(Viewbox viewbox)
        {
            _canvas.Children.Remove(viewbox);
        }

        public void RemoveAllViews()
        {
            _canvas.Children.Clear();
        }

        public void UpdateView(Viewbox refView, Viewbox cloneView, Point position, float zoomFactor)
        {
            if (!this._canvas.Children.Contains(cloneView))
            {
                return;
            }

            var path = cloneView.Child as Path;
            ViewboxCloner.UpdateViewbox(refView, ref cloneView, position);
            ViewboxCloner.UpdatePath(refView, ref path, zoomFactor);
        }
    }
}
