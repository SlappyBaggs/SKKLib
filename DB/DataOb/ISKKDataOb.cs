using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.DataOb
{
    // SaveToDB is the only function that I want exposed here...
    // Loading will be handled internally by the implementation

    public interface ISKKDataOb
    {
        void SaveToDB();
    }
}
