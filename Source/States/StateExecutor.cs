using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace WireFrame.Source.States
{
    class StateExecutor
    {
        public class State
        {
            public FiniteStateMachine fsm;
            public List<object> args;

            public State(FiniteStateMachine fsm, List<object> args)
            {
                this.fsm = fsm;
                this.args = args;
            }
        }

        public enum StateGroup
        {
            HighLight_Pan,
            DrawEllipse,
            DrawRectangle
        }

        private Dictionary<StateGroup, List<State>> stateGroups;
        private List<State> activeStates = null;
        State activeState = null;

        public StateExecutor(List<State> states)
        {
            this.activeStates = states;
        }

        public StateExecutor(Dictionary<StateGroup, List<State>> stateGroups)
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

        public void HandleInput (PointerState pointerState, PointerPoint pointer)
        {
            bool handle = false;

            if(this.activeState != null)
            {
                handle = this.activeState.fsm.HandleInput(this.activeState.args, pointerState, pointer);
                if(!handle)
                {
                    this.activeState = null;
                }
            }
            else if(this.activeStates != null)
            {
                foreach (State state in this.activeStates)
                {
                    handle = state.fsm.HandleInput(state.args, pointerState, pointer);
                    if (handle)
                    {
                        this.activeState = state;
                        break;
                    }
                }
            }
            
        }
    }
}
