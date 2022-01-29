using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireFrame
{
    enum Action
    {
        None,
        Pan,
        CreateNewEllipse
    }

    enum PointerState
    {
        Pressed,
        Moving,
        Dragging,
        Released
    }
}
