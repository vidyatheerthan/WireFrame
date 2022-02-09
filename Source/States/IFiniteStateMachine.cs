using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WireFrame
{
    interface IFiniteStateMachine
    {
        bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e);
        void HandleZoom();
        void ActiveState(IFiniteStateMachine state);
    }
}
