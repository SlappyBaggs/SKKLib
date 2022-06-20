using SKKLib.State.Interface;

namespace SKKLib.State
{
    public class StateListener : IStateListener
    {
        public virtual void ListenerUnregistered(string stateName) { }

        public virtual void StateChanged(string stateName, IStateRoot newState) { }

        public virtual void StateUnregistered(string stateName) { }
    }
}
