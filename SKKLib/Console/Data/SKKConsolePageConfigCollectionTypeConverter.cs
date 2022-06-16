using System;
using System.ComponentModel;
using System.Globalization;
using SKKLib.Console.Config;

namespace SKKLib.Console.Data
{
    public class ConsolePageConfigCollectionTypeConverter :  TypeConverter //ExpandableObjectConverter
    {
        //private static string delim_ = "|";

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ConsolePageConfigCollection coll = value as ConsolePageConfigCollection;

            if (destinationType != typeof(string) || coll is null) return base.ConvertTo(context, culture, value, destinationType);

            return "(Collection)";
            
            //if (coll.Count == 0) return "";
            //StringBuilder sb = new StringBuilder();
            //if(coll.Count > 0) foreach (ConsolePageConfig c in coll) sb.Append($"{TypeDescriptor.GetConverter(typeof(ConsolePageConfig)).ConvertToInvariantString(c)}{delim_}");
            //sb.Remove(sb.Length - delim_.Length, delim_.Length);
            //return sb.ToString();
        }

        //public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        //public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        //{
            //string coll_ = value as string;
            //if (coll_ is null) return base.ConvertFrom(context, culture, value);
            
            //ConsolePageConfigCollection collection = new ConsolePageConfigCollection();
            //string[] sa = coll_.Split(delim_.ToCharArray());
            //foreach(string s in sa) collection.Add(TypeDescriptor.GetConverter(typeof(ConsolePageConfig)).ConvertFromInvariantString(s) as ConsolePageConfig);
            //return collection;
        //}
    }
}
