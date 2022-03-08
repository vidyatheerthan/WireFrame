using WireFrame.DrawArea.Controls.Gizmo;

namespace WireFrame.DrawArea.Controls
{
    public interface IGizmo : IContainer, IBox, IMouseEventHandler
    {
        void Activate(bool activate);
    }
}
