using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireFrame.Shapes;

namespace WireFrame.Controls.Gizmo
{
    public interface IContainer : IBox
    {
        List<IShape> GetContents();
    }
}
