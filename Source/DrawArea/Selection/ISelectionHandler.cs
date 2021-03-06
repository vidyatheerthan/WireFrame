using System.Collections.Generic;
using Windows.UI.Xaml;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.Selection
{
    public interface ISelectionHandler
    {
        void SetContainer(FrameworkElement container);
        void Show(bool show);
        bool AddShape(IShape shape);
        bool AddShapes(List<IShape> shapes);
        bool RemoveShape(IShape shape);
        void RemoveAllShapes();
        List<IShape> GetShapes();
        void UpdateShapes(float zoomFactor);
    }
}
