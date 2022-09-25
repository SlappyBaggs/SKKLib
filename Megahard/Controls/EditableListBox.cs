using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Megahard.Controls
{
	public class EditableListBox : ListBox
	{
		private TextBox editBox;
		private ErrorProvider errorProvider;
		public EditableListBox()
		{
			MakeEditBox();
			editBox.Visible = false;

			DrawMode = DrawMode.OwnerDrawVariable;
			MeasureItem += new MeasureItemEventHandler(listBox_MeasureItem);
			DrawItem += new DrawItemEventHandler(listBox_DrawItem);
			SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);

			errorProvider = new ErrorProvider();
		}

		private void MakeEditBox()
		{
			Controls.Remove(editBox);
			editBox = new TextBox();
			editBox.Validating += new CancelEventHandler(editBox_Validating);
			editBox.KeyPress += new KeyPressEventHandler(editBox_KeyPress);
			editBox.KeyDown += new KeyEventHandler(editBox_KeyDown);
			Controls.Add(editBox);
		}

		private bool ValidatingEditBox()
		{
			if (editBox.Text == string.Empty)
			{
				errorProvider.SetError(editBox, "");
				return true;
			}

			double d;
			try
			{
				d = Convert.ToDouble(editBox.Text);
				errorProvider.SetError(editBox, "");
				return true;
			}
			catch (System.Exception ex)
			{
				errorProvider.SetError(editBox, ex.Message);
				editBox.SelectionStart = 0;
				editBox.SelectionLength = editBox.Text.Length;
				return false;
			}
		}

		private void editBox_KeyDown(object o, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
					if (SelectedIndex > 0)
					{
						if (ValidatingEditBox())
						{
							Items[SelectedIndex] = editBox.Text;
							SelectedIndex = SelectedIndex - 1;
						}
						e.Handled = true;
					}
					break;
				case Keys.Down:
					if (SelectedIndex < (Items.Count - 1))
					{
						if (ValidatingEditBox())
						{
							Items[SelectedIndex] = editBox.Text; 
							SelectedIndex = SelectedIndex + 1;
						}
						e.Handled = true;
					}
					break;
			}
		}

		private void editBox_KeyPress(object o, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				if (ValidatingEditBox())
				{
					if (Items.Count == 0)
						return;
					Items[SelectedIndex] = editBox.Text;
					if (SelectedIndex == Items.Count - 1)
						Items.Add("");
					SelectedIndex = SelectedIndex + 1;
				}
			}
		}

		private void UpdateEntry()
		{
			Items[SelectedIndex] = editBox.Text;
			if (SelectedIndex == Items.Count - 1)
				Items.Add("");
			SelectedIndex = SelectedIndex + 1;
		}

		private void editBox_Validating(object o, CancelEventArgs e)
		{
			e.Cancel = !ValidatingEditBox();
		}

		private void PlaceEditBox(int index)
		{
			editBox.Text = Items[index].ToString();
			editBox.Font = Font;
			editBox.Location = new Point(0, ItemHeight * index + 2);
			editBox.SelectionStart = 0;
			editBox.SelectionLength = editBox.Text.Length;
			editBox.Visible = true;
			editBox.Focus();
		}








		private object FakeListItem = new object();
		private bool ReMeasure = false;
		private int selIndex = -1;
		private void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ReMeasure == true) 
				return;
			
			if (SelectedIndex >= 0)
				PlaceEditBox(SelectedIndex);
			else
				editBox.Visible = false;
			

			ReMeasure = true;
			selIndex = SelectedIndex;
			List<object> ol = new List<object>();
			foreach (object o in Items) ol.Add(o);
			Items.Clear();
			int c = 0;
			foreach (object o in ol)
			{
				if((o.ToString() == string.Empty) && (c != selIndex))
				{
					if(c < selIndex) selIndex--;
				}
				else
				{
					Items.Add(o);
				}
				c++;
			}
			SelectedIndex = selIndex;
			ReMeasure = false;
		}

		private Brush blackBrush = new SolidBrush(Color.Black);
		private void listBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index > ((ListBox)sender).Items.Count - 1) 
				return;
			if (e.Index != SelectedIndex)
				e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, blackBrush, e.Bounds);
		}

		private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			if (e.Index == selIndex)
				e.ItemHeight = editBox.Height + 2;
			else
				e.ItemHeight = ItemHeight;
		}
	}
}
