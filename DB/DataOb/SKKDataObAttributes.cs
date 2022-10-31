using System;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SKKLib.SystemLib;
using static SKKLib.Console.SKKConsole;

//using MySqlX.XDevAPI;

namespace SKKLib.DB.DataOb
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SKKDataObComponentAttribute : Attribute
    {
        public SKKDataObComponentAttribute(string Field = "", string Table = "", [CallerMemberName] string Property = "", bool SaveOnly = false)
        {
            PropertyName = Property;
            FieldName = (Field != "") ? Field : Property;
            TableName = Table;
            SkipLoad = SaveOnly;
        }

        public string PropertyName { get; private set; }
        public string FieldName { get; private set; }
        public string TableName { get; set; }
        public object OrigValue { get; set; }
        public bool SkipLoad { get; set; }
    }

    public class SKKDataObComponent
    {
        public SKKDataObComponent(string Field = "", string Property = "", Type propertyType = null, bool skipLoad = false)
        {
            PropertyName = Property;
            FieldName = (Field != "") ? Field : Property;
            PropertyType = propertyType;
            SkipLoad = skipLoad;
        }

        public string PropertyName { get; private set; }
        public string FieldName { get; private set; }
        public Type PropertyType { get; private set; }
        public bool SkipLoad { get; private set; }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SKKDataObComponentTableAttribute : Attribute
    {
        public SKKDataObComponentTableAttribute(string _tableName, string _keyName, bool _defaultTable = false)
        {
            TableName = _tableName;
            TableKey = _keyName;
            DefaultTable = _defaultTable;
        }

        public string TableName { get; private set; }
        public string TableKey { get; private set; }
        public bool DefaultTable { get; private set; }
    }

    public class SKKDataObComponentTable
    {
        public SKKDataObComponentTable(string tname, string key, bool def = false)
        {
            TableName = tname;
            TableKey = key;
            IsDefault = def;
        }

        public string TableName { get; private set; } = "";

        public string TableKey { get; private set; } = "";

        public bool IsDefault { get; private set; } = false;
    }

#if USE_COLLECTION
    // We'll develop this at a later time...
    #region DATA OB COLLECTION
    public class SKKDataObCollectionAttribute<COLLTYPE> : Attribute
    {
        public SKKDataObCollectionAttribute()
        {
        }

        SKKDataObCollection<COLLTYPE> dataObCollection = new SKKDataObCollection<COLLTYPE>();
    }

    public class SKKDataObCollection<COLLTYPE> : Collection<COLLTYPE>
    {
    }
    #endregion
#endif
}

