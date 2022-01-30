using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Source.States
{
    class DrawEllipseState : DrawPrimitiveState
    {
        protected override FrameworkElement AddNewPrimitive(Panel canvas, double left, double top, double width, double height)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
            ellipse.Stroke = new SolidColorBrush(Colors.Orange);
            ellipse.Fill = new SolidColorBrush(Colors.LightGoldenrodYellow);

            canvas.Children.Insert(canvas.Children.Count, ellipse);
            return ellipse;
        }

        protected override void ResizePrimitive(FrameworkElement element, double x, double y)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);

            double width = x - left;
            double height = y - top;

            element.Width = width > 0 ? width : 1;
            element.Height = height > 0 ? height : 1;
        }
    }
}
