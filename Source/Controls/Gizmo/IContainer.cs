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
    public interface IContainer
    {
        IShape AddShape(IShape refShape, Point position);
        void RemoveShape(IShape cloneShape);
        void RemoveShapes();
        void UpdateShape(IShape refView, IShape cloneShape, Point position, float zoomFactor);
        List<IShape> GetShapes();
    }
}
