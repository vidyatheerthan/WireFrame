using deVoid.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using WireFrame.DrawArea.Misc;

namespace WireFrame.DrawArea.States
{
    public sealed class StateExecutor
    {
        public enum State
        {
            Select_Pan,
            SelectMoveResize_Pan_Focus,
            Resize,
            Move,
            Rotate,
            DrawEllipse,
            DrawRectangle
        }

        private static readonly StateExecutor instance = new StateExecutor();
        public static StateExecutor Instance { get => instance; }
        private Dictionary<State, List<IFiniteStateMachine>> stateList;
        private List<IFiniteStateMachine> activeStates = null;
        IFiniteStateMachine activeState = null;

        static StateExecutor() { }

        private StateExecutor()
        {
            this.stateList = new Dictionary<State, List<IFiniteStateMachine>>();

            Signals.Get<ChangeToState>().AddListener(SelectState);
        }

        public void AddState(State state, List<IFiniteStateMachine> stateObjs)
        {
            this.stateList[state] = stateObjs;
        }

        private void SelectState(State state)
        {
            if(this.stateList != null && this.stateList.ContainsKey(state) && this.stateList[state] != null && this.stateList[state].Count > 0)
            {
                this.activeStates = this.stateList[state];
                Debug.WriteLine("Active state: " + state);
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
