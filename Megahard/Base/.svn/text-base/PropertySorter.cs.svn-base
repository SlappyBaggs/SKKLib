using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Megahard.ComponentModel
{
	public class PropertySorter : ExpandableObjectConverter
	{
		public PropertySorter()
		{
		}

		#region Methods
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			//
			// This override returns a list of properties in order
			//
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
			List<PropertyOrderPair> orderedProperties = new List<PropertyOrderPair>();
			foreach (PropertyDescriptor pd in pdc)
			{
				Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];
				if (attribute != null)
				{
					//
					// If the attribute is found, then create an pair object to hold it
					//
					PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
					orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
				}
				else
				{
					//
					// If no order attribute is specifed then given it an order of 0
					//
					orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
				}
			}
			//
			// Perform the actual order using the value PropertyOrderPair classes
			// implementation of IComparable to sort
			//
			orderedProperties.Sort();
			//
			// Build a string list of the ordered names
			//
			string[] propertyNames = new string[orderedProperties.Count];
			
			for(int i = 0; i < orderedProperties.Count; ++i)
			{
				propertyNames[i] = orderedProperties[i].Name;
			}
			//
			// Pass in the ordered list for the PropertyDescriptorCollection to sort by
			//
			//return new PropertyDescriptorCollection(null);
			//return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
			return pdc.Sort(propertyNames);
		}
		#endregion
	}

	#region Helper Class - PropertyOrderAttribute
	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyOrderAttribute : Attribute
	{
		//
		// Simple attribute to allow the order of a property to be specified
		//
		private int _order;
		private static int _autoOrder;
		public PropertyOrderAttribute(int order)
		{
			_order = order;
		}
		public PropertyOrderAttribute()
		{
			_order = _autoOrder++;
		}

		public int Order
		{
			get
			{
				return _order;
			}
		}
	}
	#endregion

	#region Helper Class - PropertyOrderPair
	public class PropertyOrderPair : IComparable<PropertyOrderPair>
	{
		private int _order;
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
		}

		public PropertyOrderPair(string name, int order)
		{
			_order = order;
			_name = name;
		}

		public int CompareTo(PropertyOrderPair pop)
		{
			//
			// Sort the pair objects by ordering by order value
			// Equal values get the same rank
			//
			int otherOrder = pop._order;
			if (otherOrder == _order)
			{
				//
				// If order not specified, sort by name
				//
				string otherName = pop._name;
				return string.Compare(_name, otherName);
			}
			else if (otherOrder > _order)
			{
				return -1;
			}
			return 1;
		}

	}
	#endregion
}
