using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using WireFrame.Shapes;
using Size = Windows.Foundation.Size;
using Point = Windows.Foundation.Point;

namespace WireFrame.Misc
{
    public static class Utility
    {
        public static Size GetScreenResolution()
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            var screenSize = new Size((int)displayInformation.ScreenWidthInRawPixels,
                                      (int)displayInformation.ScreenHeightInRawPixels);

            return screenSize;
        }

        public static Size GetSizeRelativeTo(Size size, FrameworkElement source, FrameworkElement destination)
        {
            double widthRatio = size.Width / source.ActualWidth;
            double heightRatio = size.Height / source.ActualHeight;

            return new Size(destination.ActualWidth * widthRatio, destination.ActualHeight * heightRatio);
        }

        public static Point GetTopLeft(IShape shape, FrameworkElement container, bool useScale = false)
        {
            var transform = shape.GetViewbox().TransformToVisual(container);

            if (useScale)
            {
                double scaleX = 0.0, scaleY = 0.0;
                shape.GetScale(ref scaleX, ref scaleY);

                if (scaleX < 0 && scaleY > 0)
                {
                    return transform.TransformPoint(new Point(shape.GetLength(), 0));
                }

                if (scaleX > 0 && scaleY < 0)
                {
                    return transform.TransformPoint(new Point(0, shape.GetBreath()));
                }

                if (scaleX < 0 && scaleY < 0)
                {
                    return transform.TransformPoint(new Point(shape.GetLength(), shape.GetBreath()));
                }
            }

            return transform.TransformPoint(new Point(0, 0));
        }
    }
}
