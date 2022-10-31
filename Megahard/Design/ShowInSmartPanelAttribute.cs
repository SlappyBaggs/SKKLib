using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;
using Megahard.Reflection;
using ComponentDesignerAlias = System.ComponentModel.Design.ComponentDesigner;
using SelectionRulesAlias = System.Windows.Forms.Design.SelectionRules;
using INameCreationService = System.ComponentModel.Design.Serialization.INameCreationService;

namespace Megahard.Design
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true)]
	public class ShowInSmartPanelAttribute : Attribute
	{
		public ShowInSmartPanelAttribute()
		{
			AutoFireComponentChange = true;
			show_ = true;
		}
		public ShowInSmartPanelAttribute(bool show)
		{
			show_ = show;
			AutoFireComponentChange = true;
		}

		readonly bool show_;
		public bool Show
		{
			get { return show_; }
		}
		public bool IncludeAsVerb { get; set; }

		[DefaultValue(true)]
		public bool AutoFireComponentChange { get; set; } // default true

		static readonly ShowInSmartPanelAttribute s_True = new ShowInSmartPanelAttribute(true);
		public static ShowInSmartPanelAttribute True
		{
			get { return s_True; }
		}

		static readonly ShowInSmartPanelAttribute s_False = new ShowInSmartPanelAttribute(false);
		public static ShowInSmartPanelAttribute False
		{
			get { return s_False; }
		}

	}
}
