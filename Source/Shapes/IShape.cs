using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Shapes
{
    public interface IShape : IBox
    {
        void SetStroke(Brush brush);
        Brush GetStroke();

        void SetFill(Brush brush);
        Brush GetFill();

        void SetFillRule(FillRule rule);
        FillRule GetFillRule();

        void SetPathStretch(Stretch stretch);
        Stretch GetPathStretch();

        void SetOpacity(double opacity);
        double GetOpacity();

        void SetViewbox(Viewbox viewbox);
        Viewbox GetViewbox();

        void SetPath(Path path);
        Path GetPath();

        Control GetControl();
    }
}
