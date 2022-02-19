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
using WireFrame.Controls;
using WireFrame.Misc;
using WireFrame.Selection;

namespace WireFrame.States
{
    class ResizeState : IFiniteStateMachine
    {
        private class Data
        {
            public ScrollViewer scrollViewer;
            public Canvas canvas;
            public MoveResizeHandler resizeHandler;
            public VirtualKey key;

            public Data(ScrollViewer scrollViewer, Canvas canvas, MoveResizeHandler resizeHandler, VirtualKey key)
            {
                this.scrollViewer = scrollViewer;
                this.canvas = canvas;
                this.resizeHandler = resizeHandler;
                this.key = key;
            }
        }

        // --

        private Data data = null;

        // --

        public ResizeState(List<object> objects)
        {
            if (objects != null && objects.Count == 4 && (objects[0] is ScrollViewer) && (objects[1] is Canvas) && (objects[2] is MoveResizeHandler) && (objects[3].GetType().IsEnum))
            {
                this.data = new Data(objects[0] as ScrollViewer, objects[1] as Canvas, objects[2] as MoveResizeHandler, (VirtualKey)objects[3]);
            }
        }

        public bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            return false;
        }

        public bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
            return false;
        }

        public void HandleZoom()
        {
        }

        public void HandleScroll()
        {
        }

        public void ActiveState(IFiniteStateMachine state)
        {
        }
    }
}
