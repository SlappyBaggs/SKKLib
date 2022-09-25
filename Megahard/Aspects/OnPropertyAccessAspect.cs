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
	public abstract class OnPropertyAccessAspect : PropertyAspect, ICompoundAspect
	{
		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidateGet(MethodInfo getter) { return true; }
		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidateSet(MethodInfo setter) { return true; }

		protected virtual void RuntimeInitializeGet(MethodInfo method) { }
		protected virtual void RuntimeInitializeSet(MethodInfo method) { }

		protected virtual void OnGetEntry(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnGetExit(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnGetSuccess(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnGetException(MethodExecutionEventArgs eventArgs) { }


		protected virtual void OnSetEntry(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnSetExit(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnSetSuccess(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnSetException(MethodExecutionEventArgs eventArgs) { }

		protected override void ProvideAspects(PropertyInfo target, LaosReflectionAspectCollection collection)
		{
			var getter = target.GetGetMethod(true);
			var setter = target.GetSetMethod(true);
			if (getter != null && CompileTimeValidateGet(getter))
			{
				collection.AddAspect(getter, new OnGetter(this));
			}
			if (setter != null && CompileTimeValidateSet(setter))
			{
				collection.AddAspect(setter, new OnSetter(this));
			}
		}

		[Serializable]
		sealed internal class OnSetter : OnMethodBoundaryAspect
		{
			public OnSetter(OnPropertyAccessAspect owner)
			{
				owner_ = owner;
			}
			readonly OnPropertyAccessAspect owner_;

			public override void OnEntry(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnSetEntry(eventArgs);
			}

			public override void OnSuccess(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnSetSuccess(eventArgs);
			}

			public override void OnExit(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnSetExit(eventArgs);
			}

			public override void OnException(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnSetException(eventArgs);
			}

			public override void RuntimeInitialize(MethodBase method)
			{
				owner_.RuntimeInitializeSet((MethodInfo)method);
			}
		}

		[Serializable]
		sealed internal class OnGetter : OnMethodBoundaryAspect
		{
			public OnGetter(OnPropertyAccessAspect owner)
			{
				owner_ = owner;
			}
			readonly OnPropertyAccessAspect owner_;
			public override void OnEntry(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnGetEntry(eventArgs);
			}

			public override void OnSuccess(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnGetSuccess(eventArgs);
			}

			public override void OnExit(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnGetExit(eventArgs);
			}

			public override void OnException(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnGetException(eventArgs);
			}

			public override void RuntimeInitialize(MethodBase method)
			{
				owner_.RuntimeInitializeGet((MethodInfo)method);
			}
		}
	}
}
