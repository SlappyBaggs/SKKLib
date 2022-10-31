using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Collections;
using System.Reflection;

namespace Megahard.Serialization
{
	public class XSerializer
	{
		static XElement s_EmptyElement = new XElement("empty");
		static XAttribute s_EmptyAttribute = new XAttribute("empty", "");

		public XElement Serialize(object ob)
		{
			if (ob == null)
				return new XElement("Null");
			if (ob is string)
				return new XElement("String", ob.ToString());

			var xml = new XElement("Object", new XAttribute("Type", ob.GetType().FullName));

			Serialize(ob, xml, null);
			return xml;
		}

		static bool AreTypesEquivalent(Type t1, Type t2)
		{
			if (t1 == t2)
				return true;
			if (t1.IsNullable() && t1.GetTypeOfNullable() == t2)
				return true;
			if (t2.IsNullable() && t2.GetTypeOfNullable() == t1)
				return true;
			return false;
		}

		XElement Serialize(object ob, PropertyDescriptor prop, SerializationData sdata)
		{
			var xml = new XElement(prop.Name);
			object propVal = sdata.GetValueToSerialize(ob, prop);
			if (propVal != null && prop.SerializationVisibility != DesignerSerializationVisibility.Content && !AreTypesEquivalent(prop.PropertyType, propVal.GetType()))
			{
				xml.Add(new XAttribute("Type", propVal.GetType().FullName));
			}
			Serialize(propVal, xml, prop.Converter);
			return xml;
		}
		void Serialize(object ob, XElement xml, TypeConverter converter)
		{
			if(ob == null)
			{
				xml.Add(new XElement("Null"));
				return;
			}

			ISerializable serializable = (ob as ISerializable) ?? SerializableWrapper.Instance;
			
			var props = new XElement("Properties");

			var properties = from PropertyDescriptor prop in TypeDescriptor.GetProperties(ob)
							 let sd = serializable.GetSerializationData(prop.Name)
							 where sd.EvalShouldSerialize(ob, prop)
							 select Serialize(ob, prop, sd);
			props.Add(properties);

			if (props.HasElements)
				xml.Add(props);

			if (ob is IList)
			{
				var col = ob as IList;
				//if (col.IsFixedSize || col.IsReadOnly)
				//throw new SerializationException("Cannot serialize a readonly or fixed sized IList");
				// Why not serialize it?  We can, maybe we dont ever want to deserialize it, seems silly to error out here
				if (col.Count > 0)
				{
					var colxml = new XElement("Collection", from object item in col select Serialize(item));
					xml.Add(colxml);
				}
			}
			converter = converter ?? TypeDescriptor.GetConverter(ob);

			// Situation is here, is where are using string representation if ob supports it,
			// but it does raise issue: what if string rep doesnt encompass all of the object state, for now we ignore that
			if (converter.CanConvertFrom(typeof(string)))
			{
				var stringVal = converter.ConvertTo(ob, typeof(string));
				if (xml.HasElements)
					xml.Add(new XElement("Value", stringVal));
				else
					xml.SetValue(stringVal);
			}
		}
		public void RegisterCreator<T>() where T : new()
		{
			RegisterCreator(typeof(T).FullName, arg => new T());
		}

		public void RegisterCreator(Type t, Func<object, object> creator)
		{
			RegisterCreator(t.FullName, creator);
		}

		public void RegisterCreator(string name, Func<object, object> creator)
		{
			creators_.Add(name, creator);
		}

		public void DeregisterCreator(string name)
		{
			creators_.Remove(name);
		}

		readonly Dictionary<string, Func<object, object>> creators_ = new Dictionary<string, Func<object, object>>();

		object CreateObject(Type t, TypeConverter converter, string args)
		{
			if (t == null || t == typeof(string))
				return args;

			// always attempt the supplied converter first, it is assumed to have the most direct knowledge of what is going on
			try
			{
				if (converter != null && converter.CanConvertFrom(typeof(string)))
				{
					return converter.ConvertFrom(args);
				}
			}
			catch
			{
			}

			Func<object, object> creator;
			creators_.TryGetValue(t.FullName, out creator);
			object createdOb = creator != null ? creator(args) : null;
			if (createdOb == null)
			{
				try
				{
					if (args.HasChars())
					{
						converter = converter ?? TypeDescriptor.GetConverter(t);
						createdOb = converter.CanConvertFrom(typeof(string)) ? converter.ConvertFrom(args) : null;
						if(createdOb != null)
						{
							RegisterCreator(t, x =>
								{
									try
									{
										return converter.ConvertFrom(x);
									}
									catch
									{
										return Activator.CreateInstance(t);
									}
								});
						}
					}
				}
				catch
				{
				}
			}

			if (createdOb == null)
			{
				createdOb = Activator.CreateInstance(t);
				if (createdOb != null)
					RegisterCreator(t, x => Activator.CreateInstance(t));
			}

			return createdOb;
		}
		object CreateObject(string typeName, string args)
		{
			if (typeName.IsNullOrEmpty())
				return args;
			Func<object, object> creator;
			creators_.TryGetValue(typeName, out creator);
			object createdOb = creator != null ? creator(args) : null;
			if (createdOb == null)
			{
				Type t = null;
				foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					t = asm.GetType(typeName);
					if (t != null)
						break;
				}

				if (t != null && args.HasChars())
				{
					try
					{
						var converter = TypeDescriptor.GetConverter(t);
						createdOb = converter.CanConvertFrom(typeof(string)) ? converter.ConvertFrom(args) : null;
						if (createdOb != null)
						{
							RegisterCreator(t, x => converter.ConvertFrom(x));
						}
					}
					catch
					{
					}
				}
				if (createdOb == null && t != null)
				{
					createdOb = Activator.CreateInstance(t);
					if (createdOb != null)
						RegisterCreator(t, x => Activator.CreateInstance(t));
				}
			}
			return createdOb;
		}

		public object Deserialize(XElement xml, Type t, TypeConverter converter)
		{
			if (xml == null || t == null || xml.Element("Null") != null)
				return null;

			var ob = CreateObject(t, converter, (xml.Element("Value") ?? s_EmptyElement).Value.NullIfEmpty() ?? xml.Value.NullIfEmpty());
			DeserializeInto(xml, ob, SerializationData.Default);
			return ob;
		}
		public object Deserialize(XElement xml)
		{
			if (xml == null || xml.Element("Null") != null)
				return null;
			var typeName = (xml.Attribute("Type") ?? s_EmptyAttribute).Value.NullIfEmpty();
			var val = (xml.Element("Value") ?? s_EmptyElement).Value.NullIfEmpty() ?? xml.Value.NullIfEmpty();

			var ob = CreateObject(typeName, val);
			DeserializeInto(xml, ob, SerializationData.Default);
			return ob;
		}

		void Deserialize(XElement xml, object ob, PropertyDescriptor property)
		{
			SerializationData sdata = (ob is ISerializable) ? (ob as ISerializable).GetSerializationData(property.Name) : SerializationData.Default;
			if (property.SerializationVisibility == DesignerSerializationVisibility.Content)
			{
				DeserializeInto(xml, property.GetValue(ob), sdata);
			}
			else if (!property.IsReadOnly && property.SerializationVisibility != DesignerSerializationVisibility.Hidden)
			{
				object propVal = xml.Attribute("Type") != null ? Deserialize(xml) : Deserialize(xml, property.PropertyType, property.Converter);
				if (propVal != null && !property.PropertyType.IsAssignableFrom(propVal.GetType()) && property.Converter.CanConvertFrom(propVal.GetType()))
					propVal = property.Converter.ConvertFrom(propVal);
				if (sdata.PushValue != null)
					sdata.PushValue(propVal);
				else
					property.SetValue(ob, propVal);
			}
		}

		public void DeserializeInto(XElement xml, object ob, SerializationData sdata)
		{
			if (ob == null || !xml.HasElements)
				return;

			var properties = TypeDescriptor.GetProperties(ob);
			var propSelect = from x in (xml.Element("Properties") ?? s_EmptyElement).Elements() let p = properties[x.Name.LocalName] where p != null select new { Descriptor = p, Xml = x };
			foreach (var prop in propSelect)
			{
				Deserialize(prop.Xml, ob, prop.Descriptor);
			}

			var itemSelect = from x in (xml.Element("Collection") ?? s_EmptyElement).Elements() select Deserialize(x);
			if (sdata.PushValue != null)
			{
				sdata.PushValue(itemSelect.ToArray());
			}
			else if (ob is IList)
			{
				var obList = ob as IList;
				if (obList.IsFixedSize || obList.IsReadOnly)
					throw new SerializationException("Cannot deserialize FixedSize or Readonly IList");
				obList.Clear();
				foreach (var item in itemSelect)
					obList.Add(item);
			}
		}
	}

	public class SerializationException : Exception
	{
		internal SerializationException(string msg) : base(msg)
		{
		}
	}
	public interface ISerializable
	{
		SerializationData GetSerializationData(string prop);
	}
	class SerializableWrapper : ISerializable
	{
		public SerializationData GetSerializationData(string prop)
		{
			return SerializationData.Default;
		}

		public static ISerializable Instance
		{
			get { return s_Inst; }
		}

		static readonly SerializableWrapper s_Inst = new SerializableWrapper();
	}
}
