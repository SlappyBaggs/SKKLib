using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKKLib.State.Interface;

namespace SKKLib.State
{
    public class State<T> : IState<T>
    {
        private T stateValue_ = default(T);
        
        public State(T t_) => stateValue_ = t_;

        public T GetStateValue() => stateValue_;
    }
}
