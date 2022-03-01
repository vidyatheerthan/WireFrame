using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WireFrame.Shapes;

namespace WireFrame.Controls.Gizmo
{
    public interface IContainer : IBox
    {
        List<Viewbox> GetContents();
    }
}
