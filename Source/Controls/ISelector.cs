using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WireFrame.Shapes;

namespace WireFrame
{
    public interface ISelector
    {
        void SetContainer(FrameworkElement container);
        void Show(bool show);
        bool AddShape(IShape shape);
        bool AddShapes(List<IShape> shapes);
        List<IShape> GetShapes();
        void UpdateShapes(float zoomFactor);
        void RemoveAllShapes();
    }
}
