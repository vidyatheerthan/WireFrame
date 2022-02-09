using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WireFrame
{
    class FocusState : IFiniteStateMachine
    {
        private Windows.System.VirtualKey key;

        public FocusState(Windows.System.VirtualKey key)
        {
            this.key = key;
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            return false;
        }

        public bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
            Windows.System.VirtualKey pressedKey = args.VirtualKey;

            if(pressedKey == this.key)
            {
                
            }

            return false;
        }

        public void HandleZoom()
        {
        }

        public void ActiveState(IFiniteStateMachine state)
        {
        }
    }
}
