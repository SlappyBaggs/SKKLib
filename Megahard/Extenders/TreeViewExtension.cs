using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace System.Windows.Forms
{
	public static class mhTreeViewExtender
	{
		public static void AddXML(this TreeNodeCollection nodes, XElement xml)
		{
			var node = CreateNode(xml);
			nodes.Add(node);
		}

		static TreeNode CreateNode(XElement xml)
		{
			var tn = new TreeNode(string.Format("<{0}>", xml.Name.ToString()));

			if (xml.HasAttributes)
			{
				var attrNodes = (from attr in xml.Attributes() select new TreeNode(attr.ToString())).ToArray();
				tn.Nodes.AddRange(attrNodes);
			}
			if (xml.HasElements)
			{
				var children = (from el in xml.Elements() select CreateNode(el)).ToArray();
				tn.Nodes.AddRange(children);
			}

			if (xml.Nodes().Count(n => n is XText) > 0)
			{
				var sb = new StringBuilder();
				foreach (var node in xml.Nodes().Where(n => n is XText))
				{
					sb.Append(node.ToString());
				}
				tn.Text += sb.ToString();
			}
			
			return tn;
		}
	}

}
