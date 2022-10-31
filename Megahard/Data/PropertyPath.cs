using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Megahard.Data
{
	[TypeConverter(typeof(PropertyPath.Converter))]
	[System.Diagnostics.DebuggerDisplay("{Path}")]
	public struct PropertyPath
	{
		private readonly string path_;
		readonly Collections.ImmutableArray<PathPart> pathParts_;

		static readonly char[] s_splitDelim = new char[] { '.', '[' };

		public static char Separator { get { return s_splitDelim[0]; } }

		static Collections.ImmutableArray<PathPart> SplitPath(string path)
		{
			int pos = 0;
			int srchpos = 0;
			var builder = Collections.ImmutableArray<PathPart>.Build();
			while (pos < path.Length)
			{
				int delim = path.IndexOfAny(s_splitDelim, srchpos);
				if (delim == -1)
				{
					builder.Add(PathPart.Create(path.Substring(pos)));
					break;
				}
				if(delim > pos)
					builder.Add(PathPart.Create(path.Substring(pos, delim - pos)));
				pos = delim + (path[delim] == '.' ? 1 : 0);
				srchpos = delim + 1;
			}
			return builder.ToArray();
		}

		public static PropertyPath Combine(PropertyPath prop1, PropertyPath prop2)
		{
			if (prop1.IsEmpty)
				return prop2;
			if (prop2.IsEmpty)
				return prop1;
			return new PropertyPath(string.Concat(prop1.Path, Separator, prop2.Path));
		}
		public static PropertyPath operator +(PropertyPath prop1, PropertyPath prop2)
		{
			return Combine(prop1, prop2);
		}

		public static string operator +(string s, PropertyPath prop)
		{
			return s + prop.Path;
		}

		public static string operator +(PropertyPath prop, string s)
		{
			return prop.Path + s;
		}

		public PropertyPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				path_ = null;
				pathParts_ = null;
			}
			else
			{
				path_ = path;
				pathParts_ = SplitPath(path);		
			}
		}

		public bool IsChildOf(PropertyPath prop)
		{
			if (IsEmpty)
				return false;
			if (PathCount == (prop.PathCount + 1))
			{
				for (int i = 0; i < prop.PathCount; ++i)
				{
					if (this[i] != prop[i])
						return false;
				}
				return true;
			}
			return false;
		}

		public bool IsParentOf(PropertyPath prop)
		{
			if (IsEmpty)
				return false;
			if (PathCount == (prop.PathCount - 1))
			{
				for (int i = 0; i < PathCount; ++i)
				{
					if (this[i] != prop[i])
						return false;
				}
				return true;
			}
			return false;
		}

		public bool IsDescendantOf(PropertyPath prop)
		{
			if(prop.IsEmpty)
				return false;
			if (PathCount > prop.PathCount)
			{
				for (int i = 0; i < prop.PathCount; ++i)
				{
					if (this[i] != prop[i])
						return false;
				}
				return true;
			}
			return false;
		}

		public bool IsAncestorOf(PropertyPath prop)
		{
			if (PathCount < prop.PathCount && !IsEmpty)
			{
				for (int i = 0; i < PathCount; ++i)
				{
					if (this[i] != prop[i])
						return false;
				}
				return true;
			}
			return false;
		}

		[Browsable(false)]
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		public bool IsEmpty
		{
			get { return path_ == null; }
		}

		public string Path
		{
			get
			{
				return path_;
			}
		}

		public object GetValue(object reference)
		{
			if (IsEmpty)
				return reference;
			var prop = ResolveProperty(ref reference);
			return prop.GetValue(reference);
		}
		public void SetValue(object reference, object val)
		{
			if (IsEmpty)
				throw new InvalidOperationException("Cannot SetValue on Empty PropertyPath");
			var prop = ResolveProperty(ref reference);
			prop.SetValue(reference, SmartConvert.ConvertTo(val, prop.PropertyType, prop.Converter));
		}
		public static PropertyPath DifferencePath(PropertyPath prop1, PropertyPath prop2)
		{
			if (!prop1.IsDescendantOf(prop2))
				throw new InvalidOperationException(prop1.Path + " - " + prop2.Path + " cannot be calculated");

			StringBuilder sb = new StringBuilder();

			int p = prop2.PathCount;
			sb.Append(prop1[p++]);
			while (p < prop1.PathCount)
				sb.Append(Separator).Append(prop1[p++]);
			return sb.ToString();
		}


		static bool IsIndexer(string s)
		{
			int len = s.Length;
			return len > 1 && s[0] == '[' && s[len - 1] == ']';
		}
		public PropertyDescriptor ResolveProperty(ref object reference)
		{
			if (IsEmpty)
				throw new FailedToResolvePropertyException(this, null);

			try
			{
				PropertyDescriptor prop = null;
				int pos = 0;
				int lastPos = PathParts.Length - 1;
				foreach (PathPart part in PathParts)
				{
					if (reference != null)
						prop = part.Resolve(reference);
					else if (prop != null)
						prop = part.Resolve(prop.PropertyType);

					if (prop == null)
						throw new FailedToResolvePropertyException(this, null);
					if (pos != lastPos && reference != null)
					{
						reference = prop.GetValue(reference);
					}
					pos += 1;
				}
				return prop;
			}
			catch (Exception e)
			{
				throw new FailedToResolvePropertyException(this, e);
			}
		}
		public PropertyDescriptor ResolveProperty(object reference)
		{
			if (reference == null)
				throw new FailedToResolvePropertyException(this, new NullReferenceException());
			try
			{
				object r = reference;
				return ResolveProperty(ref r);
			}
			catch (FailedToResolvePropertyException)
			{
				return ResolveProperty(reference.GetType());
			}
		}
		public PropertyDescriptor ResolveProperty(Type obType)
		{
			if (IsEmpty)
				throw new FailedToResolvePropertyException(this, null);
			try
			{
				PropertyDescriptor pd = null;
				foreach (PathPart part in PathParts)
				{
					pd = part.Resolve(obType);
					if (pd == null)
						throw new FailedToResolvePropertyException(this, null);
					obType = pd.PropertyType;
				}
				return pd;
			}
			catch (Exception e)
			{
				throw new FailedToResolvePropertyException(this, e);
			}
		}
		public bool IsValid(object reference)
		{
			try
			{
				var prop = ResolveProperty(ref reference);
				return true;
			}
			catch (FailedToResolvePropertyException)
			{
				return false;
			}
		}

		public override string ToString()
		{
			return path_;
		}

		public PathPart this[int index]
		{
			get 
			{
				return pathParts_[index]; 
			}
		}

		public int PathCount
		{
			get
			{
				return pathParts_ == null ? 0 : pathParts_.Length;
			}
		}

		public string Root
		{
			get
			{
				if (pathParts_ == null)
					return null;
				return pathParts_[0].Name;
			}
		}

		public Collections.ImmutableArray<PathPart> PathParts
		{
			get { return pathParts_ ?? Collections.ImmutableArray<PathPart>.Empty; }
		}

		public static implicit operator PropertyPath(string propPath)
		{
			return new PropertyPath(propPath);
		}

		public class FailedToResolvePropertyException : Exception
		{
			internal FailedToResolvePropertyException(PropertyPath propPath, Exception inner) : base(string.Format("Failed to resolve property path '{0}'", propPath.Path), inner)
			{
				propPath_ = propPath;
			}

			private readonly PropertyPath propPath_;
			public PropertyPath PropertyPath
			{
				get { return propPath_; }
			}
		}

		public static bool operator==(PropertyPath prop, string s)
		{
			if (string.IsNullOrEmpty(s))
				return prop.Path == null;
			return prop.Path == s;
		}

		public static bool operator ==(PropertyPath prop1, PropertyPath prop2)
		{
			return prop1.Path == prop2.Path;
		}

		public static bool operator !=(PropertyPath prop1, PropertyPath prop2)
		{
			return prop1.Path != prop2.Path;
		}

		public static bool operator !=(PropertyPath prop, string s)
		{
			if (string.IsNullOrEmpty(s))
				return prop.Path != null;
			return prop.Path != s;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return Path == null;
			if (obj is string)
			{
				string s = (string)obj;
				if (s == string.Empty)
					return Path == null;
				return Path == s;
			}
				
			if (obj is PropertyPath)
				return Path == ((PropertyPath)obj).Path;
			return base.Equals(obj);
		}

		public bool Equals(string s)
		{
			if (string.IsNullOrEmpty(s))
				return Path == null;
			return Path == s;
		}

		public bool Equals(PropertyPath pp)
		{
			return Path == pp.Path;
		}

		public override int GetHashCode()
		{
			return Path == null ? 0 : Path.GetHashCode();
		}

		class Converter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new PropertyPath(value as string);
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor) && value is PropertyPath)
				{
					var propPath = (PropertyPath)value;
					var ctor = propPath.GetType().GetConstructor(new Type[] { typeof(string) });
					return new InstanceDescriptor(ctor, new object[] { propPath.Path });
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		public class PathPart : IEquatable<string>, IEquatable<PathPart>
		{
			protected PathPart(string name)
			{
				name_ = name;
			}
			readonly string name_;

			public string Name
			{
				get { return name_; }
			}

			internal static PathPart Create(string name)
			{
				if(IsIndexer(name))
				{
					return new IndexedPathPart(name);
				}
				else
				{
				}
				return new PathPart(name);
			}

			public virtual PropertyDescriptor Resolve(Type t)
			{
				return TypeDescriptor.GetProperties(t)[Name];
			}
			public virtual PropertyDescriptor Resolve(object component)
			{
				return TypeDescriptor.GetProperties(component)[Name];
			}

			public override string ToString()
			{
				return Name;
			}
			#region IEquatable<string> Members

			public bool Equals(string other)
			{
				return Name == other;
			}

			#endregion

			#region Equality Overloads
			public static bool operator ==(PathPart val1, PathPart val2)
			{
				return val1.Name == val2.Name;
			}

			public static bool operator !=(PathPart val1, PathPart val2)
			{
				return val1.Name != val2.Name;
			}

			public override int GetHashCode()
			{
				return Name.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				return (obj is PathPart) && (Name == ((PathPart)obj).Name);
			}

			public bool Equals(PathPart val)
			{
				return Name == val.Name;
			}
			#endregion

		}

		class IndexedPathPart : PathPart
		{
			internal IndexedPathPart(string name) : base(name)
			{
				var arg  = name.Substring(1, name.Length - 2);

				int i;
				decimal d;
				if (int.TryParse(arg, out i))
					indexArg_ = i;
				else if (decimal.TryParse(arg, out d))
					indexArg_ = d;
				else
					indexArg_ = arg;
			}

			object indexArg_;

			public override PropertyDescriptor Resolve(object component)
			{
				return Resolve(component.GetType());
			}

			public override PropertyDescriptor Resolve(Type t)
			{
				var defmem = t.GetCustomAttribute<DefaultMemberAttribute>(true);
				if (defmem != null)
				{
					var props = (from pi in t.GetIndexProperties() where pi.Name == defmem.MemberName && pi.GetIndexParameters().Length == 1 select pi);
					foreach (var prop in props)
					{
						var arg = prop.GetIndexParameters()[0];
						if (arg.ParameterType.IsAssignableFrom(indexArg_.GetType()))
						{
							return new ComponentModel.IndexPropertyDescriptor(prop, new object[] { indexArg_ });
						}
						if (arg.ParameterType.IsAssignableFrom(typeof(string)))
						{
							return new ComponentModel.IndexPropertyDescriptor(prop, new object[] { indexArg_.ToString() });
						}
					}
					var firstprop = props.FirstOrDefault();
					if (firstprop != null)
						return new ComponentModel.IndexPropertyDescriptor(firstprop);
				}
				throw new FailedToResolvePropertyException(Name, null);
			}
		}
	}
}
