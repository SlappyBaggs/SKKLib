using SKKLib.Handlers.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Table
    {
        public Table(string name)
        {
            if (name.Contains(" ")) Report.AddReport($"Table '{name}' contains a space.");
            if (name.Contains("-")) Report.AddReport($"Table '{name}' contains a '-'.");
            TableName = name;
        }

        public string TableName { get; private set; }
        public string TableNameOdbc { get => TableName.Contains(" ") ? $"[{TableName}]" : TableName; }
        public string TableNameSQL { get => TableName.Replace(" ", "_").Replace("-", "_"); }

        private List<Column> columns = new List<Column>();
        public List<Column> Columns => columns;
        public void AddColumn(Column c) => columns.Add(c);
        public Column this[int i] => columns[i];
        
        private List<Index> indices = new List<Index>();
        public List<Index> Indices => indices;

        public string GetPrimaryKey(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            string primaryKeyCols = "";
            foreach(Index i in indices) if(i.IndexName == "PrimaryKey") primaryKeyCols += $"{i.ColumnName},";
            if (primaryKeyCols.Length == 0) return "";
            return $"PRIMARY KEY ({primaryKeyCols.Remove(primaryKeyCols.LastIndexOf(','), 1)}),{nl}";
        }

        private Dictionary<string, List<string>> indexDict = new Dictionary<string, List<string>>();

        public void AddIndex(Index i)
        {
            string name = (i.IndexName == "PrimaryKey") ? "PRIMARY KEY" : i.IndexName;
            if(!indexDict.ContainsKey(name)) indexDict[name] = new List<string>();
            indexDict[name].Add(i.ColumnName);
        }

        private string GetIndexString(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            string ret = "";
            foreach (string key in indexDict.Keys)
            {
                if (key.StartsWith("{")) continue;
                ret += $"{((key == "PRIMARY KEY") ? key : $"INDEX {Column.FixColumnNameSQL(key)}")} (";
                string cols = "";
                foreach (string col in indexDict[key]) cols += $"{Column.FixColumnNameSQL(col)},";
                cols = cols.Substring(0, cols.Length - 1);
                ret += $"{cols}),{nl}";
            }
            return ret;
        }

        public override string ToString() => TableName;

        public string ToRaw(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            string ret = $"{TableName} ({nl}";
            foreach (Column c in columns) ret += $"\t{c.ToRaw(useNL)}";
            ret += GetIndexString(useNL);
            //foreach (Index i in indices) ret += $"\t{i.ToRaw(useNL)}";
            return ret.Remove(ret.LastIndexOf(','), 1) + $"){Environment.NewLine}";
        }

        public string ToSQL(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            string ret = $"CREATE TABLE {TableNameSQL}({nl}";
            foreach (Column col in columns) ret += col.ToSQL(useNL);
            ret += GetIndexString(useNL);
            /*
            ret += GetPrimaryKey(useNL);
            foreach (Index i in indices)
            {
                if (i.IndexName.StartsWith("{")) continue;
                if (i.IndexName != "PrimaryKey") ret += i.ToSQL(useNL);
            }
            ret = ret.Remove(ret.LastIndexOf(','), 1);
            return ret + $");{Environment.NewLine}";*/
            return ret.Remove(ret.LastIndexOf(','), 1) + $"){Environment.NewLine}";
        }
    }
}
