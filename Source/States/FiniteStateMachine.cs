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

namespace WireFrame.Source.States
{
    interface FiniteStateMachine
    {
        bool ReferenceObjectsAccepted(List<object> objects);
        bool HandleInput(List<object> objects, PointerState pointerState, PointerRoutedEventArgs e);
    }
}
