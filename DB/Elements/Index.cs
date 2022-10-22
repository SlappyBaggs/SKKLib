using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Index
    {
        public Index(string indexName, string columnName, string aorD, bool nonUnique)
        {
            IndexName = indexName;
            ColumnName = columnName;
            AorD = aorD;
            NonUnique = nonUnique;
        }

        public string IndexName { get; private set; }
        public string ColumnName { get; private set; }
        public string AorD { get; private set; }
        public bool NonUnique { get; private set; }

        public override string ToString() => IndexName;
    }
}
