using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WireFrame.Controls;
using WireFrame.Shapes;

namespace WireFrame.Selection
{
    public class MoveResizeHandler : ISelectionHandler
    {
        private MoveResizeControl control = null;
        private List<IShape> shapes = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeHandler(MoveResizeControl control)
        {
            this.control = control;
            this.shapes = new List<IShape>();
        }

        public void Show(bool show)
        {
            this.control.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetContainer(FrameworkElement container)
        {
            this.container = container;
        }

        public bool AddShape(IShape shape)
        {
            if (shape == null || this.shapes.Contains(shape))
            {
                return false;
            }

            this.shapes.Add(shape);

            return true;
        }

        public bool AddShapes(List<IShape> shapes)
        {
            bool newAddition = false;

            foreach (var shape in shapes)
            {
                if (AddShape(shape))
                {
                    newAddition = true;
                }
            }

            foreach (var shape in this.shapes.ToList())
            {
                if (!shapes.Contains(shape))
                {
                    this.shapes.Remove(shape);
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            return this.shapes;
        }

        public void UpdateShapes(float zoomFactor)
        {
            this.control.ResetBounds();

            for (int i = 0; i < this.shapes.Count; ++i)
            {
                this.control.UpdateCorners(this.container, this.shapes[i], zoomFactor, i == 0);
            }

            this.control.Update();
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapes.Contains(shape))
            {
                this.shapes.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.shapes.Clear();
            this.control.ResetBounds();
        }

        public void StartResize(Point pointer)
        {
            this.control.StartResize(pointer);            
        }

        public void Resize(Point pointer)
        {
            this.control.Resize(pointer);
        }

        public void StopResize(Point pointer)
        {
            this.control.StopResize(pointer);
        }
    }
}
