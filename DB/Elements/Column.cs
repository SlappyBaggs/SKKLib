using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SKKLib.DB.Elements
{
    public class Column
    {
        public static string FixColumnNameSQL(string cn)
        {
            if (cn.Contains(" ")) cn = cn.Replace(" ", "");
            if (Report.ReservedWords.Contains(cn.ToUpper())) cn = $"my{cn}";
            return cn;
        }

        public Column(Table t, string columnName, string dataType, int columnSize, int bufferLength, int decimalDigits, int numPrecRadix, bool nullable, int charOctetLength)
        {
            myTable = t;
            if (columnName.Contains(" ")) Report.AddReport($"Column '{columnName}' in Table '{t.TableName}' contains a space.");
            if (Report.ReservedWords.Contains(columnName.ToUpper())) Report.AddReport($"Column '{columnName}' in Table '{t.TableName}' is a reserved word.");
            ColumnName = columnName;
            DataType = dataType;
            ColumnSize = columnSize;
            BufferLength = bufferLength;
            DecimalDigits = decimalDigits;
            NumPrecRadix = numPrecRadix;
            Nullable = nullable;
            CharOctetLength = charOctetLength;
        }

        private Table myTable = null;

        public string ColumnName { get; private set; }
        public string ColumnNameOdbc { get => (ColumnName.Contains(" "))?$"[{ColumnName}]":ColumnName; }
        public string ColumnNameSQL { get => FixColumnNameSQL(ColumnName); }

        public string DataType { get; private set; }
        public int ColumnSize { get; private set; }
        public int BufferLength { get; private set; }
        public int DecimalDigits { get; private set; }
        public int NumPrecRadix { get; private set; }
        public bool Nullable { get; private set; }
        public int CharOctetLength { get; private set; }

        public override string ToString() => ColumnName;

        public string ToRaw(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            return $"{ColumnName}, {DataType}, {ColumnSize}, {BufferLength}, {DecimalDigits}, {NumPrecRadix}, {Nullable}, {CharOctetLength}{nl}";
        }

        public string ToSQL(bool useNL = false)
        {
            string nl = useNL ? Environment.NewLine : "";
            string ret = $"{ColumnNameSQL} ";
            switch (DataType)
            {
                case "COUNTER":
                    ret += "INTEGER";
                    break;
                case "LONGCHAR":
                    ret += "MEDIUMTEXT";
                    break;
                case "LONGBINARY":
                    ret += "BLOB";
                    break;
                default:
                    ret += DataType;
                    break;
            };
            
            switch (DataType)
            {
                case "CHAR":
                case "VARCHAR":
                case "BINARY":
                case "VARBINARY":
                case "TEXT":
                case "BLOB":
                case "BIT":
                    ret += $"({ColumnSize})";
                    break;
                case "TINYINT":
                case "SMALLINT":
                case "MEDIUMINT":
                case "INT":
                case "COUNTER":
                case "INTEGER":
                case "BIGINT":
                    ret += $"({BufferLength})";
                    break;
                case "FLOAT":
                case "DOUBLE":
                case "DOUBLE PRECISION":
                case "DECIMAL":
                case "DEC":
                    if((BufferLength > 0) && (DecimalDigits >= 0)) ret += $"({BufferLength},{DecimalDigits})";
                    break;
            }
            if (!Nullable) ret += " NOT NULL";
            if (DataType == "COUNTER") ret += " AUTO_INCREMENT";
            return ret + $",{nl}";
        }
    }
}
