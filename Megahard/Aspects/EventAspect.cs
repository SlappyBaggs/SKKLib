using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Laos;
using PostSharp.Extensibility;
using System.Reflection;

namespace Megahard.Aspects
{
	[Serializable]
	[MulticastAttributeUsage(MulticastTargets.Event)]
	public abstract class EventAspect : LaosAspect, ICompoundAspect
	{
		[NonSerialized]
		EventInfo targetEvent_;

		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidate(EventInfo target)
		{
			return true;
		}
		
		[CompileTimeSemantic]
		protected virtual void CompileTimeInitialize(EventInfo element)
		{
		}

		[CompileTimeSemantic]
		public override sealed bool CompileTimeValidate(object target)
		{
			EventInfo ev = target as EventInfo;
			return ev != null && CompileTimeValidate(ev);
		}

		void ICompoundAspect.CompileTimeInitialize(object element)
		{
			targetEvent_ = (EventInfo)element;
			CompileTimeInitialize(targetEvent_);
		}

		protected abstract void ProvideAspects(EventInfo target, LaosReflectionAspectCollection collection);
		void ILaosReflectionAspectProvider.ProvideAspects(LaosReflectionAspectCollection collection)
		{
			this.ProvideAspects(targetEvent_, collection);
		}
	}
}
