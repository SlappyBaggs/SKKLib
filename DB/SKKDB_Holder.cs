using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB
{
    public class SKKDBHolder : IDisposable, ISKKDBBase
    {
        private ISKKDB myDBOb;

        public static SKKDBHolder Hold(ISKKDB dbOb) => new SKKDBHolder(dbOb);

        private bool closeOnDispose = true;

        public SKKDBHolder(ISKKDB dbOb)
        {
            myDBOb = dbOb;
            closeOnDispose = !myDBOb.IsOpen;
            myDBOb.Open(true);
        }

        public void Dispose()
        {
            if (closeOnDispose)
            {
                myDBOb.Close();
            }
        }

        // ISKKDBBase Implementation
        public void ExecuteSql(string sql) => myDBOb.ExecuteSql(sql);
        public IDataReader ExecuteReader(string sql) => myDBOb.ExecuteReader(sql);
        public IDbCommand GetCommand(string sql) => myDBOb.GetCommand(sql);
        public IDataAdapter GetAdapter(string sql) => myDBOb.GetAdapter(sql);

        public DbConnection MyConnection { get => myDBOb.MyConnection; }
        public DataTable GetSchema() => myDBOb.GetSchema();
        public DataTable GetSchema(string collectionName) => myDBOb.GetSchema(collectionName);
        public DataTable GetSchema(string collectionName, string[] restrictionValues) => myDBOb.GetSchema(collectionName, restrictionValues);
    }
}
