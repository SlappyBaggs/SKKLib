using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INameCreationService = System.ComponentModel.Design.Serialization.INameCreationService;
using Megahard.ExtensionMethods;
namespace Megahard.Design
{
	class SmartNameCreationService : INameCreationService
	{
		public SmartNameCreationService(INameCreationService existing)
		{
			baseService_ = existing;
		}

		readonly INameCreationService baseService_;
		public string CreateName(System.ComponentModel.IContainer container, Type dataType)
		{
			if (dataType == null)
				throw new ArgumentNullException("dataType");

			int pos = dataType.Name.LastIndexOf('.');
			string defaultName = dataType.Name.Substring(pos + 1);
			if (defaultName.Length >= 3)
			{
				// find first lower case character
				int lowerPos = defaultName.FindFirst(0, x => char.IsLower(x));
				if (lowerPos == 1)
				{
					defaultName = char.ToLower(defaultName[0]) + defaultName.Substring(1);
				}
				else if (lowerPos != 0)
				{
					defaultName = defaultName.Substring(0, lowerPos - 1).ToLower() + defaultName.Substring(lowerPos - 1);
				}
			}

			string name = defaultName + "_";

			if (container != null && container.Components != null)
			{
				int i = 1;
				while (container.Components[name] != null)
				{
					name = defaultName + i + "_";
					i += 1;
				}
			}
			return name;
		}

		public bool IsValidName(string name)
		{
			if(baseService_ != null)
				return baseService_.IsValidName(name);
			return System.Text.RegularExpressions.Regex.IsMatch(name, "^[a-zA-Z_][a-zA-Z0-9_]*$");
		}

		public void ValidateName(string name)
		{
			if(baseService_ != null)
				baseService_.ValidateName(name);
			if (!System.Text.RegularExpressions.Regex.IsMatch(name, "^[a-zA-Z_][a-zA-Z0-9_]*$"))
				throw new Exception(name + " is not a valid name according to " + this.GetType().Name);
		}
	}
}
