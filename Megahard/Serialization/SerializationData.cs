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
	public struct SerializationData
	{
		public SerializationData(bool? serialize) : this()
		{
			ShouldSerialize = serialize;
		}
		/// <summary>
		/// null value means no preference, (so choice will be made by serialization engine)
		/// </summary>
		public bool? ShouldSerialize { get; private set; }

		internal bool EvalShouldSerialize(object ob, PropertyDescriptor prop)
		{
			if (ShouldSerialize.HasValue)
				return (bool)ShouldSerialize;
			if (prop.SerializationVisibility == DesignerSerializationVisibility.Hidden)
				return false;
			if (prop.IsReadOnly && prop.SerializationVisibility != DesignerSerializationVisibility.Content)
				return false;
			return prop.ShouldSerializeValue(ob);
		}

		internal object GetValueToSerialize(object ob, PropertyDescriptor prop)
		{
			if (PullValue != null)
				return PullValue;
			return prop.GetValue(ob);
		}

		/// <summary>
		/// If non null, use this delegate to set the deserialized data back into the object
		/// </summary>
		public Action<object> PushValue { get; set; }

		/// <summary>
		/// If non null, use this delegate to obtain the data which will be serialized (not currently used)
		/// </summary>
		public Func<object> PullValue { get; set; }

		public Type SerializedType { get; set; }

		static readonly SerializationData s_Yes = new SerializationData(true);
		static readonly SerializationData s_No = new SerializationData(false);
		static readonly SerializationData s_Default = new SerializationData(null);

		public static SerializationData Yes { get { return s_Yes; } }
		public static SerializationData No { get { return s_No; } }
		public static SerializationData Default { get { return s_Default; } }
	}


}