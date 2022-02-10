using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using WireFrame.Misc;

namespace WireFrame.States
{
    class StateExecutor
    {
        public enum StateGroup
        {
            Selection_Pan_Focus,
            RotateElement,
            DrawEllipse,
            DrawRectangle
        }

        private Dictionary<StateGroup, List<IFiniteStateMachine>> stateGroups;
        private List<IFiniteStateMachine> activeStates = null;
        IFiniteStateMachine activeState = null;

        public StateExecutor(List<IFiniteStateMachine> states)
        {
            this.activeStates = states;
        }

        public StateExecutor(Dictionary<StateGroup, List<IFiniteStateMachine>> stateGroups)
        {
            this.stateGroups = stateGroups;
        }

        public bool SelectStateGroup(StateGroup stateGroup)
        {
            if(this.stateGroups != null && this.stateGroups.ContainsKey(stateGroup) && this.stateGroups[stateGroup] != null && this.stateGroups[stateGroup].Count > 0)
            {
                this.activeStates = this.stateGroups[stateGroup];
                return true;
            }

            return false;
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
            foreach (IFiniteStateMachine state in this.activeStates)
            {
                state.HandleZoom();
            }
        }
    }
}
