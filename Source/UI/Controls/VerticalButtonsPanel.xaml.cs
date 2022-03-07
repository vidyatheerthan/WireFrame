using deVoid.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.States;

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

        private void OnPointerPressedOnRotate(object sender, PointerRoutedEventArgs e)
        {
            Signals.Get<ChangeToState>().Dispatch(StateExecutor.State.SelectRotate_Pan_Focus);
        }
    }
}
