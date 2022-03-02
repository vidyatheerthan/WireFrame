using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using WireFrame.Shapes;

namespace WireFrame.Controls.Gizmo
{
    public interface IContainer : IBox
    {
        Viewbox AddContentItem(Viewbox referenceItemView, Point position);
        void RemoveContentItem(Viewbox viewbox);
        void RemoveContents();
        void UpdateContentItem(Viewbox refView, Viewbox cloneView, Point position, float zoomFactor);
        List<Viewbox> GetContents();
    }
}
