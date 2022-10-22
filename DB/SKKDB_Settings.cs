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
        public string DBDatabase { get; set; } = "";

        public string SQLDatabase { get; set; } = "";
        public string SQLHost { get; set; } = "";
        public string SQLUser { get; set; } = "";
        public string SQLPass { get; set; } = "";

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
                ret += (DBDatabase != "") ? $"Database={DBDatabase};" : "";
                return ret;
            }
        }

        public string ConnectionStringSQL
        {
            get
            {
                string ret = "";
                ret += (SQLHost != "") ? $"server={((Environment.MachineName.ToUpper() == SQLHost.ToUpper()) ? "localhost" : SQLHost)};" : "";
                ret += (SQLDatabase != "") ? $"database={SQLDatabase};" : "";
                ret += (SQLUser != "") ? $"uid={SQLUser};" : "";
                ret += (SQLPass != "") ? $"pwd={SQLPass};" : "";
                return ret;
            }
        }
    }
}
