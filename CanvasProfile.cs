using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireFrame
{
    public class CanvasProfile
    {
        private double width;
        private double height;
        private double zoom;

        //-------------------------------

        public double Width
        {
            get => this.width;
        }

        public double Height
        {
            get => this.height;
        }

        public double Zoom
        {
            get => this.zoom;
        }

        //-------------------------------

        public CanvasProfile(double width, double height, double zoom)
        {
            this.width = width;
            this.height = height;
            this.zoom = zoom;
        }

        public Size Resize(Size screenSize)
        {
            // Figure out the ratio
            double ratioX = screenSize.Width / this.width;
            double ratioY = screenSize.Height / this.height;

            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // shrink it to 75% screen size
            ratio *= this.zoom / 100.0;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(this.width * ratio);
            int newHeight = Convert.ToInt32(this.height * ratio);

            return new Size(newWidth, newHeight);
        }
    }
}
