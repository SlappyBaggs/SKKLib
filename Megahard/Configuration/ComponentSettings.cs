using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace Megahard.Configuration
{
	public class ComponentSettings : ApplicationSettingsBase
	{
		public ComponentSettings(IComponent owner) : base(owner)
		{
			_owner = owner;
			if (owner is IPersistComponentSettings)
			{
				SetSettingsKey((owner as IPersistComponentSettings).SettingsKey);
			}
			var props = from prop in _owner.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
						let attr = prop.GetCustomAttribute<SaveAsSettingAttribute>(true)
						where prop.CanRead && prop.CanWrite && attr != null && attr.Save
						select prop;

			var templateProp = base.Properties["DummyProp"];
			foreach (var propInfo in props)
			{
				var prop = new Property(templateProp, propInfo, owner);
				_props.Add(prop);
				_vals.Add(prop.SettingsValue);
			}
			_props.SetReadOnly();
		}

		public void SetSettingsKey(string key)
		{
			base.SettingsKey = key;
		}
	
		void Load()
		{
			foreach (SettingsProvider provider in base.Providers)
			{
				var vals = provider.GetPropertyValues(Context, Properties);
				foreach (SettingsPropertyValue val in vals)
				{
					(val.Property as Property).SetValue(val.PropertyValue);
					var pval = PropertyValues[val.Property.Name];
					pval.PropertyValue = val.PropertyValue;
					pval.IsDirty = false;
				}
			}
		}

		protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			EnsureValues();
			base.OnPropertyChanged(sender, e);
		}

		public override void Save()
		{
			foreach (Property prop in Properties)
			{
				var currentVal = prop.GetValue();
				var propVal = PropertyValues[prop.Name];
				if (!object.Equals(currentVal, propVal.PropertyValue))
					propVal.PropertyValue = currentVal;
			}
			base.Save();
		}

		[UserScopedSetting]
		public bool DummyProp
		{
			get { return false; }
		}

		readonly IComponent _owner;
		readonly SettingsPropertyCollection _props = new SettingsPropertyCollection();
		readonly SettingsPropertyValueCollection _vals = new SettingsPropertyValueCollection();
		public override SettingsPropertyCollection Properties
		{
			get
			{
				return _props;
			}
		}

		void EnsureValues()
		{
			if (_vals.Count == 0)
			{
				foreach (Property prop in Properties)
				{
					_vals.Add(prop.SettingsValue);
				}
				Load();
			}
		}
		public override SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				EnsureValues();
				return _vals;
			}
		}

		public override object this[string propertyName]
		{
			get
			{
				var pval = PropertyValues[propertyName];
				if (pval != null)
					return pval.PropertyValue;
				return null;
			}
			set
			{
				var pval = PropertyValues[propertyName];
				if (pval != null)
					pval.PropertyValue = value;
			}
		}

		class Property : SettingsProperty
		{
			public Property(SettingsProperty copyFrom, PropertyInfo propInfo, object owner)
				: base(copyFrom)
			{
				_propInfo = propInfo;
				_owner = owner;
				base.Name = propInfo.Name;
				base.PropertyType = propInfo.PropertyType;
				base.DefaultValue = GetValue();
				_settingsVal = new SettingsPropertyValue(this);
			}
			public object GetValue()
			{
				return _propInfo.GetValue(_owner, null);
			}

			public void SetValue(object val)
			{
				_propInfo.SetValue(_owner, val, null);
			}

			public SettingsPropertyValue SettingsValue
			{
				get { return _settingsVal; }
			}

			readonly SettingsPropertyValue _settingsVal;
			readonly PropertyInfo _propInfo;
			readonly object _owner;
		}
	}
}