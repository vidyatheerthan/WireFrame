using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WireFrame.Shapes
{
    public class BoundingBox
    {
        private RectangleShape rect;
        private Point clickPoint;

        public BoundingBox(Color stroke, Color fill)
        {
            this.rect = new RectangleShape();
            this.rect.Stroke = new SolidColorBrush(stroke);
            this.rect.Fill = new SolidColorBrush(fill);
            this.rect.PathStretch = Stretch.Fill;
            this.rect.ViewStretch = Stretch.Fill;
        }

        public void StartTracking(Point clickPoint)
        {
            this.clickPoint = clickPoint;

            this.rect.SetLeft(clickPoint.X);
            this.rect.SetTop(clickPoint.Y);
            this.rect.SetLength(1);
            this.rect.SetBreath(1);

            this.rect.Visibility = Visibility.Visible;
        }

        public void Track(Point point)
        {
            double xDiff = point.X - this.clickPoint.X;
            double yDiff = point.Y - this.clickPoint.Y;

            if(xDiff < 0)
            {
                this.rect.SetLeft(point.X);
                this.rect.SetLength(-xDiff);
            }
            else
            {
                this.rect.SetLength(xDiff);
            }

            if (yDiff < 0)
            {
                this.rect.SetTop(point.Y);
                this.rect.SetBreath(-yDiff);
            }
            else
            {
                this.rect.SetBreath(yDiff);
            }
        }

        public void StopTracking()
        {
            this.rect.Visibility = Visibility.Collapsed;
        }

        public RectangleShape GetRectangle()
        {
            return this.rect;
        }

        public Rect GetBounds()
        {
            Rect bounds = new Rect(this.rect.Left, this.rect.Top, this.rect.Length, this.rect.Breath);
            return bounds;
        }
    }
}
