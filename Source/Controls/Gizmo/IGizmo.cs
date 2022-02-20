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
        void StartTrackingPointer(Point point);
        void TrackPointer(Point point);
        void StopTrackingPointer(Point point);
        void Update(Rect rect);
    }
}
