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
        private Dictionary<IShape, Rect> shapeSizes = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeHandler(MoveResizeControl control)
        {
            this.control = control;
            this.shapeSizes = new Dictionary<IShape, Rect>(); // each shape and their size contribution in _box
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
            if (shape == null || this.shapeSizes.ContainsKey(shape))
            {
                return false;
            }

            this.shapeSizes.Add(shape, Rect.Empty);

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

            foreach (var shape in this.shapeSizes.Keys.ToList())
            {
                if (!shapes.Contains(shape))
                {
                    this.shapeSizes.Remove(shape);
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            var shapes = this.shapeSizes.Keys.ToList();
            return shapes;
        }

        public void UpdateShapes(float zoomFactor)
        {
            this.control.ResetBounds();
            var shapes = GetShapes();

            for (int i = 0; i < shapes.Count; ++i)
            {
                IShape shape = shapes[i];
                this.control.UpdateCorners(this.container, shape, zoomFactor, i == 0);
            }

            this.control.Update();
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapeSizes.ContainsKey(shape))
            {
                this.shapeSizes.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.shapeSizes.Clear();

            this.control.ResetBounds();
        }

        public void StartResize(Point pointer)
        {
            this.control.StartResize(pointer);

            Rect rect = this.control.GetCanvasRect();
            foreach (IShape shape in GetShapes())
            {
                this.shapeSizes[shape] = new Rect(shape.GetLeft() / rect.X, shape.GetTop() / rect.Y, shape.GetLength() / rect.Width, shape.GetBreath() / rect.Height);
            }            
        }

        public void Resize(Point pointer)
        {
            this.control.Resize(pointer);

            Rect crect = this.control.GetCanvasRect();

            foreach (var kv in this.shapeSizes) {
                IShape shape = kv.Key;
                Rect shapeRect = kv.Value;
                
                shape.SetLeft(crect.X * shapeRect.X);
                shape.SetTop(crect.Y * shapeRect.Y);
                shape.SetLength(crect.Width * shapeRect.Width);
                shape.SetBreath(crect.Height * shapeRect.Height);
            }
        }

        public void StopResize(Point pointer)
        {
            this.control.StopResize(pointer);
        }
    }
}
