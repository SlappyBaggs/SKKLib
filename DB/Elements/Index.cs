using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Index
    {
        public Index(Table t,  string indexName, string columnName, string aorD, bool nonUnique)
        {
            myTable = t;
            if (indexName.StartsWith("{")) Report.AddReport($"Index '{indexName}' in Table '{t.TableName}' is all funky for some reason.");
            IndexName = indexName;
            if (columnName.Contains(" ")) Report.AddReport($"Column '{columnName}' in Index '{indexName}' in Table '{t.TableName}' contains a space.");
            if (Report.ReservedWords.Contains(columnName.ToUpper())) Report.AddReport($"Column '{columnName}' in Index '{indexName}' in Table '{t.TableName}' is a reserved word.");
            ColumnName = (columnName.ToUpper() == "INDEX") ? $"my{columnName}" : columnName;
            AorD = aorD;
            NonUnique = nonUnique;
        }

        private Table myTable = null;

        public string IndexName { get; private set; }
        public string ColumnName{ get; private set; }
        public string ColumnNameOdbc { get => (ColumnName.Contains(" ")) ? $"[{ColumnName}]" : ColumnName; }
        public string ColumnNameSQL { get => Column.FixColumnNameSQL(ColumnName); }

        public string AorD { get; private set; }
        public bool NonUnique { get; private set; }

        public override string ToString() => IndexName;

        public string ToRaw(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            return $"{((IndexName == "PrimaryKey") ? "PRIMARY KEY" : $"INDEX {IndexName}")} ({ColumnName}),{nl}";
        }
        public string ToSQL(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            return $"{((IndexName == "PrimaryKey") ? "PRIMARY KEY" : $"INDEX {IndexName}")} ({ColumnNameSQL}),{nl}";
        }
    }
}
