using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKKLib.State.Data;

namespace SKKLib.State.Interface
{
    public interface IStateListener
    {
        void StateChanged(string stateName, IStateRoot newState);
        
        // A state we were registered to was unregistered
        void StateUnregistered(string stateName);
        
        // We have been unregistered from a state we were registered to
        void ListenerUnregistered(string stateName);
    }
}
