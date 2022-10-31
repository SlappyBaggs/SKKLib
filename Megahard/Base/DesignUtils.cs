using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Megahard.ExtensionMethods;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
using System.CodeDom;
using Megahard.CodeDom;
namespace Megahard.Design
{
	public static class Utils
	{
		public static string GenerateDisplayName(MemberDescriptor md)
		{
			string codeName = md.DisplayName;
			if (codeName != md.Name) // we had a display name attribute, so use it
				return codeName;

			var matches = Regex.Matches(codeName, @".[^A-Z]*");
			StringBuilder sb = new StringBuilder();
			foreach (Match m in matches)
			{
				if (sb.Length > 0)
					sb.Append(' ');
				sb.Append(m.Value);
			}
			return sb.ToString();
		}

		public static void GenerateComponentName(IComponent comp)
		{
			Type t = comp.GetType();
			int pos = t.Name.LastIndexOf('.');
			string defaultName = t.Name.Substring(pos + 1);
			if (defaultName.Length >= 3)
			{
				// find first lower case character
				int lowerPos = defaultName.FindFirst(0, x=>char.IsLower(x));
				if (lowerPos == 1)
				{
					defaultName = char.ToLower(defaultName[0]) + defaultName.Substring(1);
				}
				else if (lowerPos > 0)
				{
					defaultName = defaultName.Substring(0, lowerPos - 1).ToLower() + defaultName.Substring(lowerPos - 1);
				}
				else if (lowerPos == -1)
				{
					defaultName = defaultName.ToLower();
				}
			}

			string name = "_" + defaultName;
			int i = 1;
			while (comp.Site.Container.Components[name] != null)
			{
				name = "_" + defaultName + i;
				i += 1;
			}
			comp.Site.Name = name;
		}

		public static void CreateInitializeComponent2Method(IComponent comp)
		{
			//Hook to the designer
			IDesignerHost host = (IDesignerHost)comp.Site.GetService(typeof(IDesignerHost));
			if (host == null)
				return;
			var typedecl = (CodeTypeDeclaration)comp.Site.GetService(typeof(CodeTypeDeclaration));
			if (typedecl == null)
				return;
			const string initComp2 = "InitializeComponent2";

			if (typedecl.FindMembers(initComp2).ToArray().Length == 0)
				typedecl.AddMember(CodeDom.CodeDom.DeclareMethod(initComp2));

			//Find the constructor and add a call to my "InitializeComponent2()" method
			var ctors = from CodeTypeMember tm in typedecl.Members where tm is CodeConstructor select tm as CodeConstructor;
			foreach (CodeConstructor ctor in ctors)
			{
				var stmts = from CodeStatement cs in ctor.Statements
							where
								(cs is CodeExpressionStatement && ((cs as CodeExpressionStatement).Expression is CodeMethodInvokeExpression) &&
								((cs as CodeExpressionStatement).Expression as CodeMethodInvokeExpression).Method.MethodName == initComp2)
							select cs;
				if (stmts.ToArray().Length == 0)
				{
					ctor.AddStatement(CodeDom.CodeDom.ThisCall(initComp2));
				}
			}

			//var ctmInitComp2 = typedecl.FindMembers(initComp2).ToArray()[0];

			//Add and remove a label because otherwise the code to add the method seems to stay "inactive",
			//while in this way it works
			var lameComponent = host.CreateComponent(typeof(System.ComponentModel.Component));
			host.DestroyComponent(lameComponent);
			
		}
	}
}
