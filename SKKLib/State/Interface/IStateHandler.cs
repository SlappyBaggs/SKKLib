using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.State.Interface
{
    public interface IStateHandler
    {
        IStateRoot GetStateValue(string stateName);
        bool SetStateValue(string stateName, IStateRoot value, bool autoReg);



        List<string> GetRegisteredStates();
        List<(string State, IStateRoot Value)> GetRegisteredStatesAndValues();

        bool IsStateRegistered(string stateName);
        bool RegisterState(string stateName, IStateRoot stateValue);
        void RegisterStates(List<string> stateNames, IStateRoot stateValue);
        bool UnRegisterState(string stateName);



        bool IsListenerRegistered(string stateName, IStateListener listener);
        bool RegisterListener(string stateName, IStateListener listener, bool autoReg);
        void RegisterListener(List<string> stateNames, IStateListener listener, bool autoReg);
        bool UnRegisterListener(string stateName, IStateListener listener);


        void RegisterStateAndListener(string stateName, IStateRoot stateValue, IStateListener listener);
    }
}
