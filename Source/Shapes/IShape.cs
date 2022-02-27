using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WireFrame.Shapes
{
    public interface IShape : IBox
    {
        Viewbox GetViewbox();
        Path GetPath();
    }
}
