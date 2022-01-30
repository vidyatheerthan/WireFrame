using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;

namespace WireFrame.Source.States
{
    class PanState : FiniteStateMachine
    {
        private bool tracking = false;

        private Point clickedPosition;


        public bool ReferenceObjectsAccepted(List<object> objects)
        {
            if (objects != null && objects.Count > 0 && (objects[0] is ScrollViewer))
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
                this.clickedPosition = pointer.Position;
                this.tracking = true;
            }
            else if (pointerState == PointerState.Moved && tracking)
            {
                if (objects[0] is ScrollViewer)
                {
                    var sv = objects[0] as ScrollViewer;

                    double x = sv.HorizontalOffset + this.clickedPosition.X - pointer.Position.X;
                    double y = sv.VerticalOffset + this.clickedPosition.Y - pointer.Position.Y;

                    sv.ChangeView(x, y, null, true);
                }
            }
            else if (pointerState == PointerState.Released)
            {
                this.tracking = false;
                return null;
            }

            return this;
        }
    }
}
