using System;
using Windows.Foundation;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    public interface IGizmoHandler : IMouseEventHandler
    {
        void OnActivate(Action<IGizmoHandler> action);
    }
}
