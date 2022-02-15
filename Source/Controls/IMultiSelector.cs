using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WireFrame.Shapes;

namespace WireFrame.Controls
{
    interface IMultiSelector
    {
        void Show(bool show);
        void SetContainer(Canvas container);
        void AddShape(IShape shape);
        void RemoveShape(IShape shape);
        IShape GetShapes();
        void UpdateShapes(float zoomFactor);
    }
}
