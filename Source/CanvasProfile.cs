using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = Windows.Foundation.Size;

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

        public CanvasProfile(double frameWidth, double frameHeight)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public Size GetCanvas(Size screenSize)
        {
            // Figure out the ratio
            double ratioX = this.frameWidth / screenSize.Width;
            double ratioY = this.frameHeight / screenSize.Height;

            // use whichever multiplier is smaller
            double ratio = ratioX > ratioY ? ratioX : ratioY;

            ratio *= 1.5; // increate screen size by 50%
            this.zoom = 1.0 / ratio;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(screenSize.Width * ratio);
            int newHeight = Convert.ToInt32(screenSize.Height * ratio);

            return new Size(newWidth, newHeight);
        }
    }
}
