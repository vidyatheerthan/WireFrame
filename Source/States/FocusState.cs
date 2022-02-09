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
using Windows.System;

namespace WireFrame
{
    class FocusState : IFiniteStateMachine
    {
        private class Data
        {
            public WFSizeBox sizeBox;
            public VirtualKey key;
            
            public Data(WFSizeBox sizeBox, VirtualKey key)
            {
                this.sizeBox = sizeBox;
                this.key = key;
            }
        }

        // --

        private Data data = null;

        // --

        public FocusState(List<object> objects)
        {
            if (objects != null && objects.Count == 2 && (objects[0] is WFSizeBox) && (objects[1].GetType().IsEnum))
            {
                this.data = new Data(objects[0] as WFSizeBox, (VirtualKey)objects[1]);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            return false;
        }

        public bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
            if (this.data == null)
            {
                return false;
            }

            if(args.VirtualKey == this.data.key && this.data.sizeBox.SelectedElement != null)
            {
                double width = this.data.sizeBox.SelectedElement.Width;
                double height = this.data.sizeBox.SelectedElement.Height;
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
