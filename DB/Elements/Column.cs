using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Column
    {
        public Column(string columnName, string dataType, int columnSize, int bufferLength, int decimalDigits, int numPrecRadix, bool nullable, int charOctetLength)
        {
            ColumnName = columnName;
            DataType = dataType;
            ColumnSize = columnSize;
            BufferLength = bufferLength;
            DecimalDigits = decimalDigits;
            NumPrecRadix = numPrecRadix;
            Nullable = nullable;
            CharOctetLength = charOctetLength;
        }       

        public string ColumnName { get; private set; }
        public string DataType { get; private set; }
        public int ColumnSize { get; private set; }
        public int BufferLength { get; private set; }
        public int DecimalDigits { get; private set; }
        public int NumPrecRadix { get; private set; }
        public bool Nullable { get; private set; }
        public int CharOctetLength { get; private set; }

        public override string ToString() => ColumnName;
    }
}
