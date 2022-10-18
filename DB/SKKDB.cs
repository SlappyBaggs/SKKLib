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
        public string DBDSN { get; set; } = "";

        public string ConnectionString
        {
            get
            {
                string ret = "";
                ret += (DBDriver != "") ? $"Driver={DBDriver};" : "";
                ret += (DBProvider != "") ? $"Provider={DBProvider};" : "";
                ret += (DBQ != "") ? $"Dbq={DBQ};" : "";
                ret += (DBSource != "") ? $"Data Source={DBSource};" : "";
                ret += (DBHost != "") ? $"server={((Environment.MachineName.ToUpper() == DBHost.ToUpper()) ? "localhost" : DBHost)};" : "";
                ret += (DBName != "") ? $"database={DBName};" : "";
                ret += (DBUser != "") ? $"uid={DBUser};" : "";
                ret += (DBPass != "") ? $"pwd={DBPass};" : "";
                ret += (DBDSN != "") ? $"DSN={DBDSN};" : "";
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
            using (var bench = SKKBench.Get("SKKLib.DBobSQL.Constructor"))
            {
                if (fileName != null) Load(fileName);
            }
        }

        public string MyConnString { get => myDB?.ConnectionString; }

        public void Load(string file = null)
        {
            using (var bench = SKKBench.Get($"SKKLib.DBObSQL.Load({file}"))
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
        }

        public void Open(bool ino = false)
        {
            using (var bench = SKKBench.Get("SKKLib.DBObSQL.Open", false))
            {
                if (!Loaded) return;    // Throw??
                if (ino && IsOpen) return;     // Throw?

                try
                {
                    myConn = new MySqlConnection(myDB.ConnectionString);
                    myConn.Open();
                }
                catch (Exception ex)
                {
                    Controls.Forms.MessageBox.ShowMessage(ex.Message, "Exception");
                    return;
                }
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

        public DataTable GetSchema() => myConn.GetSchema();
        public DataTable GetSchema(string collectionName) => myConn.GetSchema(collectionName);
        public DataTable GetSchema(string collectionName, string[] restrictionValues) => myConn.GetSchema(collectionName, restrictionValues);
    }

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
            //using (var bench = SKKBench.Get("SKKLib.DBObOdbc.Open", false))
            //{
                if (!Loaded) return;    // Throw??
                if (ino && IsOpen) return;     // Throw?

                try
                {
                    myConn = new OdbcConnection(myDB.ConnectionString);
                    myConn.Open();
                }
                catch (Exception ex)
                {
                    Controls.Forms.MessageBox.ShowMessage(ex.Message, "Exception");
                    return;
                }
            //}
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

    public class SKKDBDataReader: IDataReader
    {
        private IDataReader _reader = null;// new OdbcDataReader();
        public SKKDBDataReader(IDataReader _rdr)
        {
            _reader = _rdr;
        }
        public object this[int i] => _reader[i];
        public object this[string name] => _reader[name];
        public int Depth => _reader.Depth;
        public bool IsClosed => throw new NotImplementedException();
        public int RecordsAffected => throw new NotImplementedException();
        public int FieldCount => throw new NotImplementedException();
        public void Close() => _reader.Close();
        public void Dispose() => _reader.Dispose();
        public bool GetBoolean(int i) => _reader.GetBoolean(i);
        public byte GetByte(int i) => _reader.GetByte(i);
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        public char GetChar(int i) => _reader.GetChar(i);
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        public IDataReader GetData(int i) => _reader.GetData(i);
        public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);
        public DateTime GetDateTime(int i) => _reader.GetDateTime(i);
        public decimal GetDecimal(int i) => _reader.GetDecimal(i);
        public double GetDouble(int i) => _reader.GetDouble(i);
        public Type GetFieldType(int i) => _reader.GetFieldType(i);
        public float GetFloat(int i) => _reader.GetFloat(i);
        public Guid GetGuid(int i) => _reader.GetGuid(i);
        public short GetInt16(int i) => _reader.GetInt16(i);
        public int GetInt32(int i) => _reader.GetInt32(i);
        public long GetInt64(int i) => _reader.GetInt64(i);
        public string GetName(int i) => _reader.GetName(i);
        public int GetOrdinal(string name) => _reader.GetOrdinal(name);
        public DataTable GetSchemaTable() => _reader.GetSchemaTable();
        public string GetString(int i) => _reader.GetString(i);
        public object GetValue(int i) => _reader.GetValue(i);
        public int GetValues(object[] values) => _reader.GetValues(values);
        public bool IsDBNull(int i) => _reader.IsDBNull(i);
        public bool NextResult() => _reader.NextResult();
        public bool Read() => _reader.Read();
    }

}
