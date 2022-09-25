using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections;

namespace Megahard.Design
{
	class StandardValuesUIEditor : SelectFromListTypeEditor
	{
		/// <summary>
		/// <strong>ListBox</strong> which is dropped when the type contains standard values.
		/// </summary>
		/// 
		class PaintValuesListBox : ListBox
		{
			StandardValuesUIEditor editor_;

			/// <summary>
			/// Creates a <strong>DropListBox</strong>.
			/// </summary>
			public PaintValuesListBox(StandardValuesUIEditor editor)
			{
				System.Diagnostics.Debug.Assert(editor != null && editor.GetPaintValueSupported());
				editor_ = editor;
				BorderStyle = BorderStyle.None;
				IntegralHeight = false;
				DrawMode = DrawMode.OwnerDrawVariable;
			}

			/// <summary>
			/// This member overrides <see cref="ListBox.OnDrawItem">ListBox.OnDrawItem</see>.
			/// </summary>
			protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
			{
				e.DrawBackground();

				if (e.Index < 0 || e.Index >= Items.Count)
					return;

				object value = Items[e.Index];
				Rectangle bounds = e.Bounds;

				Pen pen = new Pen(ForeColor);
				try
				{
					Rectangle r = e.Bounds;
					r.Height -= 1;
					r.Width = PAINT_VALUE_WIDTH;
					r.X += 2;
					bounds.X += PAINT_VALUE_WIDTH + 2;
					bounds.Width -= PAINT_VALUE_WIDTH + 2;
					editor_.PaintValue(value, e.Graphics, r);
					e.Graphics.DrawRectangle(pen, r);
				}
				finally
				{
					pen.Dispose();

				}
			}
			protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
			{
				e.ItemHeight += 1;
			}
			const int PAINT_VALUE_WIDTH = 20;
		}

		// A StandardValues editor can wrap an editor, this is done in the case in which a UITypeEditor has a style of none
		// but does support standard values, this value is also permitted to be null..
		// Not sure that meant and i wrote it, but one thing for sure, the wrapped editor is used for when you have a UITypeEditor
		// with style=none but the editor supports painting values
		readonly System.Drawing.Design.UITypeEditor wrappedEditor_;
		public StandardValuesUIEditor(System.Drawing.Design.UITypeEditor wrappedEditor)
		{
			wrappedEditor_ = wrappedEditor;
			base.EditorStyle = System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}

		protected override ListBox CreateListBox(ITypeDescriptorContext context)
		{
			if(GetPaintValueSupported(context))
				return new PaintValuesListBox(this);
			return new ListBox();
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return wrappedEditor_ != null && wrappedEditor_.GetPaintValueSupported();
		}

		protected override void SetupList(ListBox listbox, ITypeDescriptorContext context, object value)
		{
			IEnumerable values = null;
			if (context != null && context.PropertyDescriptor != null && context.PropertyDescriptor.Converter != null)
			{
				values = context.PropertyDescriptor.Converter.GetStandardValues(context);
			}
			else
			{
				values = TypeDescriptor.GetConverter(value).GetStandardValues();
			}
			if (values != null)
			{
				listbox.Items.Clear();
				foreach (object val in values)
					listbox.Items.Add(val ?? "");
				listbox.SelectedItem = value;
			}
		}

	}
}
