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

        public Viewbox AddNewShape(FrameworkElement container, IShape shape)
        {
            var v = ViewboxCloner.CreateNewViewbox(shape, Utility.GetPointInContainer(shape, container), fillBrush, strokeBrush);

            _canvas.Children.Add(v);

            return v;
        }

        public void RemoveShape(Viewbox view)
        {
            this._canvas.Children.Remove(view);
        }

        public void RemoveAllShapes()
        {
            this._canvas.Children.Clear();
        }

        public void UpdateShape(FrameworkElement container, IShape shape, Viewbox childView, float zoomFactor)
        {
            if(!this._canvas.Children.Contains(childView))
            {
                return;
            }

            var path = childView.Child as Path;
            ViewboxCloner.UpdateViewbox(ref childView, shape.GetViewbox(), Utility.GetPointInContainer(shape, container));
            ViewboxCloner.UpdatePath(ref path, shape.GetViewbox(), zoomFactor);
        }        
    }
}
