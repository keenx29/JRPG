 using System;
using System.Collections.Generic;
using System.Text;
using JRPG.Abstract;

namespace JRPG
{
    public class Engine
    {
        private readonly Stack<IEngineState> _states;
        public bool IsRunning { get; private set; }
        public Engine()
        {
            _states = new Stack<IEngineState>();
            IsRunning = true;
        }
        public void Quit()
        {
            IsRunning = false;
        }
        public void PushState(IEngineState state)
        {
            if (_states.Count > 0)
            {
                _states.Peek().Deactivate();
            }
            _states.Push(state);
            state.Activate();
        }
        public void PopState(IEngineState state)
        {
            if (_states.Count == 0)
            {
                throw new InvalidOperationException("No states left in the stack to pop.");
            }
            if (state != _states.Peek())
            {
                throw new InvalidOperationException("The state you are trying to pop is not on the top of the stack.");
            }
            _states.Pop();
            state.Deactivate();
            state.Dispose();
            if (_states.Count>0)
            {
                _states.Peek().Activate();
            }
        }
        public void SwitchState(IEngineState state)
        {
            if (_states.Count>0)
            {
                var oldState = _states.Pop();
                oldState.Deactivate();
                oldState.Dispose();
            }
            _states.Push(state);
            state.Activate();
        }
        public void ProcessInput(ConsoleKeyInfo key)
        {
            if (_states.Count > 0)
            {
                _states.Peek().ProcessInput(key);
            }
        }
    }
}
