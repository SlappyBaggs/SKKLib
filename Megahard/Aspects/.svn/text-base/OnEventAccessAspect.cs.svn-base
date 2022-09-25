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
	public abstract class OnEventAccessAspect : EventAspect
	{
		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidateAdd(MethodInfo getter) { return true; }
		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidateRemove(MethodInfo setter) { return true; }
		[CompileTimeSemantic]
		protected virtual bool CompileTimeValidateRaise(MethodInfo raise) { return true; }

		protected virtual void RuntimeInitializeRemove(MethodInfo method) { }
		protected virtual void RuntimeInitializeAdd(MethodInfo method) { }
		protected virtual void RuntimeInitializeRaise(MethodInfo method) { }

		protected virtual void OnAddEntry(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnAddExit(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnAddSuccess(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnAddException(MethodExecutionEventArgs eventArgs) { }

		protected virtual void OnRemoveEntry(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRemoveExit(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRemoveSuccess(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRemoveException(MethodExecutionEventArgs eventArgs) { }

		protected virtual void OnRaiseEntry(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRaiseExit(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRaiseSuccess(MethodExecutionEventArgs eventArgs) { }
		protected virtual void OnRaiseException(MethodExecutionEventArgs eventArgs) { }

		protected override void ProvideAspects(EventInfo target, LaosReflectionAspectCollection collection)
		{
			var adder = target.GetAddMethod(true);
			if (adder != null && CompileTimeValidateAdd(adder))
			{
				collection.AddAspect(adder, new OnAdd(this));
			}
			var remover = target.GetRemoveMethod(true);
			if (remover != null && CompileTimeValidateRemove(remover))
			{
				collection.AddAspect(remover, new OnRemove(this));
			}

			var raiser = target.GetRaiseMethod(true);
			if (raiser != null && CompileTimeValidateRaise(raiser))
			{
				collection.AddAspect(raiser, new OnRaise(this));
			}
		}

		[Serializable]
		internal class OnRemove : OnMethodBoundaryAspect
		{
			public OnRemove(OnEventAccessAspect owner)
			{
				owner_ = owner;
			}
			OnEventAccessAspect owner_;

			public override void OnEntry(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRemoveEntry(eventArgs);
			}

			public override void OnSuccess(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRemoveSuccess(eventArgs);
			}

			public override void OnExit(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRemoveExit(eventArgs);
			}

			public override void OnException(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRemoveException(eventArgs);
			}

			public override void RuntimeInitialize(MethodBase method)
			{
				owner_.RuntimeInitializeRemove((MethodInfo)method);
			}
		}

		[Serializable]
		internal class OnAdd : OnMethodBoundaryAspect
		{
			public OnAdd(OnEventAccessAspect owner)
			{
				owner_ = owner;
			}
			OnEventAccessAspect owner_;
			public override void OnEntry(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnAddEntry(eventArgs);
			}

			public override void OnSuccess(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnAddSuccess(eventArgs);
			}

			public override void OnExit(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnAddExit(eventArgs);
			}

			public override void OnException(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnAddException(eventArgs);
			}

			public override void RuntimeInitialize(MethodBase method)
			{
				owner_.RuntimeInitializeAdd((MethodInfo)method);
			}
		}

		[Serializable]
		internal class OnRaise : OnMethodBoundaryAspect
		{
			public OnRaise(OnEventAccessAspect owner)
			{
				owner_ = owner;
			}
			OnEventAccessAspect owner_;
			public override void OnEntry(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRaiseEntry(eventArgs);
			}

			public override void OnSuccess(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRaiseSuccess(eventArgs);
			}

			public override void OnExit(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRaiseExit(eventArgs);
			}

			public override void OnException(MethodExecutionEventArgs eventArgs)
			{
				owner_.OnRaiseException(eventArgs);
			}

			public override void RuntimeInitialize(MethodBase method)
			{
				owner_.RuntimeInitializeRaise((MethodInfo)method);
			}
		}

	}
}
