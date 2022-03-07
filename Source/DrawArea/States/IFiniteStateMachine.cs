using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Misc;

namespace WireFrame.DrawArea.States
{
    public interface IFiniteStateMachine
    {
        bool HandleInput(PointerState pointerState, PointerRoutedEventArgs e);
        bool HandleInput(KeyBoardState keyboardState, KeyEventArgs args);
        void HandleZoom();
        void HandleScroll();
        void ActiveState(IFiniteStateMachine state);
    }
}
