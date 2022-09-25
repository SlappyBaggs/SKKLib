﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel;
using Megahard.ExtensionMethods;
namespace Megahard.Data.Controls
{
	public partial class DataBox
	{
		[TypeConverter(typeof(DataLabel.Converter))]
		[DefaultProperty("Text")]
		public class DataLabel : ObservableObject
		{
			internal DataLabel(DataBox dbox)
			{
				System.Diagnostics.Debug.Assert(dbox != null);
				dbox_ = dbox;
			}

			public enum LabelPlacement { Left, Top };

			
			#region Placement Property
			LabelPlacement propPlacement_ = LabelPlacement.Left;
			[DefaultValue(LabelPlacement.Left)]
			[ObservableProperty]
			public LabelPlacement Placement
			{
				get { return propPlacement_; }
				set
				{
					if(propPlacement_ == value)
						return;
					RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("Placement", value));
					LabelPlacement oldVal = Placement;
					propPlacement_ = value;
					RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("Placement", oldVal, value));
				}
			}
			#endregion

			#region public LabelSize Size { get; set; }
			LabelSize propSize_ = LabelSize.Auto;
			[Megahard.Data.ObservableProperty]
			public LabelSize Size
			{
				get { return propSize_; }
				set
				{
					if (Size == value)
						return;
					RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("Size", value));
					var oldVal = Size;
					propSize_ = value;
					RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("Size", oldVal, value));
				}
			}

			void ResetSize()
			{
				Size = LabelSize.Auto;
			}
			bool ShouldSerializeSize()
			{
				return Size != LabelSize.Auto;
			}
			#endregion
			readonly DataBox dbox_;

			[Browsable(false)]
			public bool Visible
			{
				get { return Size != LabelSize.Hidden; }
			}

			#region Text Property
			string propText_ = "{SmartName}";
			[DefaultValue("{SmartName}")]
			[Megahard.Data.ObservableProperty]
			public string Text
			{
				get { return propText_; }
				set
				{
					if (value == string.Empty)
						value = null;
					if(propText_ == value)
						return;
					RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("Text", value));
					string oldVal = Text;
					propText_ = value;
					RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("Text", oldVal, value));
				}
			}
			#endregion

			public string GetParsedText()
			{
				return ParseText(Text);
			}

			string ParseText(string txt)
			{
				if (string.IsNullOrEmpty(txt))
					return string.Empty;
				if (dbox_.Data == null)
				{
					return txt.Replace("{ClassName}", "").Replace("{DisplayName}", "").Replace("{ComponentName}", "").Replace("{SmartName}", "");
				}


				var attrs = TypeDescriptor.GetAttributes(dbox_.Data);
				txt = txt.Replace("{ClassName}", TypeDescriptor.GetClassName(dbox_.Data));

				var dna = attrs[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				txt = txt.Replace("{DisplayName}", dna != null ? dna.DisplayName : string.Empty);

				var componentName = TypeDescriptor.GetComponentName(dbox_.Data);
				txt = txt.Replace("{ComponentName}", componentName);

				string smartName = "";
				if(dna != null && !string.IsNullOrEmpty(dna.DisplayName))
					smartName = dna.DisplayName;
				else
					smartName = componentName;

				txt = txt.Replace("{SmartName}", smartName);
				return txt;
			}

			class Converter : TypeConverter
			{
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
				}

				public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						var lbl = value as DataLabel;
						return lbl.Text ?? "<empty>";
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}

				public override bool GetPropertiesSupported(ITypeDescriptorContext context)
				{
					return true;
				}

				public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
				{
					return TypeDescriptor.GetProperties(value, attributes);
				}
			}

			[TypeConverter(typeof(LabelSize.Converter))]
			public struct LabelSize : IEquatable<LabelSize>
			{
				int percent_;
				public LabelSize(int i)
				{
					if (i > 100)
						i = 100;
					if (i < 0)
						i = -1;
					percent_ = i;
				}


				public int CalculatePixelSize(int refSize)
				{
					if (percent_ == AutoValue)
						return refSize;
					double d = (double)percent_ / 100.0;
					return (int)(refSize * d);
				}

				public static implicit operator LabelSize(int i)
				{
					return new LabelSize(i);
				}
				public override string ToString()
				{
					if (percent_ == AutoValue)
						return "Auto";
					if (percent_ == HiddenValue)
						return "Hidden";
					return percent_.ToString() + "%";
				}
				const int AutoValue = -1;
				const int HiddenValue = 0;

				public static LabelSize Auto
				{
					get { return AutoValue; }
				}

				public static LabelSize Hidden
				{
					get { return HiddenValue; }
				}

				public bool IsHidden
				{
					get { return percent_ == HiddenValue; }
				}

				#region Equality Overloads
				public static bool operator ==(LabelSize val1, LabelSize val2)
				{
					return val1.percent_ == val2.percent_;
				}

				public static bool operator !=(LabelSize val1, LabelSize val2)
				{
					return val1.percent_ != val2.percent_;
				}

				public override int GetHashCode()
				{
					return percent_.GetHashCode();
				}

				public override bool Equals(object obj)
				{
					return (obj is LabelSize) && (percent_ == ((LabelSize)obj).percent_);
				}

				public bool Equals(LabelSize val)
				{
					return percent_ == val.percent_;
				}
				#endregion

				class Converter : TypeConverter
				{
					public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
					{
						return true;
					}

					public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
					{
						return false;
					}

					public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
					{
						LabelSize[] vals = new [] { LabelSize.Auto, LabelSize.Hidden, 10, 20, 25, 30, 40, 50, 60, 70, 75 };
						return new StandardValuesCollection(vals);
					}

					

					public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
					{
						return destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) || base.CanConvertTo(context, destinationType);
					}

					public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
					{
						if(destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor))
						{
							var lblSize = (LabelSize)value;
							if (lblSize == Hidden)
							{
								var meth = typeof(LabelSize).GetProperty("Hidden", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
								return new System.ComponentModel.Design.Serialization.InstanceDescriptor(meth, null);
							}
							else if (lblSize == Auto)
							{
								var meth = typeof(LabelSize).GetProperty("Auto", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
								return new System.ComponentModel.Design.Serialization.InstanceDescriptor(meth, null);
							}
							return new System.ComponentModel.Design.Serialization.InstanceDescriptor(typeof(LabelSize).GetConstructor<int>(), new[] { lblSize.percent_ });
						}
						if(destinationType == typeof(string))
						{
							return ((LabelSize)value).ToString();
						}
						return base.ConvertTo(context, culture, value, destinationType);
					}

					public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
					{
						return  sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
					}

					public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
					{
						if (value == null)
							return new LabelSize();
						if (value.GetType() == typeof(string))
						{
							string s = (string)value;
							s = s.Trim();

							if (s == "Auto")
								return Auto;
							if (s == "Hidden")
								return Hidden;

							if (s.EndsWith("%"))
								s = s.Substring(0, s.Length - 1);
							int i;
							if (int.TryParse(s, out i))
								return new LabelSize(i);
						}
						return base.ConvertFrom(context, culture, value);
					}
				}
			}

		}
	}
}
