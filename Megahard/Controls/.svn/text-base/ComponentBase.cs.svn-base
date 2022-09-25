using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Megahard.ComponentModel
{
	[Designer(typeof(Design.ComponentDesigner))]
	public class ComponentBase : Component, IPersistComponentSettings
	{
		public ComponentBase() { }

		protected T GetService<T>() where T : class
		{
			return GetService(typeof(T)) as T;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_settings != null && SaveSettings)
				{
					SaveComponentSettings();
				}
			}
			base.Dispose(disposing);
		}


		#region IPersistComponentSettings Support
		Configuration.ComponentSettings _settings;
		protected virtual void OnLoadComponentSettings(Configuration.ComponentSettings settings)
		{
		}
		
		public void LoadComponentSettings()
		{
			if (_settings == null)
				_settings = SaveSettings ? new Configuration.ComponentSettings(this) : null;
			if (_settings != null)
			{
				_settings.Reload();
				OnLoadComponentSettings(_settings);
			}
		}

		protected virtual void OnResetComponentSettings(Configuration.ComponentSettings settings)
		{
		}

		public void ResetComponentSettings()
		{
			if (_settings != null)
			{
				_settings.Reset();
				OnResetComponentSettings(_settings);
			}
		}

		protected virtual void OnSaveComponentSettings(Configuration.ComponentSettings settings)
		{
		}

		public void SaveComponentSettings()
		{
			if (_settings != null)
			{
				OnSaveComponentSettings(_settings);
				_settings.Save();
			}

		}

		[DefaultValue(false)]
		[Category("Application Settings")]
		public bool SaveSettings
		{
			get;
			set;
		}


		string _settingsKey;
		[DefaultValue(null)]
		[Category("Application Settings")]
		public string SettingsKey
		{
			get { return _settingsKey; }
			set
			{
				_settingsKey = value;
				if (_settings != null)
					_settings.SetSettingsKey(value);
			}
		}
		#endregion

		/// <summary>
		/// The control this component has been place on, keep in mind it can be null
		/// </summary>
		[DefaultValue(null)]
		[Browsable(false)]
		public ContainerControl ContainerControl
		{
			get;
			set;
		}

		public override ISite Site
		{
			get { return base.Site; }
			set
			{
				base.Site = value;
				if (value == null)
				{
					return;
				}

				var host = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (host != null)
				{
					var componentHost = host.RootComponent as ContainerControl;
					if (componentHost != null)
					{
						ContainerControl = componentHost;
					}
				}
			}
		}



	}
}
