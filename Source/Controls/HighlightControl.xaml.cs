using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
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
            var cloneView = ShapeCloner.CloneViewbox(refView);
            ShapeCloner.UpdateViewbox(refView, ref cloneView, position, 1.0f);
            var clonePath = cloneView.Child as Path;
            clonePath.Fill = fillBrush;
            clonePath.Stroke = strokeBrush;
            _canvas.Children.Add(cloneView);
            return cloneView;
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

            ShapeCloner.UpdateViewbox(refView, ref cloneView, position, zoomFactor);
        }
    }
}
