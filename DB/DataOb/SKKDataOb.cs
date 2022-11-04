using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SKKLib.SystemLib;
using SKKLib.DB;
using System.Data.Odbc;
using System.Data.Common;

namespace SKKLib.DB.DataOb
{
    public class SKKDataObBase
    {
        public static void SetDataObDB(ISKKDB db) => DataObDB = db;
        protected static ISKKDB DataObDB { get; private set; }

        public static void SetDataObDateFormat(string df) => DataObDateFormat = df;
        protected static string DataObDateFormat { get; private set; }
    }

    public abstract class SKKDataOb<DATAOBTYPE> : SKKDataObBase, ISKKDataOb
    {
        // Switched to an Init call over Constructor to avoid the base object passing data back up to the
        // not-fully-intitialized derived class...
        // Device was given Invoice in its constructor, but the base class went first.  Base class constructor
        // eventually called SetupNewObject() which was hooked by Device.  SetupNewObject() needed data from
        // Invoice, and while Device had been given Invoice, Device's constructor still hadn't been run yet since
        // we're still in the base class constructor's scope/stack-frame.  So Invoice wasn't set to a local member
        // variable in Device, so it had a 'null' value in SetupNewObject()... Init function fixes that by 
        // running after both constructors have been run...

        //protected SKKDataOb(string primKeyString, bool newDataOb, object data = null)
        //{
        //}
        
        protected void InitSKKDataOb(string primKeyString, bool newDataOb/*, object data = null*/)
        {
            PrimaryKeyString = primKeyString;
            NewDataOb = NeedsSave = newDataOb;

            // Only do this once, since it is static
            if (myTables == null) PopulateMyTables();

            // Do this everytime for every instance
            PopulateFieldLists();

            if (NewDataOb) SetupNewObject();
            else LoadObject();
        }

        #region VARIABLES & PROPERTIES
        // Name of Parent class for Debug Identification
        // Meant to be hidden with 'new'
        protected static string DBGName { get; }

        //public static void SetDataObDB(ISKKDB db) => DataObDB = db;
        //protected static ISKKDB DataObDB { get; private set; }
        
        //public static void SetDataObDateFormat(string df) => DataObDateFormat = df;
        //protected static string DataObDateFormat { get; private set; }

        // Is this a new DataOb or an Existing one?
        // Determines how save is done (INSERT vs UPDATE)
        protected bool NewDataOb { get; set; }

        // Went to load the DataOb, there's nothing there!!!
        // If FailedLoad is true, then it treats Saving as if NewDataOb were set to true
        // Should FailedLoad call SetupNewObject()??  Does it already??
        private bool FailedLoad { get; set; } = false;

        public bool NeedsSave { get; set; } = false;

        // The DataOb's default table's primary key value...
        // Defined as string to ease casting/type issues...
        // Most PrimKeys will be ints used as ID values, but
        // if supposed to be an actual string, include single quotes
        // when passed into constructor
        // int ex: PrimaryKeyString = "12345";
        // string ex: PrimaryKeyString = "'STEC'";
        protected string PrimaryKeyString { get; set; }
        #endregion

        #region INIT NEW & LOAD EXISTING
        protected virtual void SetupNewObject() { }

        protected bool ObjectLoading { get; set; } = false;

        protected void LoadObject()
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", $"LoadObject({PrimaryKeyString})");
            ObjectLoading = true;
            PreLoad();
            using (var db = SKKDBHolder.Hold(DataObDB))
            {
                string mySQL = GetLoadQuery();
                dd.Msg($"MYSQL: {mySQL}");

                IDataReader rdr = db.ExecuteReader(mySQL);

                if ((rdr as DbDataReader).HasRows)
                {
                    bool didRead = rdr.Read();

                    TypeInfo ti = GetType().GetTypeInfo();
                    object o;

                    // Loop through the table keys
                    foreach (string key in myFieldLists.Keys)
                    {
                        // Loop though field lists for each table
                        foreach (SKKDataObComponent sc in myFieldLists[key])
                        {
                            try
                            {
                                if (sc.SkipLoad) continue;

                                // Get the value from DB
                                o = rdr[sc.FieldName];

                                // Handle weird casting issues...
                                if ((sc.PropertyType == typeof(Int32)) && (o.GetType() == typeof(Single))) o = Convert.ToInt32(o);
                                o = (o is DBNull) ? null : o;

                                // Set property with loaded DB value
                                dd.Msg($"Setting Property: {sc.PropertyName} [{sc.PropertyType}] to {o} [{((o is null) ? null : o.GetType())}]");
                                ti.GetProperty(sc.PropertyName).SetValue(this, o);
                            }
                            catch (Exception /*x*/)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
                else
                {
                    FailedLoad = true;
                    // Should we call SetupNewObject() here??
                }
            }
            PostLoad();
            ObjectLoading = false;
        }

        protected virtual void PreLoad() { }
        protected virtual void PostLoad() { }

        private string GetLoadQuery()
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "GetLoadQuery()");
            string mySQL = "SELECT ";
            string myFrom1 = " FROM " + new string('(', myTables.Count - 1);
            string myFrom2 = "";
            string myWhere = " WHERE ";
            char tableAlias;

            // Loop through all of this class's tables defined in attributes
            foreach (SKKDataObComponentTable table in myTables)
            {
                // Does we have a field list for this table
                if (myFieldLists.ContainsKey(table.TableName))
                {
                    // Create a unique alias for this table based on index...
                    tableAlias = Convert.ToChar(97 + myTables.FindIndex(x => x.TableName == table.TableName));

                    // Loop through fields and add them to query combined with alias
                    foreach (SKKDataObComponent sc in myFieldLists[table.TableName]) mySQL += $"{tableAlias}.{sc.FieldName},";

                    // Is this table is the default table?
                    if (table.IsDefault)
                    {
                        // Use our key in where constraint
                        myWhere += $"{tableAlias}.{table.TableKey}={PrimaryKeyString};";

                        // Use this table as main query source, and others join to it
                        myFrom1 += $"{table.TableName} {tableAlias}";
                    }
                    else
                    {
                        // Join us to the main query table...
                        myFrom2 += $" LEFT JOIN {table.TableName} {tableAlias} ON {tableAlias}.{table.TableKey}={Convert.ToChar(97 + myTables.FindIndex(x => x.IsDefault))}." +
                            $"{myTables.Find(x => x.IsDefault).TableKey})";
                    }
                }
            }

            // Query will have a trailing comma we don't need, so remove it...
            mySQL = mySQL.Substring(0, mySQL.Length - 1);

            // Construct final query
            mySQL += myFrom1 + myFrom2 + myWhere;

            return mySQL;
        }

        // Pass in a specific table name & newStatus to get a save data for just that table
        // If table name is omitted, returns save data for all tables
        protected Dictionary<string, List<(string PropertyField, string Value)>> GetSaveData(string tableName = "", bool newStatus = false)
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "GetSaveData()");
            Dictionary<string, List<(string PropertyField, string Value)>> ret = new Dictionary<string, List<(string PropertyField, string Value)>>();
            TypeInfo ti = GetType().GetTypeInfo();
            object o;

            // Get default table's key property
            SKKDataObComponentTable skkTab = myTables.Find(x => x.IsDefault);
            string skkKey = skkTab.TableKey;
            PropertyInfo pi = GetType().GetTypeInfo().GetProperty(myTables.Find(x => x.IsDefault).TableKey);

            // Convert the value of the key property to a string
            string keyVal = $"{pi.GetValue(this)}";

            // If the property type actually is a string, wrap the value in quotes
            if (pi.PropertyType == typeof(string)) keyVal = $"'{keyVal}'";

            // Loop through the table keys
            foreach (string key in myFieldLists.Keys)
            {
                if ((tableName != "") && (tableName != key)) continue;
                // If we're a new device or failed to load initially, make sure we're including PrimaryKeyString into the DB
                if (NewDataOb || FailedLoad || ((tableName != "") && newStatus))
                {
                    // New device will need to write to every table, so go ahead and create a tuple list for each tablekey
                    // Pass in the table's 'Key' table name with the default table key's value
                    ret[key] = new List<(string PropertyField, string Value)>() { ($"{myTables.Find(x => x.TableName == key).TableKey}", $"{keyVal}") };
                }
                // Loop though field lists for each table
                foreach (SKKDataObComponent sc in myFieldLists[key])
                {
                    try
                    {
                        // Get the current value of the property
                        o = ti.GetProperty(sc.PropertyName).GetValue(this);

                        string v = "";
                        if ((sc.PropertyType == typeof(string)) || (sc.PropertyType.Name == "String"))
                            v = ((((string)o) == "") || (o is null)) ? "NULL" : $"'{o}'";
                        else if (sc.PropertyType == typeof(DateTime))
                            v = $"'{((DateTime)o).ToString(DataObDateFormat)}'";
                        else if (sc.PropertyType == typeof(DateTime?))
                            v = ((DateTime?)o).HasValue ? $"'{((DateTime?)o).Value.ToString(DataObDateFormat)}'" : "NULL";
                        //else v = $"{o}";
                        else v = (o.ToString() == "") ? "NULL" : $"{o}";

                        if (!ret.ContainsKey(key)) ret[key] = new List<(string PropertyField, string Value)>();
                        ret[key].Add((sc.FieldName, v));
                    }
                    catch (Exception /*x*/)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
            return ret;
        }
        #endregion

        #region SAVE FUNCTIONS
        protected List<(string TableName, bool NewStatus)> saveOptions = null;

        public void SaveToDB()
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "SaveToDB()");
            PreSave();
            List<string> sqls;
            if (saveOptions != null)
            {
                sqls = new List<string>();
                foreach ((string TableName, bool NewStatus) tup in saveOptions)
                {
                    sqls.AddRange(tup.NewStatus ? SaveToDB_New(tup.TableName) : SaveToDB_Existing(tup.TableName));
                }
            }
            else
            {
                sqls = (NewDataOb || FailedLoad) ? SaveToDB_New() : SaveToDB_Existing();
            }
            foreach (string sql in sqls)
            {
                using (var db = SKKDBHolder.Hold(DataObDB))
                {
                    dd.Msg($"Saving device with sql: {sql}");
                    DataObDB.ExecuteSql(sql);
                }
            }
            PostSave();
            NewDataOb = false;
            FailedLoad = false;
        }

        protected virtual void PreSave() { }
        protected virtual void PostSave() { }

        protected virtual bool ConfirmSaveTable(string tableName) => true;

        // Pass in a specific table name to get a save query for just that table
        // If table name is omitted, returns save queries for all tables
        private List<string> SaveToDB_New(string tableName = "")
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "SaveToDB_New()");
            Dictionary<string, List<(string PropertyField, string Value)>> saveData = GetSaveData(tableName, true);
            List<string> ret = new List<string>();
            foreach (string key in saveData.Keys)   // No need to filter here by tableName, GetSaveData should handle that...
            {
                if (!ConfirmSaveTable(key)) continue;
                string props = "";
                string vals = "";
                foreach ((string PropertyField, string Value) tup in saveData[key])
                {
                    //if (tup.Value == "''") continue;
                    props += $"{tup.PropertyField},";
                    vals += $"{tup.Value},";
                }
                props = (props == "") ? "" : $"({props.Substring(0, props.Length - 1)})";
                vals = (vals == "") ? "" : $"({vals.Substring(0, vals.Length - 1)})";
                if ((props != "") && (vals != ""))
                    ret.Add($"INSERT INTO {key} {props} VALUES {vals}");
            }
            return ret;
        }
        
        // Pass in a specific table name to get a save query for just that table
        // If table name is omitted, returns save queries for all tables
        public List<string> SaveToDB_Existing(string tableName = "")
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "SaveToDB_Existing()");
            Dictionary<string, List<(string PropertyField, string Value)>> saveData = GetSaveData(tableName, false);
            List<string> ret = new List<string>();
            foreach (string key in saveData.Keys)
            {
                if (!ConfirmSaveTable(key)) continue;
                string sql = $"UPDATE {key} SET ";
                foreach ((string PropertyField, string Value) tup in saveData[key]) sql += $"{tup.PropertyField}={tup.Value},";
                sql = sql.Substring(0, sql.Length - 1);
                ret.Add(sql + $" WHERE {myTables.Find(x => x.TableName == key).TableKey}={PrimaryKeyString};");
            }
            return ret;
        }
        #endregion

        #region STATIC TABLE DEFINITIONS
        protected static List<SKKDataObComponentTable> myTables = null;
        protected static void PopulateMyTables()
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "Populating Property & Field Lists");
            // Extract table definitions...
            myTables = new List<SKKDataObComponentTable>();
            Attribute[] classAttrs = Attribute.GetCustomAttributes(typeof(DATAOBTYPE), typeof(SKKDataObComponentTableAttribute));
            foreach (Attribute a in classAttrs)
            {
                SKKDataObComponentTableAttribute sa = a as SKKDataObComponentTableAttribute;
                myTables.Add(new SKKDataObComponentTable(sa.TableName, sa.TableKey, sa.DefaultTable));
            }
        }
        #endregion

        #region INSTANCED FIELD DEFINITIONS
        // Why isn't myFieldLists static instead of instanced??
        // I think it was because at one time the attribute was also going to store the default/original value that
        // the attribute loaded, which wouldn't work if FieldLists were static and there were ever more than one
        // object loaded in at a time...
        protected Dictionary<string, List<SKKDataObComponent>> myFieldLists = null; // new Dictionary<string, List<string>>();
        protected void PopulateFieldLists()
        {
            using var dd = SKKDebugDepth.Get($"{DBGName}", "Populating Field Lists");
            // Clear lists...
            myFieldLists = new Dictionary<string, List<SKKDataObComponent>>();

            // Type & Property Variables
            //Type myType = GetType();
            //TypeInfo myTypeInfo = GetType().GetTypeInfo();
            PropertyInfo[] myProperties = GetType().GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // Loop through our properties...
            foreach (PropertyInfo propertyInfo in myProperties)
            {
                // Check property for a 'ShipComponentAttribute;'
                IEnumerable<SKKDataObComponentAttribute> attrs = propertyInfo.GetCustomAttributes().OfType<SKKDataObComponentAttribute>();
                if (attrs.Count() > 0)
                {
                    // Property had 'ShipComponentAttribute', add to property list...
                    foreach (SKKDataObComponentAttribute attr in attrs)
                    {
                        dd.Msg($" --- CustomAttribute: {propertyInfo.Name} / {attr.TableName} / {attr.FieldName}");

                        // 'ShipComponentAttribute' has no TableName set, so set it to the default table's name
                        if ((attr.TableName is null) || (attr.TableName == "")) attr.TableName = myTables.Find(x => x.IsDefault).TableName;

                        // If field list has no key/list for this property's table, make it...
                        if (!myFieldLists.ContainsKey(attr.TableName))
                            myFieldLists[attr.TableName] = new List<SKKDataObComponent>();

                        // Add field to the fieldlist for this property's table...
                        myFieldLists[attr.TableName].Add(new SKKDataObComponent(attr.FieldName, propertyInfo.Name, propertyInfo.PropertyType, attr.SkipLoad));
                    }
                }
            }
        }
        #endregion
    }
}

