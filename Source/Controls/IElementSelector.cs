using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WireFrame
{
    interface IElementSelector
    {
        void Show(bool show);
        void SetSelectedElement(FrameworkElement element, FrameworkElement parent, float zoomFactor);
    }
}
