using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Data.Odbc;
using System.Data;
using System.Data.Common;
using static SKKLib.Console.SKKConsole;

namespace SKKLib.DB
{
    public class DBSettings
    {
        public string DBDriver { get; set; } = "";
        public string DBProvider { get; set; } = "";
        public string DBQ { get; set; } = "";
        public string DBSource { get; set; } = "";
        public string DBName { get; set; } = "";
        public string DBHost { get; set; } = "";
        public string DBUser { get; set; } = "";
        public string DBPass { get; set; } = "";

        public string ConnectionString
        {
            get
            {
                string ret = "";
                ret += (DBDriver != "") ? $"Driver={DBDriver};" : "";
                ret += (DBProvider != "") ? $"Provider={DBProvider};" : "";
                ret += (DBQ != "") ? $"DBQ={DBQ};" : "";
                ret += (DBSource != "") ? $"DataSource={DBSource};" : "";
                ret += (DBHost != "") ? $"server={((Environment.MachineName.ToUpper() == DBHost.ToUpper()) ? "localhost" : DBHost)};" : "";
                ret += (DBName != "") ? $"database={DBName};" : "";
                ret += (DBUser != "") ? $"uid={DBUser};" : "";
                ret += (DBPass != "") ? $"pwd={DBPass};" : "";
                return ret;
            }
        }
    }

    public class DBObSQL : ISKKDB
    {
        public static readonly string sqlDateFormat = "yyyy-MM-dd HH:mm:ss";

        private DBSettings myDB = null;
        private MySqlConnection myConn;

        public bool Loaded { get; private set; } = false;
        
        public bool IsOpen { get => myConn?.State == System.Data.ConnectionState.Open; }

        public DBObSQL(string fileName = null)
        {
            //DBG($"DBObSQL::Constructor: {fileName}");
            if (fileName != null) Load(fileName);
        }
            
        public string MyConnString { get => myDB?.ConnectionString; }

        public void Load(string file = null)
        {
            //DBG($"DBObSQL::Load Config File: {file}");
            Loaded = false;
            if (file == null) return;
            try
            {
                myDB = JsonConvert.DeserializeObject<DBSettings>(File.ReadAllText(file));
            }
            catch
            {
                return;                
            }
            Loaded = true;
        }

        public void Open(bool ino = false)
        {
            //DBG($"DBObSQL::Open DB, ino={ino}");
            if (!Loaded) return;    // Throw??
            if (ino && IsOpen) return;     // Throw?

            try
            {
                myConn = new MySqlConnection(myDB.ConnectionString);
                myConn.Open();
            }
            catch(Exception ex)
            {
                SKKLib.Controls.Forms.MessageBox.ShowMessage(ex.Message, "Exception");
                return;
            }
        }

        public void Close() => myConn?.Close();

        public void ExecuteSql(string sql) => new MySqlCommand(sql, myConn).ExecuteNonQuery();

        /*MySqlDataReader*/
        //public IDataReader ExecuteReader(string sql) => new MySqlCommand(sql, myConn).ExecuteReader();
        public IDataReader ExecuteReader(string sql)
        {
            //DBG($"DBObSQL::ExecuteReader: {sql}");
            return new MySqlCommand(sql, myConn).ExecuteReader();
        }

        /*MySqlCommand*/
        public IDbCommand GetCommand(string sql) => new MySqlCommand(sql, myConn);

        public IDataAdapter GetAdapter(string sql) => new MySqlDataAdapter(sql, myConn);
    }

    public class DBObOdbc : ISKKDB
    {
        public static readonly string sqlDateFormat = "yyyy-MM-dd HH:mm:ss";

        private DBSettings myDB = null;
        private OdbcConnection myConn;

        public bool Loaded { get; private set; } = false;
        
        public bool IsOpen { get => myConn?.State == System.Data.ConnectionState.Open; }

        public DBObOdbc(string fileName = null)
        {
            //DBG($"DBobOdbc::Constructor: {fileName}");
            if (fileName != null) Load(fileName);
        }

        public string MyConnString { get => myDB?.ConnectionString; }

        public void Load(string file = null)
        {
            //DBG($"DBObOdbc::Load DB {file}");
            Loaded = false;
            if (file == null) return;
            try
            {
                myDB = JsonConvert.DeserializeObject<DBSettings>(File.ReadAllText(file));
            }
            catch
            {
                return;
            }
            Loaded = true;
        }

        public void Open(bool ino = false)
        {
            //DBG($"DBObOdbc::Open DB, ino={ino}");
            if (!Loaded) return;    // Throw??
            if (ino && IsOpen) return;     // Throw?

            try
            {
                myConn = new OdbcConnection(myDB.ConnectionString);
                myConn.Open();
            }
            catch (Exception ex)
            {
                SKKLib.Controls.Forms.MessageBox.ShowMessage(ex.Message, "Exception");
                return;
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
    }
}
