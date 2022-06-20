using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKKLib.State.Interface;

namespace SKKLib.State.Data
{
    public delegate void StateChangedHandler(string stateName, IStateRoot newValue);

    public class StateData
    {
    
    }
}
