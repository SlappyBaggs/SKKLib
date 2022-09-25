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
	[MulticastAttributeUsage(MulticastTargets.Property)]
	public abstract class PropertyAspect : LaosAspect, ICompoundAspect
	{
		PropertyInfo targetProp_;
		protected PropertyInfo TargetProperty
		{
			get { return targetProp_; }
		}

		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidate(PropertyInfo target)
		{
			return true;
		}

		[CompileTimeSemantic]
		protected virtual void CompileTimeInitialize(PropertyInfo target)
		{
		}

		[CompileTimeSemantic]
		public override sealed bool CompileTimeValidate(object target)
		{
			PropertyInfo pi = target as PropertyInfo;
			return pi != null && CompileTimeValidate(pi);
		}

		[CompileTimeSemantic]
		void ICompoundAspect.CompileTimeInitialize(object element)
		{
			targetProp_ = (PropertyInfo)element;
			CompileTimeInitialize(targetProp_);
		}

		
		protected abstract void ProvideAspects(PropertyInfo target, LaosReflectionAspectCollection collection);
		void ILaosReflectionAspectProvider.ProvideAspects(LaosReflectionAspectCollection collection)
		{
			this.ProvideAspects(targetProp_, collection);
		}
	}

}
