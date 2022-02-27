using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Point = Windows.Foundation.Point;

namespace WireFrame.Controls.Gizmo
{
    public interface IGizmo
    {
        void OnActivate(Action<IGizmo> action);
        void StartTrackingPointer(Panel box, Point pointer);
        void TrackPointer(Panel box, Point pointer);
        void StopTrackingPointer(Panel box, Point pointer);
        void Update(Rect rect);
    }
}
