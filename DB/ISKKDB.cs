using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB
{
    public interface ISKKDB
    {
        bool Loaded { get; }
        bool IsOpen { get; }
        string MyConnString { get; }

        void Load(string file = null);
        void Open(bool ino = false);
        void Close();
        void ExecuteSql(string sql);
        IDataReader ExecuteReader(string sql);
        IDbCommand GetCommand(string sql);
        IDataAdapter GetAdapter(string sql);
    }
}
