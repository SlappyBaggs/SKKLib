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
using System.Configuration;
using System.Windows.Forms;

namespace Megahard.Data
{
	[Serializable]
	[TypeConverter(typeof(ComponentConverter)), 
		//RootDesignerSerializer("System.ComponentModel.Design.Serialization.RootCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
							   //"System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
								//true),
		DesignerSerializer(typeof(CodeDomSerializer), typeof(CodeDomSerializer)),
		Designer(typeof(Design.ComponentDesigner)), Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerCategory("Component")]
	public partial class ObservableComponent : ObservableObject, ISupportInitialize, IComponent, System.Configuration.IPersistComponentSettings
	{
		public ObservableComponent()
		{
			ehDisposed_ = new Megahard.Threading.SynchronizedEventBacking(lockOb_);
		}
		public ObservableComponent(IContainer cont)
		{
			if (cont != null)
				cont.Add(this);
			ehDisposed_ = new Megahard.Threading.SynchronizedEventBacking(lockOb_);
		}
		~ObservableComponent()
		{
			DisposeInternal(false);
		}

		protected virtual void Dispose(bool disposing) { }

		sealed protected override void OnDispose()
		{
			DisposeInternal(true);
			base.OnDispose();
			GC.SuppressFinalize(this);
		}

		void DisposeInternal(bool disposing)
		{
			Dispose(disposing);
			if (disposing)
			{
				if (settings_ != null && SaveSettings)
				{
					SaveComponentSettings();
				}

				using(lockOb_.Lock())
				{
					disposeState_ = DisposeState.Disposing;
					if (_site != null && _site.Container != null)
						_site.Container.Remove(this);
					disposeState_ = DisposeState.Disposed;
				}
			}
		}


		enum DisposeState { NotDisposed, Disposing, Disposed };
		DisposeState disposeState_;
		protected bool IsDisposing
		{
			get { return disposeState_ == DisposeState.Disposing; }
		}

		protected bool IsDisposed
		{
			get { return disposeState_ == DisposeState.Disposed; }
		}

		readonly Megahard.Threading.SyncLock lockOb_ = new Megahard.Threading.SyncLock();

		[Browsable(false)]
		public IContainer Container
		{
			get
			{
				ISite site = _site;
				if (site != null)
					return site.Container;
				return null;
			}
		}

		protected bool DesignMode
		{
			get
			{
				var site = _site;
				return site != null && site.DesignMode;
			}
		}

		//<SyncEvent Name="Disposed" NoArgs="true"/>

		private ISite _site;
		[Browsable(false)]
		public virtual ISite Site
		{
			get	{ return _site;	}
			set
			{
				_site = value;
				if (_site != null)
				{
					var host = _site.GetService(typeof(IDesignerHost)) as IDesignerHost;
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

		protected virtual object GetService(Type service)
		{
			ISite site = _site;
			if (site != null)
				return _site.GetService(service);
			return null;
		}

		public override string ToString()
		{
			ISite site = _site;
			if (site != null)
			{
				return (site.Name + " [" + GetType().FullName + "]");
			}
			return GetType().FullName;
		}

		bool initializing_;

		protected virtual void OnBeginInit() { }
		protected virtual void OnEndInit() { }

		void ISupportInitialize.BeginInit()
		{
			initializing_ = true;
			OnBeginInit();
		}

		void ISupportInitialize.EndInit()
		{
			System.Diagnostics.Debug.Assert(initializing_);
			initializing_ = false;
			OnEndInit();
		}

		[Browsable(false)]
		public bool IsInitialized
		{
			get { return initializing_ == false; }
		}


		[Browsable(false)]
		protected override bool CanRaiseChangeEvents
		{
			get { return base.CanRaiseChangeEvents && IsInitialized; }
		}
		[DefaultValue(null)]
		[Browsable(false)]
		public ContainerControl ContainerControl
		{
			get;
			set;
		}


		#region IPersistComponentSettings Support
		Configuration.ComponentSettings settings_;
		protected virtual void OnLoadComponentSettings(Configuration.ComponentSettings settings)
		{
		}

		protected void ReinitializedComponentSettings()
		{
			if (SaveSettings)
			{
				settings_ = new Configuration.ComponentSettings(this);
				settings_.Reload();
				OnLoadComponentSettings(settings_);
			}
		}
		public void LoadComponentSettings()
		{
			if (settings_ == null)
				settings_ = SaveSettings ? new Configuration.ComponentSettings(this) : null;
			if (settings_ != null)
			{
				settings_.Reload();
				OnLoadComponentSettings(settings_);
			}
		}

		protected virtual void OnResetComponentSettings(Configuration.ComponentSettings settings)
		{
		}

		public void ResetComponentSettings()
		{
			if (settings_ != null)
			{
				settings_.Reset();
				OnResetComponentSettings(settings_);
			}
		}

		protected virtual void OnSaveComponentSettings(Configuration.ComponentSettings settings)
		{
		}

		public void SaveComponentSettings()
		{
			if (settings_ != null)
			{
				OnSaveComponentSettings(settings_);
				settings_.Save();
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
				if (settings_ != null)
					settings_.SetSettingsKey(value);
			}
		}

		#endregion
	}
	
	class ChildObservables : IDisposable
	{
		public ChildObservables(EventHandler<ObjectChangedEventArgs> chg, EventHandler<ObjectChangingEventArgs> changing)
		{
			chg_ = chg;
			changing_ = changing;
		}

		readonly Dictionary<IObservableObject, PropertyPath> dict_ = new Dictionary<IObservableObject, PropertyPath>();
		readonly EventHandler<ObjectChangedEventArgs> chg_;
		readonly EventHandler<ObjectChangingEventArgs> changing_;

		public void RegisterObservableProperty(PropertyPath prop, IObservableObject ob)
		{
			dict_[ob] = prop;
			ob.ObjectChanged += chg_;
			ob.ObjectChanging += changing_;
		}

		public void UnRegisterObservableProperty(IObservableObject ob)
		{
			dict_.Remove(ob);
			ob.ObjectChanged -= chg_;
			ob.ObjectChanging -= changing_;
		}

		public PropertyPath GetPropertyName(IObservableObject ob)
		{
			return dict_[ob];
		}

		#region IDisposable Members

		public void Dispose()
		{
			foreach (IObservableObject ob in dict_.Keys)
			{
				ob.ObjectChanged -= chg_;
				ob.ObjectChanging -= changing_;
			}
		}

		#endregion
	}
}
