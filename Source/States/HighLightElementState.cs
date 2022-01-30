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

namespace WireFrame.Source.States
{
    class HighLightElementState : FiniteStateMachine
    {
        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count >= 2 && (objects[0] is Panel) && (objects[1] is WFTitleBox))
            {
                return true;
            }

            return false;
        }

        public FiniteStateMachine HandleInput(List<object> objects, PointerState pointerState, PointerPoint pointer)
        {
            if (!ReferenceObjectsAccepted(objects))
            {
                return null;
            }

            if (pointerState == PointerState.Pressed && pointer.Properties.IsLeftButtonPressed)
            {
            }
            else if (pointerState == PointerState.Moved)
            {
            }
            else if (pointerState == PointerState.Released)
            {
            }

            return this;
        }
    }
}
