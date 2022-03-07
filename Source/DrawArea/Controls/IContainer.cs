using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.Controls
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
