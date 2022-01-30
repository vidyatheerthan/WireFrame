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
        private double frameWidth;
        private double frameHeight;
        private double zoom;

        //-------------------------------

        public double FrameWidth
        {
            get => this.frameWidth;
        }

        public double FrameHeight
        {
            get => this.frameHeight;
        }

        public double Zoom
        {
            get => this.zoom;
        }

        //-------------------------------

        public CanvasProfile(double frameWidth, double frameHeight, double zoom)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.zoom = zoom;
        }

        public Size ResizeFrame(Size canvasSize)
        {
            // Figure out the ratio
            double ratioX = canvasSize.Width / this.frameWidth;
            double ratioY = canvasSize.Height / this.frameHeight;

            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // shrink it to 75% canvas size
            ratio *= this.zoom / 100.0;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(this.frameWidth * ratio);
            int newHeight = Convert.ToInt32(this.frameHeight * ratio);

            return new Size(newWidth, newHeight);
        }

        public Size ResizeToFrame(Size frameDisplaySize, Size size)
        {
            double widthRatio = frameDisplaySize.Width / this.FrameWidth;
            double heightRatio = frameDisplaySize.Height / this.FrameHeight;

            int newWidth = Convert.ToInt32(size.Width * widthRatio);
            int newHeight = Convert.ToInt32(size.Height * heightRatio);

            return new Size(newWidth, newHeight);
        }
    }
}
