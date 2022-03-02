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
        private Dictionary<IShape, Viewbox> shapesClones = null;
        private FrameworkElement container = null;

        ///-------------------------------------------------------------------

        public MoveResizeHandler(MoveResizeControl control)
        {
            this.control = control;
            this.shapesClones = new Dictionary<IShape, Viewbox>();
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

            Point pos = Utility.GetPointInContainer(shape, container);
            this.shapesClones.Add(shape, this.control.AddContentItem(shape.GetViewbox(), pos));

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
                    this.control.RemoveContentItem(this.shapesClones[shape]);
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
                Point pos = Utility.GetPointInContainer(shapes[i], container);
                UpdateControl(shapes[i].GetViewbox(), pos, zoomFactor, i == 0);
            }

            for (int i = 0; i < shapes.Count; ++i)
            {
                Point pos = Utility.GetPointInContainer(shapes[i], container);
                pos = new Point(pos.X - this.control.Left, pos.Y - this.control.Top);
                this.control.UpdateContentItem(shapes[i].GetViewbox(), this.shapesClones[shapes[i]], pos, zoomFactor);
            }
        }

        public bool RemoveShape(IShape shape)
        {
            if (this.shapesClones.ContainsKey(shape))
            {
                this.control.RemoveContentItem(this.shapesClones[shape]);
                this.shapesClones.Remove(shape);
                return true;
            }

            return false;
        }

        public void RemoveAllShapes()
        {
            this.shapesClones.Clear();
            this.control.RemoveContents();
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

        public void StopResize(Point pointer)
        {
            this.control.StopResize(pointer);
        }

        ///-------------------------------------------------------------------

        public void UpdateControl(Viewbox refView, Point position, float zoomFactor, bool reset)
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

            if (position.X + (refView.ActualWidth * zoomFactor) > this.bounds.Right)
            {
                this.bounds.Right = position.X + (refView.ActualWidth * zoomFactor);
            }

            if (position.Y + (refView.ActualHeight * zoomFactor) > this.bounds.Bottom)
            {
                this.bounds.Bottom = position.Y + (refView.ActualHeight * zoomFactor);
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
