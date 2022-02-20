using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Point = Windows.Foundation.Point;

namespace WireFrame.Controls.Gizmo
{
    public interface IGizmo
    {
        void OnActivate(Action<IGizmo> action);
        void StartTrackingPointer(ref Point topLeft, ref Point bottomRight, Point pointer);
        void TrackPointer(ref Point topLeft, ref Point bottomRight, Point pointer);
        void StopTrackingPointer(ref Point topLeft, ref Point bottomRight, Point pointer);
        void Update(Rect rect);
    }
}
