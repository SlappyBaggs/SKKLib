using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
namespace Megahard.Data
{
	// Should it be a struct?
	// Unbound data is handled fairly well, except for the case of null unbound data, not so sure about that one yet
	[TypeConverter(typeof(DataObject.Converter))]
	public sealed class DataObject : ObservableObject, ICustomTypeDescriptor, ISupportExtendedDataBinding, IResetable
	{
		[SmartConverter]
		public DataObject(object value) : this(value, null, false) { }
		public DataObject(object value, bool readOnly) : this(value, null, readOnly) { }
		public DataObject(object instance, PropertyPath property) : this(instance, property, false) { }

		public DataObject(object instance, PropertyPath property, bool readOnly)
		{
			instance_ = instance;
			prop_ = property;
			readonly_ = readOnly;

			if (!prop_.IsEmpty)
			{
				if (instance_ is IObservableObject)
				{
					var observable = instance_ as IObservableObject;
					observable.ObjectChanged += Instance_ObjectChanged;
					observable.ObjectChanging += Instance_ObjectChanging;
				}
				else if (instance_ is INotifyPropertyChanged)
				{
					var notifier = instance_ as INotifyPropertyChanged;
					notifier.PropertyChanged += Instance_PropertyChanged;
				}
				if (!readonly_ && instance_ != null)
				{
					try
					{
						readonly_ = prop_.ResolveProperty(instance_).IsReadOnly;
					}
					catch (PropertyPath.FailedToResolvePropertyException)
					{
					}
				}
			}


			object val = null;
			try
			{
				val = GetValue();
			}
			catch
			{
			}
			if (val is IObservableObject)
				RegisterChildObservable(null, val as IObservableObject);
		}

		void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (suspendInstanceEvents_ || sender != instance_)
				return;

			var propPath = (PropertyPath)e.PropertyName;
			if (propPath == prop_)
				RaiseObjectChanged(new ObjectChangedEventArgs(null, null, null));

			if (propPath.IsAncestorOf(prop_))
			{
				RaiseObjectChanged(new ObjectChangedEventArgs(null, null, null));
			}			
		}

		public void InvokeEditor()
		{
			
		}


		protected override void OnDispose()
		{
			if (instance_ is IObservableObject)
			{
				(instance_ as IObservableObject).ObjectChanged -= Instance_ObjectChanged;
				(instance_ as IObservableObject).ObjectChanging -= Instance_ObjectChanging;
			}
			else if (instance_ is INotifyPropertyChanged)
			{
				(instance_ as INotifyPropertyChanged).PropertyChanged -= Instance_PropertyChanged;
			}

			base.OnDispose();
		}

		Type dataType_;
		public Type GetDataType()
		{
			// if it is null then calculate it
			if (dataType_ == null)
			{
				try
				{
					if (prop_.IsEmpty)
					{
						object val = GetValue();
						dataType_ = (val == null ? typeof(object) : val.GetType());
					}
					else
					{
						dataType_ = PropertyDescriptor.PropertyType;
					}
				}
				catch
				{
					return typeof(object);
				}
			}
			return dataType_;
		}

		bool suspendInstanceEvents_;
		void Instance_ObjectChanged(object sender, ObjectChangedEventArgs e)
		{
			if (suspendInstanceEvents_ || sender != instance_)
				return;

			if (e == ObjectChangedEventArgs.ObjectReset)
			{
				RaiseObjectChanged(e);
				return;
			}
			if (e.PropertyName == prop_)
				RaiseObjectChanged(e.RemovePropertyName());

			if (e.PropertyName.IsAncestorOf(prop_))
			{
				try
				{
					var diff = PropertyPath.DifferencePath(prop_, e.PropertyName);

					object newVal = null;
					object oldVal = null;

					try
					{
						newVal = diff.GetValue(e.NewValue);
					}
					catch (PropertyPath.FailedToResolvePropertyException)
					{
					}

					try
					{
						oldVal = diff.GetValue(e.OldValue);
					}
					catch (PropertyPath.FailedToResolvePropertyException)
					{
					}

					RaiseObjectChanged(e.RemovePropChangeValue(oldVal, newVal));
				}
				catch (PropertyPath.FailedToResolvePropertyException)
				{
				}
			}
		}

		void Instance_ObjectChanging(object sender, ObjectChangingEventArgs e)
		{
			if (suspendInstanceEvents_ || sender != instance_)
				return;
			if (e.PropertyName == prop_)
				RaiseObjectChanging(e.RemovePropertyName());

			if (e.PropertyName.IsAncestorOf(prop_))
			{
				try
				{
					var diff = PropertyPath.DifferencePath(prop_, e.PropertyName);

					object newVal = null;
					try
					{
						newVal = diff.GetValue(e.NewValue);
					}
					catch (PropertyPath.FailedToResolvePropertyException)
					{
					}
					RaiseObjectChanging(e.RemovePropChangeValue(newVal));
				}
				catch (PropertyPath.FailedToResolvePropertyException)
				{
				}
			}

		}

		public VisualEditor CreateVisualEditor()
		{
			return new Visualization.VisualUITypeEditor(new DataObject(instance_, prop_, readonly_));
		}

		object instance_;
		readonly PropertyPath prop_;
		readonly bool readonly_;

		public bool IsReadOnly
		{
			get { return readonly_; }
		}

		public object GetValue() 
		{ 
			return prop_.GetValue(instance_);
		}
		object SafeGetValue()
		{
			try
			{
				return GetValue();
			}
			catch
			{
				return null;
			}
		}

		public object GetValue(PropertyPath childProp) { return childProp.GetValue(GetValue());	}

		public T GetValue<T>() { return (T)GetValue(); }
		public T GetValueAs<T>()
		{
			var val = GetValue();
			if (val is T)
				return (T)val;
			return default(T);
		}

		public T GetValue<T>(PropertyPath childProp) { return (T)GetValue(childProp); }
		public T GetValueAs<T>(PropertyPath childProp) where T : class { return GetValue(childProp) as T; }

		public void SetValue(object val)
		{
			if (IsReadOnly)
				throw new NotSupportedException("DataObject is read only, cannot set value");

			var oldVal = GetValue();
			if(object.Equals(oldVal, val))
				return;
			OnObjectChanging(new ObjectChangingEventArgs(null, val));
			if (prop_.IsEmpty)
			{
				object convertedVal;
				if (instance_ != null && SmartConvert.ConvertTo(val, instance_.GetType(), out convertedVal))
					val = convertedVal;
				instance_ = val;
			}
			else
			{
				try
				{
					suspendInstanceEvents_ = true;
					prop_.SetValue(instance_, val);
				}
				finally
				{
					suspendInstanceEvents_ = false;
				}
			}

			dataType_ = null; // null out datatype so it will be recalculated next time it is asked for, could do a check to see if it actually changed and only
							  // null it out if it did
			RaiseObjectChanged(new ObjectChangedEventArgs(null, oldVal, val));
		}

		public void SetValue(object val, PropertyPath childProp) { childProp.SetValue(GetValue(), val); }

		public void GetBindingDetails(out object ob, out PropertyPath property)
		{
			ob = instance_;
			property = prop_;
		}

		public bool BindingEnabled
		{
			get
			{
				if (instance_ is ISupportExtendedDataBinding)
					return (instance_ as ISupportExtendedDataBinding).BindingEnabled;
				return true;
			}
		}


		class Converter : ComponentModel.CompositeTypeConverter
		{
			static Converter()
			{
				s_ctor1 = typeof(DataObject).GetConstructor<object>();
				s_ctor2 = typeof(DataObject).GetConstructor<object, bool>();
				s_ctor3 = typeof(DataObject).GetConstructor<object, PropertyPath>();
				s_ctor4 = typeof(DataObject).GetConstructor<object, PropertyPath, bool>();
			}

			public Converter() { }
			public Converter(TypeConverter dataConverter) : base(dataConverter) { }

			static System.Reflection.ConstructorInfo s_ctor1, s_ctor2, s_ctor3, s_ctor4;

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					var data = (DataObject)value;

					if (data.prop_.IsEmpty)
					{
						if (!data.IsReadOnly)
							return new InstanceDescriptor(s_ctor1, new[] { data.instance_ });
						return new InstanceDescriptor(s_ctor2, new[] { data.instance_, true });
					}
					else
					{
						if (!data.IsReadOnly)
							return new InstanceDescriptor(s_ctor3, new[] { data.instance_, data.prop_ });
						return new InstanceDescriptor(s_ctor4, new[] { data.instance_, data.prop_, true });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		#region ICustomTypeDescriptor Members

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			AttributeCollection col = null;
			var prop = PropertyDescriptor;
			if (prop != null)
			{
				col = prop.Attributes;
			}
			else
			{
				object val = SafeGetValue();
				if (val != null)
					col = TypeDescriptor.GetAttributes(val);
			}
			if (col != null)
			{
				List<Attribute> attrs = new List<Attribute>();
				foreach (Attribute attr in col)
				{
					if (attr.GetType() != typeof(DesignerSerializerAttribute))
						attrs.Add(attr);
				}
				//attrs.Add(new DesignerSerializerAttribute(typeof(Serializer), typeof(CodeDomSerializer)));
				return new AttributeCollection(attrs.ToArray());
			}
			else
			{
				return new AttributeCollection(new Attribute[0]);
			}

		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			var prop = PropertyDescriptor;
			if (prop != null)
				return prop.DisplayName;
			var val = SafeGetValue();
			if(val != null)
				return TypeDescriptor.GetComponentName(val);
			return TypeDescriptor.GetComponentName(this, true);
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			TypeConverter converter = null;
			var prop = PropertyDescriptor;
			if (prop != null && prop.Converter != null && prop.Converter.GetType() != typeof(TypeConverter))
			{
				converter = prop.Converter;
			}
			else
			{
				var val = SafeGetValue();
				if (val == null)
					converter = prop != null ? prop.Converter : new TypeConverter();
				else
					converter = TypeDescriptor.GetConverter(val);
			}
			return new Converter(converter);
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(SafeGetValue());
		}

		object GetEditor(Type editorBaseType)
		{
			var prop = PropertyDescriptor;
			if (prop != null)
			{
				var ed = prop.GetEditor(editorBaseType);
				if (ed != null)
					return ed;
			}
			var val = SafeGetValue();
			if (val != null)
				return TypeDescriptor.GetEditor(val, editorBaseType);
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return GetEditor(editorBaseType);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(SafeGetValue(), attributes);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(SafeGetValue());
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			var prop = PropertyDescriptor;
			var val = SafeGetValue();
			if (prop != null)
			{
				if(val != null)
					return ComponentModel.WrappedPropertyDescriptor.WrapProperties(prop.GetChildProperties(val, attributes), val);
			}
			if(val != null)
				return ComponentModel.WrappedPropertyDescriptor.WrapProperties(val, attributes);
			return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return (this as ICustomTypeDescriptor).GetProperties(null);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return SafeGetValue();
		}

		#endregion

		PropertyDescriptor PropertyDescriptor
		{
			get 
			{
				try
				{
					return prop_.ResolveProperty(instance_);
				}
				catch (PropertyPath.FailedToResolvePropertyException)
				{
					return null;	
				}
			}
		}

		/*
		class Serializer : CodeDomSerializer
		{

			public override object Serialize(IDesignerSerializationManager manager, object value)
			{
				bool complete;
				return base.SerializeCreationExpression(manager, value, out complete);
			}
		}
		*/

		#region IResetable Members

		public void Reset()
		{
			bool fireChangedEvent = false;
			try
			{
				suspendInstanceEvents_ = true;
				if (prop_.IsEmpty)
				{
					if (instance_ is IResetable)
					{
						(instance_ as IResetable).Reset();
						fireChangedEvent = true;
					}
				}
				else
				{
					object ob = instance_;
					var pd = prop_.ResolveProperty(ref ob);
					if (pd.CanResetValue(ob))
					{
						pd.ResetValue(ob);
						fireChangedEvent = true;
					}
					else
					{
						var val = pd.GetValue(ob);
						if (val is IResetable)
						{
							(val as IResetable).Reset();
							if (!pd.IsReadOnly)
								pd.SetValue(ob, val);
							fireChangedEvent = true;
						}
					}
				}
			}
			catch (PropertyPath.FailedToResolvePropertyException)
			{
			}
			finally
			{
				suspendInstanceEvents_ = false;
			}
			if(fireChangedEvent)
				RaiseObjectChanged(new ObjectChangedEventArgs(null));
		}

		public bool IsDefault
		{
			get 
			{
				try
				{
					if (prop_.IsEmpty)
					{
						if (instance_ is IResetable)
						{
							return (instance_ as IResetable).IsDefault;
						}
					}
					else
					{
						object ob = instance_;
						var pd = prop_.ResolveProperty(ref ob);
						if (pd.CanResetValue(ob))
							return false;
						var val = pd.GetValue(ob);
						if (val is IResetable)
							return (val as IResetable).IsDefault;
					}
				}
				catch (PropertyPath.FailedToResolvePropertyException)
				{
				}
				return true;
			}
		}

		#endregion
	}

	public interface ISupportExtendedDataBinding
	{
		bool BindingEnabled
		{
			get;
		}
	}

	public abstract class VisualEditor
	{
		public abstract void DisplayModal();
	}

	

}
