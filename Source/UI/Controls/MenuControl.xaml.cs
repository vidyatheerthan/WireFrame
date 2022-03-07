using deVoid.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.States;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WireFrame.UI.Controls
{
    public sealed partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            this.InitializeComponent();
        }

        private void OnPointerPressedOnDrawRectangle(object sender, PointerRoutedEventArgs e)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.DrawRectangle);
        }

        private void OnPointerPressedOnDrawEllipse(object sender, PointerRoutedEventArgs e)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.DrawEllipse);
        }
    }
}
