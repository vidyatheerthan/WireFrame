using System;
using Size = Windows.Foundation.Size;

namespace WireFrame.DrawArea.Misc
{
    public class CanvasProfile
    {
        private double frameWidth;
        private double frameHeight;

        //-------------------------------

        public double FrameWidth
        {
            get => this.frameWidth;
        }

        public double FrameHeight
        {
            get => this.frameHeight;
        }


        //-------------------------------

        public CanvasProfile(double frameWidth, double frameHeight)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public Size GetCanvas(Size screenSize, out float zoomFactor)
        {
            // Figure out the ratio
            double ratioX = this.frameWidth / screenSize.Width;
            double ratioY = this.frameHeight / screenSize.Height;

            // use whichever multiplier is smaller
            double ratio = ratioX > ratioY ? ratioX : ratioY;

            ratio *= 1.5; // increate screen size by 50%
            zoomFactor = 1.0f / (float)ratio;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(screenSize.Width * ratio);
            int newHeight = Convert.ToInt32(screenSize.Height * ratio);

            return new Size(newWidth, newHeight);
        }

        public Size ResizeFrameToCanvas(Size canvasSize, out float zoomFactor)
        {
            // Figure out the ratio
            double ratioX = canvasSize.Width / this.frameWidth;
            double ratioY = canvasSize.Height / this.frameHeight;

            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // shrink it to 50% canvas size
            ratio *= 0.5;
            zoomFactor = 1.0f;

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(this.frameWidth * ratio);
            int newHeight = Convert.ToInt32(this.frameHeight * ratio);

            return new Size(newWidth, newHeight);
        }
    }
}
