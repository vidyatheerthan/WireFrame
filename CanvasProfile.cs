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

        //-------------------------------

        public double Width
        {
            get => this.width;
        }

        public double Height
        {
            get => this.height;
        }

        //-------------------------------

        public CanvasProfile(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public Size Resize(Size canvasSize)
        {
            // Figure out the ratio
            double ratioX = canvasSize.Width / width;
            double ratioY = canvasSize.Height / height;

            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // shrink it to 75% canvas size
            ratio *= 0.75;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(width * ratio);
            int newHeight = Convert.ToInt32(height * ratio);

            return new Size(newWidth, newHeight);
        }
    }
}
