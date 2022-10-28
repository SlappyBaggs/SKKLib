using System;
using System.IO;
using System.Data.Odbc;
using System.Data;
using System.Data.Common;
using System.Windows.Forms.VisualStyles;

using SKKLib.SystemLib;
using SKKLib.DB.Exceptions;
using static SKKLib.Console.SKKConsole;

using Newtonsoft.Json;

using MySql.Data.MySqlClient;

namespace SKKLib.DB
{
    public class DBObOdbc : ISKKDB
    {
        public static readonly string sqlDateFormat = "yyyy-MM-dd HH:mm:ss";

        private DBSettings myDB = null;
        private OdbcConnection myConn;

        public bool Loaded { get; private set; } = false;
        
        public bool IsOpen { get => myConn?.State == ConnectionState.Open; }

        public DBObOdbc(string fileName = null)
        {
            using (var bench = SKKBench.Get("SKKLib.DBobOdbc.Constructor"))
            {
                    if (fileName != null) Load(fileName);
            }
        }

        public string MyConnString { get => myDB?.ConnectionString; }

        public DbConnection MyConnection { get => myConn; }

        public void Load(string file = null)
        {
            using (var bench = SKKBench.Get($"SKKLib.DBObOdbc.Load({file}", false))
             {
                Loaded = false;
                if (file == null) return;
                try
                {
                    myDB = JsonConvert.DeserializeObject<DBSettings>(File.ReadAllText(file));
                }
                catch(Exception /*x*/)
                {
                    return;
                }
                Loaded = true;
            }
        }

        public void Open(bool ino = false)
        {
            using (var bench = SKKBench.Get("SKKLib.DBObOdbc.Open", false))
            {
                if (!Loaded) return;    // Throw??
                if (ino && IsOpen) return;     // Throw?

                try
                {
                    myConn = new OdbcConnection(MyConnString);
                    myConn.Open();
                }
                catch (Exception ex)
                {
                    throw new SKKDBException(ex.Message);
                    //Controls.Forms.MessageBox.ShowMessage(ex.Message, "DBOdbc Exception");
                }
            }
        }

        public void Close() => myConn?.Close();

        public void ExecuteSql(string sql) => new OdbcCommand(sql, myConn).ExecuteNonQuery();

        /*OdbcDataReader*/
        //public IDataReader ExecuteReader(string sql) => new OdbcCommand(sql, myConn).ExecuteReader();
        public IDataReader ExecuteReader(string sql)
        {
            //DBG($"DBObOdbc::ExecuteReader: {sql}");
            return new OdbcCommand(sql, myConn).ExecuteReader();
        }

        /*OdbcCommand*/
        public IDbCommand GetCommand(string sql) => new OdbcCommand(sql, myConn);

        public IDataAdapter GetAdapter(string sql) => new OdbcDataAdapter(sql, myConn);

        public DataTable GetSchema() => myConn.GetSchema();
        public DataTable GetSchema(string collectionName) => myConn.GetSchema(collectionName);
        public DataTable GetSchema(string collectionName, string[] restrictionValues) => myConn.GetSchema(collectionName, restrictionValues);
    }
}
