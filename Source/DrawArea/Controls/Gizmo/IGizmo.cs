using System;
using Point = Windows.Foundation.Point;

namespace WireFrame.DrawArea.Controls.Gizmo
{
    public interface IGizmo
    {
        void OnActivate(Action<IGizmo> action);
        void StartTrackingPointer(Point pointer);
        void TrackPointer(Point pointer);
        void StopTrackingPointer(Point pointer);
    }
}
