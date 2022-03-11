using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using WireFrame.DrawArea.Controls;
using WireFrame.DrawArea.Misc;
using WireFrame.DrawArea.Shapes;

namespace WireFrame.DrawArea.Selection
{
    public class MoveResizeRotateHandler : ISelectionHandler
    {
        private Thickness bounds;
        private IGizmo control = null;
        private Dictionary<IShape, IShape> shapesClones = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeRotateHandler(IGizmo control)
        {
            this.control = control;
            this.shapesClones = new Dictionary<IShape, IShape>();
        }

        public void Show(bool show)
        {
            this.control.Activate(show);
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
                pos = new Point(pos.X - this.control.GetLeft(), pos.Y - this.control.GetTop());
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

        public void StartTrackingPointer(Point pointer)
        {
            this.control.StartTrackingPointer(pointer);            
        }

        public void TrackPointer(Point pointer)
        {
            this.control.TrackPointer(pointer);
        }

        public void StopTrackingPointer(Point pointer, float zoomFactor)
        {
            this.control.StopTrackingPointer(pointer);

            double boxScaleX = 0.0, boxScaleY = 0.0;
            this.control.GetScale(ref boxScaleX, ref boxScaleY);
            Point boxTransformPos = this.control.GetTransformOrigin(true);
            double boxRot = this.control.GetRotation();

            foreach (var shapeClone in this.shapesClones)
            {
                var srcShape = shapeClone.Value;
                var destShape = shapeClone.Key;

                ShapeCloner.Update(srcShape, destShape, Utility.GetTopLeft(srcShape, destShape.GetControl()), 1.0f / zoomFactor);

                double shapeScaleX = 0.0, shapeScaleY = 0.0;
                srcShape.GetScale(ref shapeScaleX, ref shapeScaleY);

                destShape.SetScale(boxScaleX * shapeScaleX, boxScaleY * shapeScaleY);
                //destShape.SetTransformOrigin(TransformPoint(destShape.GetControl(), boxTransformPos));
                destShape.SetRotation(boxRot);
            }

            // reset box
            this.control.SetScale(1.0, 1.0);
            this.control.SetRotation(0.0);
            var shapes = GetShapes();
            RemoveAllShapes();
            AddShapes(shapes);
            UpdateShapes(zoomFactor);
        }

        private Point TransformPoint(FrameworkElement dstElement, Point rootPosition)
        {
            GeneralTransform t = dstElement.TransformToVisual(null).Inverse;
            var tp = t.TransformPoint(rootPosition);
            return tp;
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
