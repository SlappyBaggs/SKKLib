using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using SKKLib.Console.Config;

namespace SKKLib.Console.Data
{
    public class ConsolePageConfigTypeConverter : ExpandableObjectConverter
    {
        internal static string delim_ = ";";

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ConsolePageConfig)
            {
                ConsolePageConfig config = (ConsolePageConfig)value;
                return $"{config.PageName}{delim_}{config.PageColor.Name}{delim_}{TypeDescriptor.GetConverter(typeof(Font)).ConvertToInvariantString(config.PageFont)}"; // "Pagex " + config.PageName;
                //return $"{config.PageName}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var casted = value as string;
            if (casted == null) return base.ConvertFrom(context, culture, value);
            
            string[] sa = casted.Split(delim_.ToCharArray());
            int i;
            try
            {
                return new ConsolePageConfig(sa[0],
                    Int32.TryParse(sa[1], NumberStyles.HexNumber, CultureInfo.GetCultureInfo("en-us"), out i) ?
                    Color.FromArgb(Int32.Parse(sa[1], NumberStyles.HexNumber)) :
                    Color.FromName(sa[1]),
                    TypeDescriptor.GetConverter(typeof(Font)).ConvertFromInvariantString(sa[2]) as Font);
            }
            catch
            {
                return null;
            }
        }
    }
}
