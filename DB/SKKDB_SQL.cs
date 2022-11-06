using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Data.Odbc;
using System.Data;
using System.Data.Common;
using static SKKLib.Console.SKKConsole;
using SKKLib.SystemLib;
using System.Windows.Forms.VisualStyles;

namespace SKKLib.DB
{
    public class DBObSQL : ISKKDB
    {
        public static readonly string sqlDateFormat = "yyyy-MM-dd HH:mm:ss";

        private static string DBGName => "DBobSQL";

        private DBSettings myDBSettings = null;
        private MySqlConnection myConn;
        
        public bool Loaded { get; private set; } = false;
        
        public bool IsOpen { get => myConn?.State == System.Data.ConnectionState.Open; }

        public DBObSQL(string fileName = null)
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "SKKLib.DBobSQL.Constructor");
            if (fileName != null) Load(fileName);
        }

        public string MyConnString { get => myDBSettings?.ConnectionStringSQL; }

        public DbConnection MyConnection { get => myConn; }

        public void Load(string file = null)
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", $"SKKLib.DBObSQL.Load({file}");
            //DBG($"DBObSQL::Load Config File: {file}");
            Loaded = false;
            if (file == null) return;
            try
            {
                myDBSettings = JsonConvert.DeserializeObject<DBSettings>(File.ReadAllText(file));
            }
            catch (Exception ex)
            {
                Controls.Forms.MessageBox.ShowMessage(ex.Message, "DBObSQL Exception");
                return;
            }
            Loaded = true;
        }

        public void Open(bool ino = false)
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "SKKLib.DBObSQL.Open");
            if (!Loaded) return;    // Throw??
            if (ino && IsOpen) return;     // Throw?

            try
            {
                myConn = new MySqlConnection(MyConnString);
                myConn.Open();
            }
            catch (Exception ex)
            {
                Controls.Forms.MessageBox.ShowMessage(ex.Message, "DBObSQL Exception");
                return;
            }
        }
        public void Close() => myConn?.Close();

        public void ExecuteSql(string sql) => new MySqlCommand(sql, myConn).ExecuteNonQuery();

        /*MySqlDataReader*/
        //public IDataReader ExecuteReader(string sql) => new MySqlCommand(sql, myConn).ExecuteReader();
        public IDataReader ExecuteReader(string sql)
        {
            SKKDebugDepth.Msg($"{DBGName}", $"DBObSQL::ExecuteReader: {sql}");
            return new MySqlCommand(sql, myConn).ExecuteReader();
        }

        /*MySqlCommand*/
        public IDbCommand GetCommand(string sql) => new MySqlCommand(sql, myConn);

        public IDataAdapter GetAdapter(string sql) => new MySqlDataAdapter(sql, myConn);

        public DataTable GetSchema() => myConn.GetSchema();
        public DataTable GetSchema(string collectionName) => myConn.GetSchema(collectionName);
        public DataTable GetSchema(string collectionName, string[] restrictionValues) => myConn.GetSchema(collectionName, restrictionValues);
    }
}
