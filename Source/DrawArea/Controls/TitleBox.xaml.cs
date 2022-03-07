using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WireFrame.DrawArea.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.DrawArea.Controls
{
    public sealed partial class TitleBox : UserControl//, ISelector
    {
        private IShape selectedShape;
        private FrameworkElement container;

        // --

        public TitleBox()
        {
            this.InitializeComponent();
        }

        public void SetSelectedShape(IShape shape, FrameworkElement container, float zoomFactor)
        {
            this.selectedShape = shape;
            this.container = container;

            UpdateSelectedShape(zoomFactor);
        }

        public void UpdateSelectedShape(float zoomFactor)
        {
            if (this.selectedShape == null || this.container == null) { return; }

            var transform = this.selectedShape.GetPath().TransformToVisual(this.container);
            var ePoint = transform.TransformPoint(new Point(0, 0));

            Canvas.SetLeft(_box, ePoint.X);
            Canvas.SetTop(_box, ePoint.Y);
            _box.Width = this.selectedShape.GetLength() * zoomFactor;
            _box.Height = this.selectedShape.GetBreath() * zoomFactor;

            UpdateTextBoxSize();
        }

        public IShape GetSelectedShape()
        {
            return this.selectedShape;
        }

        private void UpdateTextBoxSize()
        {
            if (_box.ActualWidth < _textBorder.Width || _box.ActualHeight < _textBorder.Height)
            {
                _textBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                _textBorder.Visibility = Visibility.Visible;

                Canvas.SetLeft(_textBorder, Canvas.GetLeft(_box));
                Canvas.SetTop(_textBorder, Canvas.GetTop(_box) - _textBorder.Height);
            }
        }

        public void SetTitle(string title)
        {
            _textBlock.Text = title;
        }

        public void Show(bool show)
        {
            Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
