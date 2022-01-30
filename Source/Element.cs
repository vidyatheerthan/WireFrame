using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WireFrame
{
    public class Element
    {
        private double left;
        private double top;

        private double width;
        private double height;

        private FrameworkElement frameworkElement;

        //-------------------------------

        public double Left
        {
            get => this.left;
        }

        public double Top
        {
            get => this.top;
        }

        public double Width
        {
            get => this.width;
        }

        public double Height
        {
            get => this.height;
        }

        public FrameworkElement FrameworkElement
        {
            get => this.frameworkElement;
        }

        //-------------------------------

        public Element(double left, double top, double width, double height, FrameworkElement frameworkElement)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.frameworkElement = frameworkElement;
        }
    }
}
