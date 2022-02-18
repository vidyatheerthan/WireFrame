using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using WireFrame.Misc;
using deVoid.Utils;

namespace WireFrame.States
{
    public class StateExecutor
    {
        public enum State
        {
            Selection_Pan_Focus,
            RotateElement,
            DrawEllipse,
            DrawRectangle
        }

        private Dictionary<State, List<IFiniteStateMachine>> stateList;
        private List<IFiniteStateMachine> activeStates = null;
        IFiniteStateMachine activeState = null;

        public StateExecutor(List<IFiniteStateMachine> states)
        {
            this.activeStates = states;

            Signals.Get<ChangeToState>().AddListener(SelectState);
        }

        public StateExecutor(Dictionary<State, List<IFiniteStateMachine>> stateList)
        {
            this.stateList = stateList;

            Signals.Get<ChangeToState>().AddListener(SelectState);
        }

        private void SelectState(State state)
        {
            if(this.stateList != null && this.stateList.ContainsKey(state) && this.stateList[state] != null && this.stateList[state].Count > 0)
            {
                this.activeStates = this.stateList[state];
            }
        }

        private void HandleInput (Func<IFiniteStateMachine, bool> func)
        {
            bool handle = false;

            if(this.activeState != null)
            {
                foreach (IFiniteStateMachine state in this.activeStates)
                {
                    if (state == this.activeState)
                    {
                        handle = func(this.activeState);
                        if (!handle)
                        {
                            this.activeState = null;
                        }
                    }
                    else
                    {
                        state.ActiveState(this.activeState);
                    }
                }
            }
            else if(this.activeStates != null)
            {
                foreach (IFiniteStateMachine state in this.activeStates)
                {
                    handle = func(state);
                    if (handle)
                    {
                        this.activeState = state;
                        break;
                    }
                }
            }
        }

        public void HandleInput(PointerState pointerState, PointerRoutedEventArgs e)
        {
            HandleInput((IFiniteStateMachine state) => { return state.HandleInput(pointerState, e); });
        }

        public void HandleInput(KeyBoardState keyboardState, KeyEventArgs args)
        {
            HandleInput((IFiniteStateMachine state) => { return state.HandleInput(keyboardState, args); });
        }

        public void HandleZoom()
        {
            if(this.activeStates == null) { return; }

            foreach (IFiniteStateMachine state in this.activeStates)
            {
                state.HandleZoom();
            }
        }

        public void HandleScroll()
        {
            if (this.activeStates == null) { return; }

            foreach (IFiniteStateMachine state in this.activeStates)
            {
                state.HandleScroll();
            }
        }
    }
}
