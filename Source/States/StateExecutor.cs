using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace WireFrame.Source.States
{
    class StateExecutor
    {
        public enum StateGroup
        {
            Selection_Pan,
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

        public void HandleInput (PointerState pointerState, PointerRoutedEventArgs e)
        {
            bool handle = false;

            if(this.activeState != null)
            {
                foreach (IFiniteStateMachine state in this.activeStates)
                {
                    if (state == this.activeState)
                    {
                        handle = this.activeState.HandleInput(pointerState, e);
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
                    handle = state.HandleInput(pointerState, e);
                    if (handle)
                    {
                        this.activeState = state;
                        break;
                    }
                }
            }
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
