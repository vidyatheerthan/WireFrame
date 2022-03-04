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
using WireFrame.Misc;
using WireFrame.Shapes;

namespace WireFrame.Selection
{
    public class MoveResizeHandler : ISelectionHandler
    {
        private Thickness bounds;
        private MoveResizeControl control = null;
        private Dictionary<IShape, IShape> shapesClones = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeHandler(MoveResizeControl control)
        {
            this.control = control;
            this.shapesClones = new Dictionary<IShape, IShape>();
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
            if (shape == null || this.shapesClones.ContainsKey(shape))
            {
                return false;
            }

            var cloneShape = this.control.AddShape(shape, Utility.GetTopLeft(shape, container));
            this.shapesClones.Add(shape, cloneShape);

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

            foreach (var shape in GetShapes())
            {
                if (!shapes.Contains(shape))
                {
                    this.control.RemoveShape(this.shapesClones[shape]);
                    this.shapesClones.Remove(shape);
                }
            }

            return newAddition;
        }

        public List<IShape> GetShapes()
        {
            return this.shapesClones.Keys.ToList();
        }

        public void UpdateShapes(float zoomFactor)
        {
            ResetBounds();

            var shapes = GetShapes();

            for (int i = 0; i < shapes.Count; ++i)
            {
                Point pos = Utility.GetTopLeft(shapes[i], container, true);
                UpdateControl(shapes[i], pos, zoomFactor, i == 0);
            }

            for (int i = 0; i < shapes.Count; ++i)
            {
                Point pos = Utility.GetTopLeft(shapes[i], container);
                pos = new Point(pos.X - this.control.Left, pos.Y - this.control.Top);
                this.control.UpdateShape(shapes[i], this.shapesClones[shapes[i]], pos, zoomFactor);
            }
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapesClones.ContainsKey(shape))
            {
                this.control.RemoveShape(this.shapesClones[shape]);
                this.shapesClones.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.shapesClones.Clear();
            this.control.RemoveShapes();
        }

        ///-------------------------------------------------------------------

        public void StartResize(Point pointer)
        {
            this.control.StartResize(pointer);            
        }

        public void Resize(Point pointer)
        {
            this.control.Resize(pointer);
        }

        public void StopResize(Point pointer, float zoomFactor)
        {
            this.control.StopResize(pointer);

            double boxScaleX = 0.0, boxScaleY = 0.0;
            this.control.GetScale(ref boxScaleX, ref boxScaleY);

            foreach (var shapeClone in this.shapesClones)
            {
                var srcShape = shapeClone.Value;
                var destShape = shapeClone.Key;

                ShapeCloner.Update(srcShape, destShape, Utility.GetTopLeft(srcShape, destShape.GetControl()), 1.0f / zoomFactor);

                double shapeScaleX = 0.0, shapeScaleY = 0.0;
                srcShape.GetScale(ref shapeScaleX, ref shapeScaleY);

                destShape.SetScale(boxScaleX * shapeScaleX, boxScaleY * shapeScaleY);
            }

            // reset box
            this.control.SetScale(1.0, 1.0);
            var shapes = GetShapes();
            RemoveAllShapes();
            AddShapes(shapes);
            UpdateShapes(zoomFactor);
        }

        ///-------------------------------------------------------------------

        private void UpdateControl(IShape refShape, Point position, float zoomFactor, bool reset)
        {
            if (reset)
            {
                this.bounds.Left = position.X;
                this.bounds.Top = position.Y;
            }
            else
            {
                if (position.X < this.bounds.Left)
                {
                    this.bounds.Left = position.X;
                }

                if (position.Y < this.bounds.Top)
                {
                    this.bounds.Top = position.Y;
                }
            }

            if (position.X + (refShape.GetLength() * zoomFactor) > this.bounds.Right)
            {
                this.bounds.Right = position.X + (refShape.GetLength() * zoomFactor);
            }

            if (position.Y + (refShape.GetBreath() * zoomFactor) > this.bounds.Bottom)
            {
                this.bounds.Bottom = position.Y + (refShape.GetBreath() * zoomFactor);
            }

            this.control.SetLeft(this.bounds.Left);
            this.control.SetTop(this.bounds.Top);
            this.control.SetLength(this.bounds.Right - this.bounds.Left);
            this.control.SetBreath(this.bounds.Bottom - this.bounds.Top);
        }

        private void ResetBounds()
        {
            this.bounds.Top = 0;
            this.bounds.Left = 0;
            this.bounds.Right = 0;
            this.bounds.Bottom = 0;

            this.control.SetLength(0);
            this.control.SetBreath(0);
        }
    } 
}
