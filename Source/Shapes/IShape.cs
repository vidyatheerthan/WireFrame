using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Shapes
{
    public interface IShape
    {
        double GetLeft();
        void SetLeft(double left);

        double GetTop();
        void SetTop(double top);

        double GetLength();
        void SetLength(double length);

        double GetBreath();
        void SetBreath(double breath);

        Viewbox GetViewbox();
        Path GetPath();
    }
}
