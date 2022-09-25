using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB
{
    public static class AccessToDataSet
    {
        private const string connString = "Provider=Microsoft.Jet.OLEDB.4.0;";

        public static void ConvertMDBtoXML(string mdb)
        {
            DataSet dataSet = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(connString + $";Data Source='{mdb}';"))
            {
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow dataTableRow in schemaTable.Rows)
                {
                    string tableName = dataTableRow["Table_Name"].ToString();
                    if (!tableName.StartsWith("~", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FillTable(dataSet, conn, tableName);
                    }
                }
            }

            string name = mdb.ToLowerInvariant();
            dataSet.WriteXmlSchema(name.Replace(".mdb", ".schema.xml"));
            dataSet.WriteXml(name.Replace(".mdb", ".xml"));
        }

        private static void FillTable(DataSet dataSet, OleDbConnection conn, string tableName)
        {
            DataTable dataTable = dataSet.Tables.Add(tableName);
            using (OleDbCommand readRows = new OleDbCommand("SELECT * from " + tableName, conn))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(readRows);
                adapter.Fill(dataTable);
            }
        }
    }
}
