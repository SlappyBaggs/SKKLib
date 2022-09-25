using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace System
{
	public static class mhTypeExtender
	{
		public static string GetDisplayName(this Type t)
		{
			//var dispName = System.Reflection.mhMemberInfoExtender.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>(t, false);
			//if (dispName == null)
			//return t.Name;
			//return dispName.DisplayName;
			return "FUCK YOU";
		}

		/// <summary>
		/// Returns the interfaces the type directly derives from (ie excluding inherited ifaces
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetDirectInterfaces(this Type t)
		{
			Type[] ifaces = t.GetInterfaces();
			List<Type> exclude = new List<Type>();
			foreach(Type iface in ifaces)
			{
				if (ifaces.Any(x=>x != iface && x.GetInterfaces().Any(z=>z == iface )))
					exclude.Add(iface);
			}

			return ifaces.Where(x => exclude.Any(z => z == x) == false);
		}

		public static IList<Tuple<MethodInfo, Type>> GetExplicitInterfaceMethods(this Type t)
		{
			return GetExplicitInterfaceMethods(t, false);
		}
		public static IList<Tuple<MethodInfo, Type>> GetExplicitInterfaceMethods(this Type t, bool includeSpecial)
		{
			List<Tuple<MethodInfo, Type>> ret = new List<Tuple<MethodInfo, Type>>();
			foreach(Type iface in t.GetInterfaces())
			{
				var map = t.GetInterfaceMap(iface);
				for (int i = 0; i < map.InterfaceMethods.Length; ++i)
				{
					MethodInfo mi = map.TargetMethods[i];
					if (((int)mi.Attributes & (int)MethodAttributes.Private) != 0 && (includeSpecial || !mi.IsSpecialName))
					{
						ret.Add(Tuple.Create(mi, iface));
					}
				}
			}
			return ret;
		}

		public static Type IsInInterfaceMap(this Type t, MethodInfo mi)
		{
			foreach(Type iface in t.GetInterfaces())
			{
				if (t.GetInterfaceMap(iface).TargetMethods.Any(x => x == mi))
					return iface;
			}
			return null;
		}

		public static IList<Tuple<PropertyInfo, Type>> GetExplicitInterfaceProperties(this Type t)
		{
			List<Tuple<PropertyInfo, Type>> ret = new List<Tuple<PropertyInfo, Type>>();
			foreach (PropertyInfo p in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				// if the get or set accessor is in the interface map this property is an interface property
				// because it is private, it is explicitly implemented
				foreach(MethodInfo acc in p.GetAccessors(true))
				{
					var tt = (t.IsInInterfaceMap(acc));
					if (tt != null)
					{
						ret.Add(Tuple.Create(p, tt));
						break;
					}
				}
			}
			return ret;
		}

		public static ConstructorInfo GetConstructor<Arg1Type>(this Type t)
		{
			return t.GetConstructor(new Type[] { typeof(Arg1Type) });
		}
		public static ConstructorInfo GetConstructor<Arg1Type, Arg2Type>(this Type t)
		{
			return t.GetConstructor(new Type[] { typeof(Arg1Type), typeof(Arg2Type) });
		}
		public static ConstructorInfo GetConstructor<Arg1Type, Arg2Type, Arg3Type>(this Type t)
		{
			return t.GetConstructor(new Type[] { typeof(Arg1Type), typeof(Arg2Type), typeof(Arg3Type) });
		}
		public static ConstructorInfo GetConstructor<Arg1Type, Arg2Type, Arg3Type, Arg4Type>(this Type t)
		{
			return t.GetConstructor(new Type[] { typeof(Arg1Type), typeof(Arg2Type), typeof(Arg3Type), typeof(Arg4Type) });
		}
		public static ConstructorInfo GetConstructor<Arg1Type, Arg2Type, Arg3Type, Arg4Type, Arg5Type>(this Type t)
		{
			return t.GetConstructor(new Type[] { typeof(Arg1Type), typeof(Arg2Type), typeof(Arg3Type), typeof(Arg4Type), typeof(Arg5Type)});
		}

		/// <summary>
		/// Returns all public index properties in a type
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static IEnumerable<PropertyInfo> GetIndexProperties(this Type t)
		{
			foreach (var prop in t.GetProperties())
			{
				if (prop.GetIndexParameters().Length > 0)
					yield return prop;
			}
		}

		public static string DecodeGenericName(this Type t)
		{
			return Megahard.Reflection.GenericUtils.DecodeGenericName(t.Name);
		}

		public static string Describe(this Type t)
		{
			if (!t.IsGenericType)
				return t.Name;
			string s = t.DecodeGenericName() + "<";
			int pos = 0;
			foreach (Type gt in t.GetGenericArguments())
			{
				s += gt.Name + (pos++ == 0 ? "" : ", ");
			}
			s += ">";
			return s;
		}

		public static IEnumerable<MethodInfo> GetMethods<attr>(this Type t, BindingFlags flags, bool inherit) where attr : Attribute
		{
			foreach (System.Reflection.MethodInfo mi in t.GetMethods(flags))
			{
				if (mi.GetCustomAttributes(typeof(attr), inherit).Length > 0)
					yield return mi;
			}
		}

		public static bool IsNullable(this Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
		public static Type GetTypeOfNullable(this Type t)
		{
			if (!t.IsNullable())
				throw new InvalidOperationException("GetTypeOfNullable called on non nullable type arg");
			return t.GetGenericArguments()[0];
		}
	}
}
