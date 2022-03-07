using WireFrame.DrawArea.Controls.Gizmo;

namespace WireFrame.DrawArea.Controls
{
    public interface IGizmo : IContainer, IBox, IGizmoHandler
    {
        void Activate(bool activate);
    }
}
