using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WireFrame.Shapes
{
    public interface IBox
    {
        double GetLeft();
        void SetLeft(double left);

        double GetTop();
        void SetTop(double top);

        double GetLength();
        void SetLength(double length);

        double GetBreath();
        void SetBreath(double breath);
    }
}
