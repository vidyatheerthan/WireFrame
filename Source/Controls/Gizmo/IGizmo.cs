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
        void StartTrackingPointer(Point pointer);
        void TrackPointer(Point pointer);
        void StopTrackingPointer(Point pointer);
    }
}
