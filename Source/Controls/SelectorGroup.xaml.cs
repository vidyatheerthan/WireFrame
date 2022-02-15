using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WireFrame.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.Controls
{
    //[ContentProperty(Name = nameof(Children))]
    public sealed partial class SelectorGroup : UserControl, IMultiSelector
    {

        //public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
        //    nameof(Children),
        //    typeof(ISelector),
        //    typeof(SelectorGroup),
        //    new PropertyMetadata(null)
        //);

        //public ISelector Children
        //{
        //    get { return (ISelector)GetValue(ChildrenProperty); }
        //    set { SetValue(ChildrenProperty, value); }
        //}

        public static readonly DependencyProperty SelectorProperty = DependencyProperty.Register(
            nameof(Selector),
            typeof(ISelector),
            typeof(SelectorGroup),
            new PropertyMetadata(null)
        );

        public ISelector Selector
        {
            get { return (ISelector)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }

        public SelectorGroup()
        {
            this.InitializeComponent();
        }

        public void Show(bool show)
        {
            throw new NotImplementedException();
        }

        public void SetContainer(Canvas container)
        {
            throw new NotImplementedException();
        }

        public void AddShape(IShape shape)
        {
            throw new NotImplementedException();
        }

        public void RemoveShape(IShape shape)
        {
            throw new NotImplementedException();
        }

        public IShape GetShapes()
        {
            throw new NotImplementedException();
        }

        public void UpdateShapes(float zoomFactor)
        {
            throw new NotImplementedException();
        }
    }
}
