using Windows.Foundation;

namespace WireFrame.DrawArea
{
    public interface IMouseEventHandler
    {
        void StartTrackingPointer(Point pointer);
        void TrackPointer(Point pointer);
        void StopTrackingPointer(Point pointer);
    }
}
