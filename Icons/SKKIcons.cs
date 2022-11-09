using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKKLib.Icons
{
    public static class SKKIcons
    {
        public static void InitSKKIcons() => ExtendedResourceManager.InitSKKIcons();

        public static Type BuildDynamicIconManager()
        {
            AppDomain myDomain = Thread.GetDomain();
            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "SKKIconManagerAssembly";

            // To generate a persistable assembly, specify AssemblyBuilderAccess.RunAndSave.
            AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
            
            // Generate a persistable single-module assembly.
            ModuleBuilder myModBuilder = myAsmBuilder.DefineDynamicModule(myAsmName.Name, myAsmName.Name + ".dll");

            TypeBuilder myTypeBuilder = myModBuilder.DefineType("SKKIcons", TypeAttributes.Public);

            FieldBuilder iconNameBldr = myTypeBuilder.DefineField("iconName", typeof(string), FieldAttributes.Private);

            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use an array with no elements: new Type[] {})
            PropertyBuilder iconNamePropBldr = myTypeBuilder.DefineProperty("IconName",
                                                             PropertyAttributes.HasDefault,
                                                             typeof(string),
                                                             null);

            // The property set and property get methods require a special
            // set of attributes.
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method for CustomerName.
            MethodBuilder iconNameGetPropMthdBldr = myTypeBuilder.DefineMethod("get_IconName", getSetAttr, typeof(string), Type.EmptyTypes);

            ILGenerator iconNameGetIL = iconNameGetPropMthdBldr.GetILGenerator();

            iconNameGetIL.Emit(OpCodes.Ldarg_0);
            iconNameGetIL.Emit(OpCodes.Ldfld, iconNameBldr);
            iconNameGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for CustomerName.
            MethodBuilder iconNameSetPropMthdBldr = myTypeBuilder.DefineMethod("set_IconName", getSetAttr, null, new Type[] { typeof(string) });

            ILGenerator iconNameSetIL = iconNameSetPropMthdBldr.GetILGenerator();

            iconNameSetIL.Emit(OpCodes.Ldarg_0);
            iconNameSetIL.Emit(OpCodes.Ldarg_1);
            iconNameSetIL.Emit(OpCodes.Stfld, iconNameBldr);
            iconNameSetIL.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to
            // their corresponding behaviors, "get" and "set" respectively.
            iconNamePropBldr.SetGetMethod(iconNameGetPropMthdBldr);
            iconNamePropBldr.SetSetMethod(iconNameSetPropMthdBldr);

            Type retval = myTypeBuilder.CreateType();

            // Save the assembly so it can be examined with Ildasm.exe,
            // or referenced by a test program.
            myAsmBuilder.Save(myAsmName.Name + ".dll");
            return retval;
        }

        public static void IconManagerTest()
        {
            Type iconNameDataType = BuildDynamicIconManager();

            PropertyInfo[] iconNameDataPropInfo = iconNameDataType.GetProperties();
            foreach (PropertyInfo pInfo in iconNameDataPropInfo)
            {
                System.Console.WriteLine("Property '{0}' created!", pInfo.ToString());
            }

            System.Console.WriteLine("---");
            // Note that when invoking a property, you need to use the proper BindingFlags -
            // BindingFlags.SetProperty when you invoke the "set" behavior, and
            // BindingFlags.GetProperty when you invoke the "get" behavior. Also note that
            // we invoke them based on the name we gave the property, as expected, and not
            // the name of the methods we bound to the specific property behaviors.

            object iconNameData = Activator.CreateInstance(iconNameDataType);
            iconNameDataType.InvokeMember("IconName", BindingFlags.SetProperty, null, iconNameData, new object[] { "Arrow Up" });

            System.Console.WriteLine("The iconName field of instance iconNameData has been set to '{0}'.",
                               iconNameDataType.InvokeMember("IconName", BindingFlags.GetProperty,
                                                          null, iconNameData, new object[] { }));
        }
    }

    public class ExtendedResourceManager : ResourceManager
    {
        public ExtendedResourceManager(string _nameSpace, Assembly _assembly) : base(_nameSpace, _assembly)
        {
            NameSpace = _nameSpace;
            Assembly = _assembly;
            resMan = this;
        }

        internal static void InitSKKIcons()
        {
            var innerField = typeof(Properties.Resources).GetField("resourceMan",
                BindingFlags.NonPublic | BindingFlags.Static);

            innerField.SetValue(null,
                new ExtendedResourceManager("SKKLib.Properties.Resources",
                    typeof(Properties.Resources).Assembly));

            innerField = typeof(Properties.Resources).GetField("resourceCulture",
                BindingFlags.NonPublic | BindingFlags.Static);

            innerField.SetValue(null, CultureInfo.CurrentUICulture);
        }

        private static ResourceManager resMan { get; set; }

        private string NameSpace { get; }
        private Assembly Assembly { get; }

        public override object GetObject(string name, CultureInfo culture)
        {
            object o = base.GetObject((name == "skibby") ? "arrow_right" : name, culture);
            return ((System.Drawing.Bitmap)(o));
        }
    }

    public static class Arrows
    {
        private static readonly int _rows = 9;
        private static readonly int _cols = 6;

        public static System.Drawing.Bitmap Skibby
        {
            get
            {
                object obj = Properties.Resources.ResourceManager.GetObject("skibby", Properties.Resources.Culture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }

        public static void Test()
        {
        }

    }
}
