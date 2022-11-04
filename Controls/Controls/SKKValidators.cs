using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SKKLib.Controls.Controls
{
    public static class SKKValidators
    {
        private static ErrorProvider errorProvider = new ErrorProvider();

        #region VALIDATORS

        #region DATETIME
        private static CultureInfo culture_enUS = new CultureInfo("en-US");
        public static string DateFormat { get; set; }
        public static void DateTimeValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !DateTime.TryParse(((Control)sender).Text, out _))) errorProvider.SetError(((Control)sender), $"You must enter a valid date: {DateFormat}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void DateTimeExactValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (DateFormat != "") && (e.Cancel = !DateTime.TryParseExact(((Control)sender).Text, DateFormat, culture_enUS, DateTimeStyles.None, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid date: {DateFormat}");
            else errorProvider.SetError((sender as Control), "");
        }
        #endregion

        #region INTEGERS

        #region (S)Byte
        public static void SByteValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !sbyte.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), "You must enter a valid sbyte");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void ByteValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !byte.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), "You must enter a valid byte");
            else errorProvider.SetError((sender as Control), "");
        }
        #endregion

        #region (U)INT16 / (U)Short
        public static void Int16Validating(object sender, CancelEventArgs e, string name = "Int16")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Int16.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void ShortValidating(object sender, CancelEventArgs e) => Int16Validating(sender, e, "short");

        public static void UInt16Validating(object sender, CancelEventArgs e, string name = "UInt16")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !UInt16.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void UShortValidating(object sender, CancelEventArgs e) => UInt16Validating(sender, e, "ushort");
        #endregion

        #region (U)INT32 / (U)Int
        public static void Int32Validating(object sender, CancelEventArgs e, string name = "Int32")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Int32.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void IntValidating(object sender, CancelEventArgs e) => Int32Validating(sender, e, "int");

        public static void UInt32Validating(object sender, CancelEventArgs e, string name = "UInt32")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !UInt32.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void UIntValidating(object sender, CancelEventArgs e) => UInt32Validating(sender, e, "uint");
        #endregion

        #region (U)INT64 / (U)Long
        public static void Int64Validating(object sender, CancelEventArgs e, string name = "Int64")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Int64.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void LongValidating(object sender, CancelEventArgs e) => Int64Validating(sender, e, "long");

        public static void UInt64Validating(object sender, CancelEventArgs e, string name = "UInt64")
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !UInt64.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((sender as Control), $"You must enter a valid {name}");
            else errorProvider.SetError((sender as Control), "");
        }
        public static void ULongValidating(object sender, CancelEventArgs e) => UInt64Validating(sender, e, "ulong");
        #endregion

        #endregion

        #region FLOATING PRECISION

        public static void DecimalValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Decimal.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((Control)sender, "You must enter a valid decimal");
            else errorProvider.SetError((Control)sender, "");
        }
        
        public static void DoubleValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Double.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((Control)sender, "You must enter a valid double");
            else errorProvider.SetError((Control)sender, "");
        }

        public static void FloatValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !float.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((Control)sender, "You must enter a valid float");
            else errorProvider.SetError((Control)sender, "");
        }

        public static void SingleValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !Single.TryParse(((Control)sender).Text, out _))) errorProvider.SetError((Control)sender, "You must enter a valid single");
            else errorProvider.SetError((Control)sender, "");
        }
        #endregion

        #endregion
    }
}
