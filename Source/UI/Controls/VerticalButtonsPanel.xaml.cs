using deVoid.Utils;
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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WireFrame.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.UI.Controls
{
    public sealed partial class VerticalButtonsPanel : UserControl
    {
        public VerticalButtonsPanel()
        {
            this.InitializeComponent();
        }

        private void OnPointerPressedOnMoveResize(object sender, PointerRoutedEventArgs e)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectMoveResize_Pan_Focus);
        }

        private void OnPointerPressedOnSelect(object sender, PointerRoutedEventArgs e)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.Select_Pan);
        }
    }
}
