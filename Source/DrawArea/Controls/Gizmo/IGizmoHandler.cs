using Windows.Foundation;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    public interface IGizmoHandler
    {
        void StartTrackingPointer(Point pointer);
        void TrackPointer(Point pointer);
        void StopTrackingPointer(Point pointer);
    }
}
