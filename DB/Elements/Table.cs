using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Table
    {
        public Table(string name) => TableName = name;

        public string TableName { get; private set; }
        
        private List<Column> columns = new List<Column>();
        public List<Column> Columns => columns;
        public void AddColumn(Column c) => columns.Add(c);
        public Column this[int i] => columns[i];

        private List<Index> indices = new List<Index>();
        public List<Index> Indices => indices;
        public void AddIndex(Index i) => indices.Add(i);

        public override string ToString() => TableName;
    }
}
