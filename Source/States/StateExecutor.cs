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

        private List<State> states;
        State activeState = null;

        public StateExecutor(List<State> states)
        {
            this.states = states;
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
            else
            {
                foreach (State state in this.states)
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
