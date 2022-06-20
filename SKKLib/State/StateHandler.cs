using System.Collections.Generic;
using System.Linq;
using SKKLib.State.Interface;

namespace SKKLib.State
{
    public class StateHandler : IStateHandler
    {
        // Value for key 'stateName' is a tuple of an 'IStateRoot' and a List of 'IStateListeners' registered to that state...
        private Dictionary<string, (IStateRoot Value, List<IStateListener> Listeners)> allStates_ = new Dictionary<string, (IStateRoot Value, List<IStateListener> Listeners)>();



        public List<string> GetRegisteredStates() => allStates_.Keys.ToList();

        public List<(string State, IStateRoot Value)> GetRegisteredStatesAndValues()
        {
            List<(string State, IStateRoot Value)> ret = new List<(string State, IStateRoot Value)>();
            foreach (string key in GetRegisteredStates()) ret.Add((State: key, Value: allStates_[key].Value));
            return ret;
        }



        public bool IsStateRegistered(string stateName) => allStates_.ContainsKey(stateName);
        public bool RegisterState(string stateName, IStateRoot stateValue)
        {
            if (!IsStateRegistered(stateName))
            {
                // State isn't already registered...  add it to the list!
                allStates_[stateName] = (Value: stateValue, Listeners: new List<IStateListener>());
                return true;
            }

            // State was already registered...  do nothing.
            return false;
        }
        public void RegisterStates(List<string> stateNames, IStateRoot stateValue)
        {
            foreach(string stateName in stateNames) RegisterState(stateName, stateValue);
        }
        public bool UnRegisterState(string stateName)
        {
            if (IsStateRegistered(stateName))
            {
                foreach (IStateListener sl in allStates_[stateName].Listeners)
                {
                    sl.StateUnregistered(stateName);
                }

                allStates_.Remove(stateName);
                return true;
            }
            
            // State wasn't registered...
            return false;
        }

        
        
        // An object doesn't need to be a regisrered listener of a state to query its value
        public IStateRoot GetStateValue(string stateName) => allStates_.ContainsKey(stateName) ? allStates_[stateName].Value : null;
        public bool SetStateValue(string stateName, IStateRoot value, bool autoReg = false)
        {
            if (IsStateRegistered(stateName))
            {
                if (GetStateValue(stateName) != value)
                {
                    allStates_[stateName] = (Value: value, Listeners: allStates_[stateName].Listeners); ;

                    // the value of 'stateName' has changed...
                    foreach (IStateListener sl in allStates_[stateName].Listeners)
                    {
                        sl.StateChanged(stateName, value);
                    }
                }
                return true;
            }
            else
            {
                // State wasn't registered... if 'autoReg' was set, then let's add it...
                if (autoReg)
                {
                    RegisterState(stateName, value);
                    return true;
                }

                // State was unregistered, and we did not auto-register it...
                return false;
            }
        }
        
        

        public bool IsListenerRegistered(string stateName, IStateListener listener) => IsStateRegistered(stateName) && allStates_[stateName].Listeners.Contains(listener);
        public bool RegisterListener(string stateName, IStateListener listener, bool add = false)
        {
            if (!IsStateRegistered(stateName))
            {
                // State isn't registered, check 'add' flag
                if (!add) return false;

                // State isn't registered, 'add' is true, so register 'stateName' with value of 'null'
                RegisterState(stateName, null);
            }
            
            // State is registered, check if listenerRegister has been set yet
            if (IsListenerRegistered(stateName, listener))
                return false;   // Listener was already registered

            // State is registered, listener is not already registered, so registate them
            List<IStateListener> listenList = allStates_[stateName].Listeners;
            listenList.Add(listener);
            allStates_[stateName] = (Value: allStates_[stateName].Value, Listeners: listenList);
            return true;
        }
        public void RegisterListener(List<string> stateNames, IStateListener listener, bool add = false)
        {
            foreach(string stateName in stateNames) RegisterListener(stateName, listener, add);
        }
        public bool UnRegisterListener(string stateName, IStateListener listener)
        {
            if (IsListenerRegistered(stateName, listener))
            {
                List<IStateListener> listenList = allStates_[stateName].Listeners;
                listenList.Remove(listener);
                allStates_[stateName] = (Value: allStates_[stateName].Value, Listeners: listenList);
                return true;
            }

            // Listener was not registered to 'stateName'
            return false;
        }



        public void RegisterStateAndListener(string stateName, IStateRoot stateValue, IStateListener listener)
        {
            RegisterState(stateName, stateValue);
            RegisterListener(stateName, listener);
        }
    }
}
