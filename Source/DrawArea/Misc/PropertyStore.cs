using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.DrawArea.Misc
{
    class PropertyStore
    {
        // common
        private double width;
        private double height;

        // Border
        private double cornerRadiusTopLeft;
        private double cornerRadiusTopRight;
        private double cornerRadiusBottomLeft;
        private double cornerRadiusBottomRight;

        // TextBlock
        private double fontSize;

        // Shape
        private double strokeThickness;

        //------------------------------------------------------------------

        public double Width { get => width; }
        public double Height { get => height; }

        public double CornerRadiusTopLeft { get => cornerRadiusTopLeft; }
        public double CornerRadiusTopRight { get => cornerRadiusTopRight; }
        public double CornerRadiusBottomLeft { get => cornerRadiusBottomLeft; }
        public double CornerRadiusBottomRight { get => cornerRadiusBottomRight; }


        public double FontSize { get => fontSize; }

        public double StrokeThickness { get => strokeThickness; }

        //------------------------------------------------------------------

        public PropertyStore(Border border)
        {
            this.width = border.Width;
            this.height = border.Height;
            //
            this.cornerRadiusTopLeft = border.CornerRadius.TopLeft;
            this.cornerRadiusTopRight = border.CornerRadius.TopRight;
            this.cornerRadiusBottomLeft = border.CornerRadius.BottomLeft;
            this.cornerRadiusBottomRight = border.CornerRadius.BottomRight;
        }

        public PropertyStore(TextBlock textBlock)
        {
            this.width = textBlock.Width;
            this.height = textBlock.Height;
            //
            this.fontSize = textBlock.FontSize;
        }

        public PropertyStore(Shape shape)
        {
            this.width = shape.Width;
            this.height = shape.Height;
            //
            this.strokeThickness = shape.StrokeThickness;
        }
    }
}
