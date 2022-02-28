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
            var view = ShapeCloner.CloneViewbox(refView, fillBrush, strokeBrush);
            ShapeCloner.UpdateViewbox(refView, ref view, position);
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
            ShapeCloner.UpdateViewbox(refView, ref cloneView, position);
            ShapeCloner.UpdatePath(refView, ref path, zoomFactor);
        }
    }
}
