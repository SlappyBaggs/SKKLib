using System.ComponentModel;
using System;
using System.Collections.Generic;
namespace Megahard.Extenders
{
	public abstract class ExtenderBase<T, PropType> : ComponentModel.ComponentBase, IExtenderProvider where T : class
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && typeof(IDisposable).IsAssignableFrom(typeof(PropType)))
			{
				foreach (PropType val in dict_.Values)
				{
					(val as IDisposable).Dispose();
				}
			}
			base.Dispose(disposing);
		}
		bool IExtenderProvider.CanExtend(object extendee)
		{
			return extendee is T && CanExtend(extendee as T);
		}

		protected virtual bool CanExtend(T extendee)
		{
			return true;
		}

		protected PropType GetValue(T extendee)
		{
			if (dict_.ContainsKey(extendee))
				return dict_[extendee];
			return default(PropType);
		}

		protected void SetValue(T extendee, PropType val)
		{
			dict_[extendee] = val;
			OnSet(extendee, val);
		}

		protected abstract void OnSet(T extendee, PropType val);

		readonly Dictionary<T, PropType> dict_ = new Dictionary<T, PropType>();
	}
}
