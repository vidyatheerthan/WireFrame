using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WireFrame
{
    public class WFElement
    {
        private double left;
        private double top;

        private double width;
        private double height;

        private FrameworkElement element;

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

        public FrameworkElement Element
        {
            get => this.element;
        }

        //-------------------------------

        public WFElement(double left, double top, double width, double height, FrameworkElement element)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.element = element;
        }
    }
}
