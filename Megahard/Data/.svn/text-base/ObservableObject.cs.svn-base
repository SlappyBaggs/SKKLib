using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.ComponentModel.Design;
using Megahard.CodeDom;
using System.Reflection;

namespace Megahard.Data
{
	/// <summary>
	/// Implementation of IObservableObject, should be used whenever possible instead of implementing the interface yourself
	/// </summary>
	public partial class ObservableObject : IObservableObject, IDisposable
	{
		protected ObservableObject()
		{
		}

		/// <summary>
		/// Register a delegate to be called whenever the specified property has changed
		/// the arguments are oldval newval
		/// </summary>

		public AttachedObserver AttachObserver(PropertyPath prop, Action<ObjectChangedEventArgs> callback, Action<ObjectChangingEventArgs> changingCallback)
		{

			EventHandler<ObjectChangedEventArgs> eh = null;

			if (callback != null)
			{
				eh = (s, chg) =>
				{
					if (chg.PropertyName == prop)
						callback(chg);
				};
				ObjectChanged += eh;
			}
			EventHandler<ObjectChangingEventArgs> ehChanging = null;
			if (changingCallback != null)
			{
				ehChanging = (s, chging) =>
					{
						if (chging.PropertyName == prop)
							changingCallback(chging);
					};
				ObjectChanging += ehChanging;
			}

			Action detatch = () =>
				{
					if (ehChanging != null)
						ObjectChanging -= ehChanging;
					if (eh != null)
						ObjectChanged -= eh;
				};
			return new AttachedObserver(detatch);
		}
		public AttachedObserver AttachObserver(PropertyPath prop, Action<ObjectChangedEventArgs> callback)
		{
			return AttachObserver(prop, callback, null);
		}

		public AttachedObserver AttachObserver<T>(PropertyPath prop, Action<ObjectChangedEventArgs<T>> callback)
		{
			Action<ObjectChangedEventArgs> newCallback = chg=>
				{
					if (chg is ObjectChangedEventArgs<T>)
						callback((ObjectChangedEventArgs<T>)chg);
				};
			return AttachObserver(prop, newCallback, null);
		}

		ChildObservables childObs_;
		
		#region Dispose HandlingCode

		void IDisposable.Dispose()
		{
			Dispose();
		}
		protected virtual void OnDispose()
		{
		}
		public void Dispose()
		{
			try
			{
				OnDispose();
			}
			finally
			{
				if (childObs_ != null)
				{
					childObs_.Dispose();
					childObs_ = null;
				}


				ehObjectChanged_.Dispose();
				ehObjectChanging_.Dispose();
				propChg_ = null;
				propChanging_ = null;
			}
		}

		#endregion
		
		//<SyncEvent Name="ObjectChanging" Category="Observation"/>
		//<SyncEvent Name="ObjectChanged" Category="Observation"/>

		partial void PreRaiseObjectChanging(ObjectChangingEventArgs args, ref bool fireEvent)
		{
			if (!args.IsChild && args.NewValue is IObservableObject)
			{
				RegisterChildObservable(args.PropertyName, args.NewValue as IObservableObject);
			}
			fireEvent = CanRaiseChangeEvents;
		}

		partial void PostRaiseObjectChanging(ObjectChangingEventArgs args)
		{
			var copy = propChanging_;
			if (copy != null)
				copy(this, args);
		}

		/// <summary>
		/// The object used as sender when firing Changed and Changing events
		/// </summary>
		protected virtual object EventSenderObject
		{
			get { return this; }
		}

		protected void RegisterChildObservable(PropertyPath prop, IObservableObject ob)
		{
			if (childObs_ == null)
				childObs_ = new ChildObservables(PropertyObject_ObjectChanged, PropertyObject_ObjectChanging);
			childObs_.RegisterObservableProperty(prop, ob);
		}


		partial void PreRaiseObjectChanged(ObjectChangedEventArgs args, ref bool fireEvent)
		{
			if (childObs_ != null && args.OldValue is IObservableObject && !args.IsChild)
			{
				childObs_.UnRegisterObservableProperty(args.OldValue as IObservableObject);
			}
			fireEvent = CanRaiseChangeEvents;
		}

		partial void PostRaiseObjectChanged(ObjectChangedEventArgs args)
		{
			var copy = propChg_;
			if (copy != null)
				copy(this, args);
		}


		protected void IndicateObjectReset()
		{
			RaiseObjectChanged(ObjectChangedEventArgs.ObjectReset);
		}

		PropertyChangedEventHandler propChg_;
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { propChg_ += value; }
			remove { propChg_ -= value; }
		}

		PropertyChangingEventHandler propChanging_;
		event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
		{
			add { propChanging_ += value; }
			remove { propChanging_ -= value; }
		}

		void PropertyObject_ObjectChanging(object sender, ObjectChangingEventArgs e)
		{
			RaiseObjectChanging(e.CreateChildInternal(childObs_.GetPropertyName(sender as IObservableObject)));
		}

		void PropertyObject_ObjectChanged(object sender, ObjectChangedEventArgs e)
		{
			RaiseObjectChanged(e.CreateChildInternal(childObs_.GetPropertyName(sender as IObservableObject)));
		}

		[Browsable(false)]
		protected virtual bool CanRaiseChangeEvents
		{
			get { return suspendChgEvents_ == 0; }
		}

		int suspendChgEvents_ = 0;
		protected void SuspendChangeEvents()
		{
			suspendChgEvents_ += 1;
		}

		protected void ResumeChangeEvents()
		{
			suspendChgEvents_ -= 1;
			if (suspendChgEvents_ <= 0)
				suspendChgEvents_ = 0;
		}

		protected struct PropertyBacking<T>
		{
			public PropertyBacking(string prop) : this()
			{
				prop_ = prop;
			}
			public PropertyBacking(string prop, T val)
				: this()
			{
				prop_ = prop;
				val_ = val;
			}

			T val_;
			readonly PropertyPath prop_;
			public PropertyPath Name
			{
				get { return prop_; }
			}
			public bool WouldChange(T value)
			{
				return !object.Equals(val_, value);
			}

			/// <summary>
			/// never returns null
			/// </summary>
			public ObjectChangedEventArgs<T> SetValueNoEqualCheck(ObservableObject owner, T value)
			{
				owner.RaiseObjectChanging(new ObjectChangingEventArgs(prop_, value));
				var old = val_;
				val_ = value;
				var chged = new ObjectChangedEventArgs<T>(prop_, old, value);
				owner.RaiseObjectChanged(chged);
				return chged;
			}

			/// <summary>
			/// returns null if the prop already had the specified value
			/// </summary>
			public ObjectChangedEventArgs<T> SetValue(ObservableObject owner, T value)
			{
				if(WouldChange(value))
					return SetValueNoEqualCheck(owner, value);
				return null;
			}

			public T GetValue()
			{
				return val_;
			}
		}

	}

	[System.Diagnostics.DebuggerDisplay("{PropertyPath}")]
	public class ObjectChangingEventArgs : System.ComponentModel.PropertyChangingEventArgs
	{
		public ObjectChangingEventArgs(PropertyPath prop, object newValue, object extra)
			: base(prop != null ? prop.Root : null)
		{
			PropertyName = prop;
			NewValue = newValue;
			ExtraInfo = extra;
		}
		public ObjectChangingEventArgs(PropertyPath prop, object newValue) : this(prop, newValue, null)
		{
		}

		ObjectChangingEventArgs()
			: base(null)
		{
		}

		bool child_;
		internal bool IsChild
		{
			get { return child_; }
		}

		internal ObjectChangingEventArgs CreateChildInternal(PropertyPath parentProperty)
		{
			var ret = CreateChild(parentProperty);
			ret.child_ = true;
			return ret;
		}
		public virtual ObjectChangingEventArgs CreateChild(PropertyPath parentProperty)
		{
			return new ObjectChangingEventArgs(parentProperty + PropertyName, NewValue, ExtraInfo);
		}

		public virtual ObjectChangingEventArgs RemovePropertyName()
		{
			return new ObjectChangingEventArgs(null, NewValue, ExtraInfo);
		}
		public virtual ObjectChangingEventArgs RemovePropChangeValue(object newValue)
		{
			return new ObjectChangingEventArgs(null, newValue, ExtraInfo);
		}

		public object NewValue { get; private set; }
		public new PropertyPath PropertyName { get; private set; }

		/// <summary>
		/// Just some extra data that can be tacked on for usage as needed
		/// </summary>
		public object ExtraInfo { get; private set; }


		static readonly ObjectChangingEventArgs s_Empty = new ObjectChangingEventArgs();

		public new static ObjectChangingEventArgs Empty { get { return s_Empty; } }
	}

	[System.Diagnostics.DebuggerDisplay("{PropertyName}")]
	public class ObjectChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
	{
		public ObjectChangedEventArgs(PropertyPath prop, object oldValue, object newValue, object extra)
			: base(prop != null ? prop.Root : null)
		{
			PropertyName = prop;
			OldValue = oldValue;
			NewValue = newValue;
			ExtraInfo = extra;

			Type oldType = oldValue == null ? null : oldValue.GetType();
			Type newType = newValue == null ? null : newValue.GetType();

			propTypeChanged_ = oldType != newType;
		}
		public ObjectChangedEventArgs(PropertyPath prop, object oldValue, object newValue) : this(prop, oldValue, newValue, null)
		{
		}

		public ObjectChangedEventArgs(PropertyPath prop) : this(prop, StandardValues.UnavailableValue, StandardValues.UnavailableValue, null)
		{
		}
		ObjectChangedEventArgs()
			: base(null)
		{
			IsObjectReset = true;
		}

		bool child_;
		internal bool IsChild
		{
			get { return child_; }
		}

		internal ObjectChangedEventArgs CreateChildInternal(PropertyPath parentProperty)
		{
			var ret = CreateChild(parentProperty);
			ret.child_ = true;
			ret.IsObjectReset = IsObjectReset;
			return ret;
		}

		public virtual ObjectChangedEventArgs CreateChild(PropertyPath parentProperty)
		{
			return new ObjectChangedEventArgs(parentProperty + PropertyName, OldValue, NewValue, ExtraInfo);
		}

		public virtual ObjectChangedEventArgs RemovePropertyName()
		{
			return new ObjectChangedEventArgs(null, OldValue, NewValue, ExtraInfo) { IsObjectReset = IsObjectReset };
		}
		public virtual ObjectChangedEventArgs RemovePropChangeValue(object oldValue, object newValue)
		{
			return new ObjectChangedEventArgs(null, oldValue, newValue, ExtraInfo) { IsObjectReset = IsObjectReset };
		}


		#region bool TypeChanged { get; readonly; }
		readonly bool propTypeChanged_;

		/// <summary>
		/// This is true if the value obtain from the property is of a different type than it used to be
		/// </summary>
		public bool TypeChanged
		{
			get { return propTypeChanged_; }
		}
		#endregion

		/// <summary>
		/// If the OldValue is not known or cannot be obtained due to complexity speed issues, then it should be set to 
		/// StandardValues.UnavailableValue, use null for when the value really is null
		/// </summary>
		public object OldValue { get; private set; }
		public object NewValue { get; private set; }

		/// <summary>
		/// Just some extra data that can be tacked on for usage as needed
		/// </summary>
		public object ExtraInfo { get; private set; }
		
		public new PropertyPath PropertyName { get; private set; }

		static readonly ObjectChangedEventArgs s_Reset = new ObjectChangedEventArgs();
		internal static ObjectChangedEventArgs ObjectReset { get { return s_Reset; } }

		public bool IsObjectReset
		{
			get;
			private set;
		}
	}

	public class ObjectChangedEventArgs<T> : ObjectChangedEventArgs
	{
		public ObjectChangedEventArgs(PropertyPath prop, T oldValue, T newValue, object extra) : base(prop, oldValue, newValue, extra)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
		public ObjectChangedEventArgs(PropertyPath prop, T oldValue, T newValue)
			: this(prop, oldValue, newValue, null)
		{
		}
		public new T OldValue { get; private set; }
		public new T NewValue { get; private set; }
	}

}

namespace Megahard.Data.Experimental
{

	class Change
	{
	}

	// PropertyChange
	class PropertyChange
	{
		public PropertyPath Property { get; set; }
	}
	class PropertyChange<T> : PropertyChange
	{
		public T OldValue { get; set; }
		public T NewValue { get; set; }
	}
	
	class NewObjectChanged<T>
	{
		IEnumerable<PropertyChange> PropertyChanges;
	}

	interface IObservationLink
	{
		void DestroyLink();
		void PauseLink();
	}
	class NewObservableObject
	{
		//IObservationLink AttachObserver(Action notifier);
	}
}
