using System;
using System.Collections.Generic;
using System.CodeDom;
using System.Linq;
using System.Text;
using System.Reflection;
using Megahard.ExtensionMethods;
using System.CodeDom.Compiler;

namespace Megahard.CodeDom
{
	public class PropertyGroup
	{
		public void Add<T>(string name, bool canSet)
		{
			Add(typeof(T).Name, name, canSet);
		}
		public void Add(string t, string name, bool canSet)
		{
			props_.Add(new PropData { Name = name, Type = t, CanSet = canSet });
		}

		internal IEnumerable<PropData> Props()
		{
			foreach (var p in props_)
				yield return p;
		}

		private List<PropData> props_ = new List<PropData>();
	}
	struct PropData
	{
		public string Name;
		public string Type;
		public bool CanSet;
	}

	/// <summary>
	/// Helper class for making the usage of CodeDom a bit less tedious
	/// </summary>
	public static class CodeDom
	{
		public static CodeNamespace DeclareNamespace(string s)
		{
			return new CodeNamespace(s);
		}

		public static CodeCastExpression Cast<T>(CodeExpression e)
		{
			return new CodeCastExpression(typeof(T), e);
		}
		public static CodeCastExpression Cast(Type t, CodeExpression e)
		{
			return new CodeCastExpression(CreateTypeRef(t), e);
		}

		public static CodeCastExpression Cast(string t, CodeExpression e)
		{
			return new CodeCastExpression(t, e);
		}

		public static CodeCastExpression Cast(string t, string s)
		{
			return new CodeCastExpression(t, new CodeSnippetExpression(s));
		}

		public static CodeCastExpression Cast(Type t, string s)
		{
			return new CodeCastExpression(t, new CodeSnippetExpression(s));
		}

		public static CodeCastExpression Cast<T>(string s)
		{
			return new CodeCastExpression(typeof(T), new CodeSnippetExpression(s));
		}

		public static CodeObjectCreateExpression Create(string t, params CodeExpression[] p)
		{
			return new CodeObjectCreateExpression(t, p);
		}

		public static CodeObjectCreateExpression Create(CodeTypeReference t, params CodeExpression[] p)
		{
			return new CodeObjectCreateExpression(t, p);
		}

		public static CodeBinaryOperatorExpression Equals(CodeExpression left, CodeExpression right)
		{
			return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.IdentityEquality, right);
		}

		public static CodeBinaryOperatorExpression NotEquals(CodeExpression left, CodeExpression right)
		{
			return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.IdentityInequality, right);
		}

		public static CodeConditionStatement If(CodeExpression e)
		{
			return new CodeConditionStatement() { Condition = e };
		}
		public static CodeConditionStatement IfEqualNull(CodeExpression comparetoNull, params CodeStatement[] statements)
		{
			return new CodeConditionStatement(Equals(comparetoNull, Null()), statements);
		}

		public static CodeArgumentReferenceExpression RefArg(string n)
		{
			return new CodeArgumentReferenceExpression(n);
		}

		public static CodeMemberEvent CreateEvent(string name, Type eventType)
		{
			return CreateEvent(name, eventType.Name);
		}
		public static CodeMemberEvent CreateEvent(string name, string type)
		{
			var cme = new CodeMemberEvent() { Name = name, Type = new CodeTypeReference(type) };
			return cme;
		}

		public static CodePrimitiveExpression Null() { return new CodePrimitiveExpression(null); }
		public static CodeAssignStatement Assign(CodeExpression left, CodeExpression right)
		{
			return new CodeAssignStatement(left, right);
		}

		public static CodeTypeDeclaration Decompose(Type t, string name)
		{
			//if (!t.IsValueType)
			//	throw new ArgumentException("t");

			var mystruct = DeclareStruct(name);
			if (t.IsPublic) mystruct.MakePublic();
			foreach (var p in t.GetProperties())
			{
				var prop = DeclareProperty(p.PropertyType, p.Name);
				prop.MakePublic();
				string propBackingName = "prop" + p.Name + "_";
				var field = DeclareField(p.PropertyType, propBackingName);
				mystruct.AddMember(field);
				var get = p.GetGetMethod();
				if (get != null)
				{
					prop.AddGetStatement(Return(mystruct.RefMember(propBackingName)));
				}

				var set = p.GetSetMethod();
				if (set != null)
				{
					prop.AddSetStatement(Assign(mystruct.RefMember(propBackingName), RefArg("value")));
				}
				mystruct.AddMember(prop);
			}

			foreach (var p in t.GetFields())
			{
				var field = DeclareField(p.FieldType, p.Name);
				field.MakePublic();
				mystruct.AddMember(field);
			}

/*
			foreach (var p in t.GetMethods())
			{
				var meth = DeclareSnippetMethod("public partial " + p.ReturnType.ToString() + " " + p.Name + "();");
				mystruct.AddMember(meth);
			}
  */
			return mystruct;
		}


		public static CodeTypeDeclaration Decompose(Type t)
		{
			return Decompose(t, t.Name);
		}

		public static CodeTypeDeclaration Decompose<T>() where T : struct
		{
			return Decompose(typeof(T));
		}

		public static CodeFieldReferenceExpression RefMemberVar(CodeExpression targetOb, string varName)
		{
			CodeFieldReferenceExpression e = new CodeFieldReferenceExpression(targetOb, varName);
			return e;
		}

		public static CodeFieldReferenceExpression RefMemberVar(string targetOb, string varName)
		{
			CodeFieldReferenceExpression e = new CodeFieldReferenceExpression(RefType(targetOb), varName);
			return e;
		}

		public static CodeTypeReferenceExpression RefType(string n)
		{
			return new CodeTypeReferenceExpression(n);
		}

		public static CodeTypeDeclaration DeclareStruct(string name)
		{
			var c = new CodeTypeDeclaration(name);
			c.IsStruct = true;
			return c;
		}
		public static CodeTypeDeclaration DeclareClass(string name)
		{
			CodeTypeDeclaration mfc = new CodeTypeDeclaration(name);
			mfc.IsClass = true;
			return mfc;
		}

		public static CodeTypeDeclaration DeclareInterface(string name)
		{

			if (!name.StartsWith("I") || name.Length == 1 || Char.IsLower(name[1]))
				name = "I" + name;
			CodeTypeDeclaration inter = new CodeTypeDeclaration(name);
			inter.IsInterface = true;
			
			return inter;
		}

		public static CodeMemberMethod DeclareMethod(string name)
		{
			var meth = new CodeMemberMethod();
			meth.Name = name;
			return meth;
		}

		public static CodeSnippetTypeMember DeclareSnippetMethod(string s)
		{
			return new CodeSnippetTypeMember(s);
		}

		public static CodeMemberProperty DeclareProperty(string t, string name)
		{
			var prop = new CodeMemberProperty();
			prop.Name = name;
			prop.Type = new CodeTypeReference(t);
			return prop;
		}

		public static CodeMemberField DeclareField(string t, string name)
		{
			return new CodeMemberField(t, name);
		}

		public static CodeMemberField DeclareField(Type t, string name)
		{
			return new CodeMemberField(t, name);
		}

		public static CodeMemberField DeclareField<T>(string name)
		{
			return new CodeMemberField(typeof(T), name);
		}

		public static CodeMemberProperty DeclareProperty(Type t, string name)
		{
			var prop = new CodeMemberProperty();
			prop.Name = name;
			prop.Type = CreateTypeRef(t);
			return prop;
		}

		public static CodeMemberProperty DeclareProperty<T>(string name)
		{
			var prop = new CodeMemberProperty();
			prop.Name = name;
			prop.Type = new CodeTypeReference(typeof(T));
			return prop;
		}

		public static CodeMemberMethod DeclareMethod(string name, Type t)
		{
			var meth = new CodeMemberMethod();
			meth.Name = name;
			meth.ReturnType = CreateTypeRef(t);
			return meth;
		}

		public static CodeMemberMethod DeclareMethod(string name, string t)
		{
			var meth = new CodeMemberMethod();
			meth.Name = name;
			meth.ReturnType = new CodeTypeReference(t);
			return meth;
		}

		public static CodeMemberMethod DeclareMethod<T>(string name)
		{
			var meth = new CodeMemberMethod();
			meth.Name = name;
			meth.ReturnType = new CodeTypeReference(typeof(T));
			return meth;
		}

		public static CodeMethodInvokeExpression ThisCall(string name)
		{
			return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), name));
		}

		public static CodeMethodInvokeExpression Call(CodeExpression ob, string methodName)
		{
			return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ob, methodName));
		}

		public static CodeMethodInvokeExpression Call(CodeExpression ob, string methodName, params Type[] genericArgs)
		{
			CodeTypeReference[] r = new CodeTypeReference[genericArgs.Length];
			for(int i = 0; i < r.Length; ++i)
				r[i] = new CodeTypeReference(new CodeTypeParameter(genericArgs[i].Name));
			return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(ob, methodName, r));
		}

		public static CodeMethodInvokeExpression Call(CodeExpression ob, MethodInfo mi)
		{
			return Call(ob, mi.Name, mi.GetGenericArguments());
		}


		public static CodeMethodReturnStatement Return(CodeExpression e)
		{
			return new CodeMethodReturnStatement(e);
		}

		public static CodeMethodReturnStatement Return(object o)
		{
			return new CodeMethodReturnStatement(new CodePrimitiveExpression(o));
		}

		public static CodeMethodReturnStatement Return(string s)
		{
			return new CodeMethodReturnStatement(new CodeSnippetExpression(s));
		}

		public static CodeThisReferenceExpression This()
		{
			return new CodeThisReferenceExpression();
		}

		public static CodeTypeReference CreateTypeRef<T>()
		{
			return CreateTypeRef(typeof(T));
		}
		public static CodeTypeReference CreateTypeRef(Type t)
		{
			if (t == null)
				return new CodeTypeReference();
			if (t.IsGenericParameter)
				return new CodeTypeReference(new CodeTypeParameter(t.Name));
			if(t.IsGenericType)
			{
				var ctr = new CodeTypeReference();
				ctr.BaseType = Reflection.GenericUtils.DecodeGenericName(t.Name);
				foreach(Type gp in t.GetGenericArguments())
					ctr.TypeArguments.Add(CreateTypeRef(gp));
				return ctr;
			}
			return new CodeTypeReference(t);
		}

	}

	public static class CodeTypeMemberExtensions
	{
		public static CodeTypeMember AddAttribute(this CodeTypeMember t, string aname, string initVal)
		{
			t.CustomAttributes.Add(new CodeAttributeDeclaration(aname, new CodeAttributeArgument(new CodeSnippetExpression(initVal))));
			return t;
		}

		public static T MakePublic<T>(this T t) where T : CodeTypeMember
		{
			t.Attributes = (t.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
			return t;
		}

		public static T MakeInternal<T>(this T t) where T : CodeTypeMember
		{
			t.Attributes = (t.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Assembly;
			return t;
		}


		public static T MakeAbstract<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Abstract;
			return meth;
		}

		public static T MakeVirtual<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.ScopeMask) & (~MemberAttributes.Final);
			return meth;
		}

		public static T MakeOverride<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.ScopeMask) | (MemberAttributes.Override);
			return meth;
		}

		public static T MakeStatic<T>(this T m) where T : CodeTypeMember
		{
			m.Attributes = (m.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Static;
			IsStatic(m);
			return m;
		}

		public static bool IsStatic<T>(this T m) where T : CodeTypeMember
		{
			var res = (m.Attributes & MemberAttributes.Static);
			return res == MemberAttributes.Static;
		}

		public static bool IsPublic<T>(this T m) where T : CodeTypeMember
		{
			return (m.Attributes & MemberAttributes.Public) == MemberAttributes.Public;
		}

		public static T MakeProtected<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Family;
			return meth;
		}
		public static T MakeProtectedInternal<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.FamilyOrAssembly;
			return meth;
		}
		public static T MakePrivate<T>(this T meth) where T : CodeTypeMember
		{
			meth.Attributes = (meth.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
			return meth;
		}

		public static T MakeNew<T>(this T m) where T : CodeTypeMember
		{
			m.Attributes = (m.Attributes & ~MemberAttributes.VTableMask) | MemberAttributes.New;
			return m;
		}

	}

	public static class CodeMemberMethodExtensions
	{
		public static CodeMemberMethod AddParam(this CodeMemberMethod m, string varType, string name)
		{
			m.Parameters.Add(new CodeParameterDeclarationExpression(varType, name));
			return m;
		}

		public static CodeMemberMethod AddParam(this CodeMemberMethod m, Type t, string name)
		{
			m.Parameters.Add(new CodeParameterDeclarationExpression(CodeDom.CreateTypeRef(t), name));
			return m;
		}

		public static CodeMemberMethod AddParam(this CodeMemberMethod m, CodeTypeReference t, string name)
		{
			m.Parameters.Add(new CodeParameterDeclarationExpression(t, name));
			return m;
		}

		public static CodeMemberMethod AddParam<T>(this CodeMemberMethod m, string name)
		{
			m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(T), name));
			return m;
		}

		public static CodeMemberMethod AddStatement(this CodeMemberMethod m, CodeStatement s)
		{
			m.Statements.Add(s);
			return m;
		}

		public static CodeMemberMethod AddStatement(this CodeMemberMethod m, CodeExpression e)
		{
			m.Statements.Add(e);
			return m;
		}

		public static CodeMemberMethod AddStatement(this CodeMemberMethod m, string snippet)
		{
			m.AddStatement(new CodeSnippetExpression(snippet));
			return m;
		}

		public static void SetPrivateImplementation(this CodeMemberMethod m, Type t)
		{
			/*
			if (t.IsGenericType)
			{
				// Workaround for codedom bug, it seems it doesnt handle generic private base types very well
				string s = Reflection.GenericUtils.DecodeGenericName(t) + "<";
				int i = 0;
				foreach(Type ga in t.GetGenericArguments())
				{
					s += (i++ == 0 ? "" : ", ") + ga.Name;
				}
				s += ">";

				m.PrivateImplementationType = new CodeTypeReference(s);
			}
			else
			{
			 */
				var ctr = CodeDom.CreateTypeRef(t);
				m.PrivateImplementationType = ctr;
			//}
		}
	}

	public static class CodeMemberPropertyExtensions
	{
		public static CodeMemberProperty AddGetStatement(this CodeMemberProperty p, CodeExpression e)
		{
			p.GetStatements.Add(e);
			return p;
		}
		public static CodeMemberProperty AddGetStatement(this CodeMemberProperty p, CodeStatement e)
		{
			p.GetStatements.Add(e);
			return p;
		}

		public static CodeMemberProperty AddGetStatement(this CodeMemberProperty p, string s)
		{
			p.GetStatements.Add(new CodeSnippetExpression(s));
			return p;
		}
		
		public static CodeMemberProperty AddSetStatement(this CodeMemberProperty p, CodeExpression e)
		{
			p.SetStatements.Add(e);
			return p;
		}
		public static CodeMemberProperty AddSetStatement(this CodeMemberProperty p, CodeStatement e)
		{
			p.SetStatements.Add(e);
			return p;
		}
		public static CodeMemberProperty AddSetStatement(this CodeMemberProperty p, string s)
		{
			p.SetStatements.Add(new CodeSnippetExpression(s));
			return p;
		}

		public static CodeMemberField GetBackingField(this CodeMemberProperty p)
		{
			if (p.UserData.Contains("backing"))
				return p.UserData["backing"] as CodeMemberField;
			else
				return null;
		}

		public static CodeMemberProperty MakeStatic(this CodeMemberProperty p)
		{
			p.Attributes = (p.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Static;
			if (p.UserData.Contains("backing"))
			{
				CodeMemberField f = p.UserData["backing"] as CodeMemberField;
				f.MakeStatic();
			}
			return p;
		}
		public static void SetPrivateImplementation(this CodeMemberProperty m, Type t)
		{
			/*
			if (t.IsGenericType)
			{
				// Workaround for codedom bug, it seems it doesnt handle generic private base types very well
				string s = Reflection.GenericUtils.DecodeGenericName(t) + "<";
				int i = 0;
				foreach (Type ga in t.GetGenericArguments())
				{
					s += (i++ == 0 ? "" : ", ") + ga.Name;
				}
				s += ">";

				m.PrivateImplementationType = new CodeTypeReference(s);
			}
			else
			{
			 */
				var ctr = CodeDom.CreateTypeRef(t);
				m.PrivateImplementationType = ctr;
			//}
		}

	}

	public static class CodeTypeDeclarationExtensions
	{
		public static CodeTypeDeclaration ExtractPublicInterface(this CodeTypeDeclaration t)
		{
			CodeTypeDeclaration ctd = new CodeTypeDeclaration("I" + t.Name);
			ctd.IsInterface = true;

			foreach (CodeMemberProperty prop in (from CodeTypeMember p in t.Members where p.IsPublic() && !p.IsStatic() && p is CodeMemberProperty select p))
			{
				var iprop = new CodeMemberProperty() { Type = prop.Type, Name = prop.Name };
				iprop.HasGet = prop.HasGet;
				iprop.HasSet = prop.HasSet;
				ctd.Members.Add(iprop);
			}

			foreach (CodeMemberMethod mm in (from CodeTypeMember p in t.Members where p.IsPublic() && !p.IsStatic() && p is CodeMemberMethod select p))
			{
				var imm = new CodeMemberMethod() { Name = mm.Name };
				imm.ReturnType = mm.ReturnType;
				imm.Parameters.AddRange(mm.Parameters);
				ctd.Members.Add(imm);
			}

			
			return ctd;
		}

		public static IEnumerable<CodeTypeMember> FindMembers(this CodeTypeDeclaration t, string memberName)
		{
			return from CodeTypeMember ctm in t.Members where ctm.Name == memberName select ctm;
		}

		public static IEnumerable<CodeTypeMember> FindMembers(this CodeTypeDeclaration t, Func<CodeTypeMember, bool> compare)
		{
			return from CodeTypeMember ctm in t.Members where compare(ctm) select ctm;
		}

		public static string GenerateCSharpCode(this CodeTypeDeclaration t)
		{
			System.CodeDom.Compiler.CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("c#");
			System.CodeDom.Compiler.CodeGeneratorOptions options = new System.CodeDom.Compiler.CodeGeneratorOptions();
			options.BlankLinesBetweenMembers = true;
			options.BracingStyle = "C";

			var sw = new System.IO.StringWriter();
			provider.GenerateCodeFromType(t, sw, options);
			return sw.ToString();
		}

		public static string GenerateCode(this CodeTypeDeclaration t, ICodeGenerator gen)
		{
			System.CodeDom.Compiler.CodeGeneratorOptions options = new System.CodeDom.Compiler.CodeGeneratorOptions();
			options.BlankLinesBetweenMembers = true;
			options.BracingStyle = "C";

			var sw = new System.IO.StringWriter();
			gen.GenerateCodeFromType(t, sw, options);
			return sw.ToString();
		}

		public static CodeTypeDeclaration MakePublic(this CodeTypeDeclaration t)
		{

			t.TypeAttributes = (t.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
			return t;
		}

		public static CodeTypeDeclaration MakePrivate(this CodeTypeDeclaration t)
		{
			t.TypeAttributes = (t.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.NotPublic;
			return t;
		}

		public static CodeTypeDeclaration MakeAbstract(this CodeTypeDeclaration t)
		{
			t.TypeAttributes |= TypeAttributes.Abstract;
			return t;
		}

		public static CodeTypeDeclaration MakePartial(this CodeTypeDeclaration t)
		{
			t.IsPartial = true;
			return t;
		}

		public static CodeTypeDeclaration AddMember(this CodeTypeDeclaration t, CodeTypeMember m)
		{
			t.Members.Add(m);
			return t;
		}

		public static CodeFieldReferenceExpression RefMember(this CodeTypeDeclaration t, string name)
		{
			foreach (CodeTypeMember mem in t.Members)
			{
				if (mem.Name == name && mem.IsStatic())
					return new CodeFieldReferenceExpression(null, name);
			}
			return CodeDom.RefMemberVar(new CodeThisReferenceExpression(), name);
		}

		public static CodeFieldReferenceExpression RefPropBacking(this CodeTypeDeclaration t, string name)
		{
			return RefMember(t, "prop" + name + "_");
		}

		public static CodeFieldReferenceExpression RefPropBacking(this CodeTypeDeclaration t, CodeMemberProperty prop)
		{
			return RefMember(t, prop.GetBackingField().Name);
		}

		public static CodeMemberField AddPrivateField(this CodeTypeDeclaration c, string t, string name)
		{
			var field = CodeDom.DeclareField(t, name);
			c.AddMember(field);
			return field;
		}

		public static CodeMemberField AddPrivateField(this CodeTypeDeclaration c, Type t, string name)
		{
			var cmf = AddPrivateField(c, t.Name, name);
			if(t.IsGenericType)
			{
				foreach(Type gt in t.GetGenericTypeDefinition().GetGenericArguments())
				{
					cmf.Type.TypeArguments.Add(new CodeTypeReference(new CodeTypeParameter(gt.Name)));
				}
			}
			return cmf;
		}

		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration c, string t, string name, bool canSet)
		{
			return AddProperty(c, t, name, canSet, t);
		}
		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration c, Type t, string name, bool canSet, string backingType)
		{
			return AddProperty(c, t.Name, name, canSet, backingType);
		}

		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration c, string t, string name, bool canSet, string backingType)
		{
			var prop = CodeDom.DeclareProperty(t, name);
			prop.MakePublic();
			string propBackingName = "prop" + name + "_";
			var pf = c.AddPrivateField(backingType, propBackingName);
			prop.UserData.Add("backing", pf);

			prop.AddGetStatement(CodeDom.Return(c.RefMember(propBackingName)));
			if (canSet)
			{
				prop.AddSetStatement(CodeDom.Assign(c.RefMember(propBackingName), CodeDom.RefArg("value")));
			}
			c.AddMember(prop);
			return prop;
		}

		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration t, string ptype, string pname, string getSnippet, string setSnippet)
		{
			var prop = CodeDom.DeclareProperty(ptype, pname);
			prop.MakePublic();
			if(getSnippet != null)
				prop.AddGetStatement(getSnippet);
			if(setSnippet != null)
				prop.AddSetStatement(setSnippet);

			t.Members.Add(prop);
			return prop;
		}

		public static CodeSnippetTypeMember AddProperty(this CodeTypeDeclaration t, string ptype, string pname, string snippet)
		{
			var sn = new CodeSnippetTypeMember(snippet) { Name = pname };
			t.Members.Add(sn);
			return sn;
		}
		public static CodeSnippetTypeMember AddProperty(this CodeTypeDeclaration t, Type ptype, string pname, string snippet)
		{
			return AddProperty(t, ptype.ToString(), pname, snippet);
		}

		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration t, Type ptype, string pname, string getSnippet, string setSnippet)
		{
			return AddProperty(t, ptype.ToString(), pname, getSnippet, setSnippet);
		}

		public static CodeMemberProperty AddProperty(this CodeTypeDeclaration c, Type t, string name, bool canSet)
		{
			return AddProperty(c, t.Name, name, canSet);
		}

		public static CodeMemberProperty AddProperty<T>(this CodeTypeDeclaration c, string name, bool canSet)
		{
			return c.AddProperty(typeof(T), name, canSet);
		}

		// IDictionary is name, type
		public static CodeConstructor Add(this CodeTypeDeclaration c, PropertyGroup props)
		{
			var ctor = new CodeConstructor();
			foreach(PropData prop in props.Props())
			{
				c.AddProperty(prop.Type, prop.Name, prop.CanSet);
				ctor.AddParam(prop.Type, prop.Name);
				ctor.AddStatement(CodeDom.Assign(c.RefPropBacking(prop.Name), CodeDom.RefArg(prop.Name)));
			}
			ctor.MakePublic();
			c.AddMember(ctor);
			return ctor;
		}

		/// <summary>
		/// Creates a constructor that takes args which initialize existing member fields
		/// </summary>
		/// <param name="fields">The fields to initialize</param>
		public static CodeConstructor MakeConstructor(this CodeTypeDeclaration c, params string[] fields)
		{
			
			var ctor = new CodeConstructor();
			var arr = (from CodeTypeMember m in c.Members where m is CodeMemberField && fields.Contains(m.Name) select (CodeMemberField)m).ToArray();
			foreach(string f in fields)
			{
				CodeMemberField field = arr.First(x => x.Name == f);
				string argName = (field.Name[field.Name.Length - 1] == '_' ? field.Name.Substring(0, field.Name.Length - 1) : "arg" + field.Name);
				ctor.AddParam(field.Type, argName);
				ctor.AddStatement(CodeDom.Assign(c.RefMember(field.Name), CodeDom.RefArg(argName)));
			}
			c.AddMember(ctor);
			return ctor;
		}

		public static CodeConstructor MakeConstructor(this CodeTypeDeclaration c)
		{
			var ctor = new CodeConstructor();
			foreach (object o in c.Members)
			{
				CodeMemberField mem = o as CodeMemberField;
				if (mem == null) continue;
				if (mem.Name.StartsWith("prop"))
				{
					string argName = mem.Name.Substring(4, mem.Name.Length - 5);
					ctor.AddParam(mem.Type.BaseType, argName);
					ctor.AddStatement(CodeDom.Assign(c.RefMember(mem.Name), CodeDom.RefArg(argName)));
				}
			}
			c.AddMember(ctor);
			return ctor;
		}
		public static CodeMemberEvent AddEvent(this CodeTypeDeclaration t, EventInfo ei)
		{
			var cme = AddEvent(t, ei.Name, ei.EventHandlerType);
			// explicitly implemented events are so not working right in the Csharp code generator, not alot i can do about that short of writing one
			//cme.SetPrivateImplementation(ei.GetExplicitlyImplementedInterface());
			return cme;
		}
		public static CodeMemberEvent AddEvent(this CodeTypeDeclaration t, string name, Type eventType)
		{
			int dotpos = name.LastIndexOf('.');
			if (dotpos != -1)
				name = name.Substring(dotpos + 1);
			var cme = CodeDom.CreateEvent(name, eventType);
			cme.MakePublic();
			t.AddMember(cme);
			var fireEvent = CodeDom.DeclareMethod("On" + name, typeof(void));

			cme.UserData["megahard.FireEvent"] = fireEvent;

			MethodInfo invoke = eventType.GetMethod("Invoke");
			foreach(ParameterInfo pi in invoke.GetParameters())
			{
				fireEvent.AddParam(pi.ParameterType, pi.Name);
			}

			var ifStatement =  CodeDom.If(CodeDom.NotEquals(t.RefMember(name), new CodePrimitiveExpression(null)));
			CodeExpression[] delegateParams = new CodeExpression[fireEvent.Parameters.Count];
			for (int i = 0; i < delegateParams.Length; ++i)
				delegateParams[i] = CodeDom.RefArg(fireEvent.Parameters[i].Name);

			ifStatement.TrueStatements.Add(
					new CodeDelegateInvokeExpression(new CodeEventReferenceExpression(CodeDom.This(), name), delegateParams));
			fireEvent.AddStatement(ifStatement);
			fireEvent.MakeProtected();
			t.AddMember(fireEvent);
			return cme;
		}

	}


	public static class CodeMethodInvokeExpressionExtensions
	{
	
		public static CodeMethodInvokeExpression AddParam(this CodeMethodInvokeExpression m, CodeExpression e)
		{
			m.Parameters.Add(e);
			return m;
		}

		public static CodeMethodInvokeExpression AddParam(this CodeMethodInvokeExpression m, string snippet)
		{
			m.Parameters.Add(new CodeSnippetExpression(snippet));
			return m;
		}

		public static CodeMethodInvokeExpression AddParam(this CodeMethodInvokeExpression m, object val)
		{
			m.Parameters.Add(new CodePrimitiveExpression(val));
			return m;
		}
	}

	public static class CodeNamespaceExtensions
	{
		public static CodeNamespace AddType(this CodeNamespace ns, CodeTypeDeclaration t)
		{
			ns.Types.Add(t);
			return ns;
		}

		public static CodeNamespace AddImport(this CodeNamespace ns, string s)
		{
			ns.Imports.Add(new CodeNamespaceImport(s));
			return ns;
		}

		public static CodeNamespace AddImports(this CodeNamespace ns, params string[] names)
		{
			foreach (string s in names)
			{
				ns.Imports.Add(new CodeNamespaceImport(s));
			}
			return ns;
		}
	}

	public static class CodeMemberFieldExtensions
	{

		public static T InitAsArray<T>(this T t, CodeExpression[] init) where T : CodeMemberField
		{
			t.InitExpression = new CodeArrayCreateExpression(t.Type, init);
			return t;
		}
	}



	public static class CodeCompileUnitExtensions
	{
		public static CodeCompileUnit AddNamespace(this CodeCompileUnit cu, CodeNamespace ns)
		{
			cu.Namespaces.Add(ns);
			return cu;
		}
	}

	public static class CodeConstructorExtensions
	{
		public static CodeConstructor SupportInitProperty<T>(this CodeConstructor ctor, string name)
		{
			ctor.AddParam<T>(name);
			ctor.AddStatement(CodeDom.Assign(CodeDom.RefMemberVar(CodeDom.This(), "prop" + name + "_"), CodeDom.RefArg(name)));
			return ctor;
		}
	}

	public static class CodeMemberEventExtensions
	{
		public static CodeMemberMethod GetFireEvent(this CodeMemberEvent e)
		{
			return (CodeMemberMethod)e.UserData["megahard.FireEvent"];
		}

		public static void SetPrivateImplementation(this CodeMemberEvent e, Type t)
		{
			if (t.IsGenericType)
			{
				// Workaround for codedom bug, it seems it doesnt handle generic private base types very well
				string s = t.DecodeGenericName() + "<";
				int i = 0;
				foreach (Type ga in t.GetGenericArguments())
				{
					s += (i++ == 0 ? "" : ", ") + ga.Name;
				}
				s += ">";

				e.PrivateImplementationType = new CodeTypeReference(s);
			}
			else
			{
				var ctr = CodeDom.CreateTypeRef(t);
				e.PrivateImplementationType = ctr;
			}
		}
	}

	
}
