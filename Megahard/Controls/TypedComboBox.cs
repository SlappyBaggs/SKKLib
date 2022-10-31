using System;
using System.Windows.Forms;
using System.ComponentModel;

using System.Linq;
namespace Megahard.Controls
{
	/*
	 * I dont understand why, but when a class derives from this, it cannot work in the designer... or at least with udcom it didnt
	 * perhaps investigate more when time permits
	 */

	public class TypedComboBox<ItemType, ValueType> : ComboBox
	{
		public TypedComboBox()
		{
		}
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ItemType SelectedItem
		{
			get { return (ItemType)base.SelectedItem; }
			set { base.SelectedItem = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ValueType SelectedValue
		{
			get { return (ValueType)base.SelectedValue; }
			set { base.SelectedValue = value; }
		}
	}
}