using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.State.Interface
{
    public interface IStateRoot { }

    public interface IState<T> : IStateRoot
    {
        T GetStateValue();
    }
}
