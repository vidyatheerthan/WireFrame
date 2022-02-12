using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WireFrame.Shapes;

namespace WireFrame
{
    interface IElementSelector
    {
        void Show(bool show);
        void SetSelectedShape(IShape shape, FrameworkElement parent, float zoomFactor);
        IShape GetSelectedShape();
        void UpdateSelectedShape(float zoomFactor);
    }
}
