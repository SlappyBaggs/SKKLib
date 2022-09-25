using System;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ComponentFactory.Krypton.Toolkit;


namespace Megahard.Extenders
{
	[ProvideProperty("ScrollToBottom", typeof(Control))]
	public class TextBoxScrollExtender : ExtenderBase<Control, bool>
	{
		protected override bool CanExtend(Control extendee)
		{
			if (extendee is TextBox && (extendee as TextBox).Multiline)
				return true;
			if (extendee is KryptonTextBox && (extendee as KryptonTextBox).Multiline)
				return true;
			return false;
		}

		protected override void OnSet(Control extendee, bool val)
		{
			extendee.TextChanged -= ScrollTextChanged;
			if (val)
				extendee.TextChanged += ScrollTextChanged;
		}

		[Category("MegaHard Extenders - Scroll to Bottom")]
		[Description("Automatically scroll to bottom on input")]
		[DefaultValue(false)]
		public bool GetScrollToBottom(Control tb)
		{
			return base.GetValue(tb);
		}
		public void SetScrollToBottom(Control tb, bool b)
		{
			base.SetValue(tb, b);
		}

		void ScrollTextChanged(object sender, EventArgs args)
		{
			if (sender is TextBox)
			{
				TextBox tb = sender as TextBox;
				tb.SelectionStart = tb.Text.Length;
				tb.ScrollToCaret();
			}
			if (sender is KryptonTextBox)
			{
				KryptonTextBox tb = sender as KryptonTextBox;
				tb.SelectionStart = tb.Text.Length;
				tb.ScrollToCaret();
			}
		}
	}

	[ProvideProperty("NumOnly", typeof(TextBox))]
	public class TextBoxNumOnlyExtender : ExtenderBase<TextBox, bool>
	{
		protected override void OnSet(TextBox extendee, bool val)
		{
			extendee.KeyPress -= NumOnlyKeyPress;
			if (val)
				extendee.KeyPress += NumOnlyKeyPress;
		}

		[Category("MegaHard Extenders - Numbers Only")]
		[Description("Allows only numbers to be entered")]
		[DefaultValue(false)]
		public bool GetNumOnly(TextBox tb)
		{
			return base.GetValue(tb);
		}
		public void SetNumOnly(TextBox tb, bool b)
		{
			base.SetValue(tb, b);
		}

		void NumOnlyKeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = sender as TextBox;
			if(!(e.KeyChar <= 0x1F || (e.KeyChar >= 0x30 && e.KeyChar <= 0x39) || e.KeyChar == 0x2D || (e.KeyChar == 0x2E && tb.Text.IndexOf('.') == -1)))
			{
				e.Handled = true;
			}
		}
	}

	[ProvideProperty("RegExp", typeof(TextBox))]
	public class TextBoxRegExpExtender : ExtenderBase<TextBox, string>
	{
		protected override void OnSet(TextBox extendee, string val)
		{
			extendee.Validating -= RegExpValidate;
			if (!string.IsNullOrEmpty(val))
				extendee.Validating += RegExpValidate;
		}

		[Category("MegaHard Extenders - Regular Expressions")]
		[Description("Makes the TextBox validate to a regular expression")]
		[DefaultValue("")]
		public string GetRegExp(TextBox tb)
		{
			return base.GetValue(tb);
		}

		public void SetRegExp(TextBox tb, string s)
		{
			base.SetValue(tb, s);
		}

		void RegExpValidate(object sender, CancelEventArgs e)
		{
			TextBox tb = sender as TextBox;
			Regex rex = new Regex(base.GetValue(tb));
			if(rex.IsMatch(tb.Text))
			{
				tb.BackColor = System.Drawing.Color.White;
			}
			else
			{
				e.Cancel = true;
				tb.BackColor = System.Drawing.Color.Pink;
			}
		}

	}

	public class InputMaskData
	{
		internal string Regex { get; set; }
		internal System.Drawing.Color ForeColor { get; set; }
		internal System.Drawing.Color BackColor { get; set; }
		internal bool TrapFocus { get; set; }
		internal bool ErrorState { get; set; }
		internal System.Drawing.Color OldForeColor { get; set; }
		internal System.Drawing.Color OldBackColor { get; set; }
	}

	[ProvideProperty("InputMask", typeof(TextBox))]
	[ProvideProperty("ErrorForeColor", typeof(TextBox))]
	[ProvideProperty("ErrorBackColor", typeof(TextBox))]
	[ProvideProperty("TrapFocus", typeof(TextBox))]
	public class TextBoxInputMaskExtender : ExtenderBase<TextBox, InputMaskData>
	{
		[Category("MegaHard Extenders - Input Mask")]
		[Description("Makes the TextBox match a regular expression input mask")]
		[DefaultValue("")]
		public string GetInputMask(TextBox tb)
		{
			return GetValue(tb).Regex;
		}
		public void SetInputMask(TextBox tb, string s)
		{
			var v = GetValue(tb);
			v.Regex = s;
			base.SetValue(tb, v);
		}

		protected override void OnSet(TextBox extendee, InputMaskData val)
		{
			extendee.Validating -= InputMaskValidate;
			if (!string.IsNullOrEmpty(val.Regex))
				extendee.Validating += InputMaskValidate;
		}

		new InputMaskData GetValue(TextBox tb)
		{
			var v = base.GetValue(tb);
			if (v != null)
				return v;
			v = new InputMaskData() { Regex = null, ForeColor = System.Drawing.Color.White, BackColor = System.Drawing.Color.Pink, TrapFocus = true };
			base.SetValue(tb, v);
			return v;
		}


		[Category("MegaHard Extenders - Input Mask")]
		[Description("Color to make the ForeColor on Mask failure")]
		public System.Drawing.Color GetErrorForeColor(TextBox tb)
		{
			return GetValue(tb).ForeColor;
		}
		public void SetErrorForeColor(TextBox tb, System.Drawing.Color val)
		{
			var v = GetValue(tb);
			v.ForeColor = val;
			base.SetValue(tb, v);
		}

		[Category("MegaHard Extenders - Input Mask")]
		[Description("Color to make the BackColor on Mask failure")]
		public System.Drawing.Color GetErrorBackColor(TextBox tb)
		{
			return GetValue(tb).BackColor;
		}
		public void SetErrorBackColor(TextBox tb, System.Drawing.Color val)
		{
			var v = GetValue(tb);
			v.BackColor = val;
			base.SetValue(tb, v);
		}
	
		[Category("MegaHard Extenders - Input Mask")]
		[Description("Trap focus on Mask failure")]
		[DefaultValue(true)]
		public bool GetTrapFocus(TextBox tb)
		{
			return GetValue(tb).TrapFocus;
		}
		public void SetTrapFocus(TextBox tb, bool val)
		{
			var v = GetValue(tb);
			v.TrapFocus = val;
			base.SetValue(tb, v);
		}

		void InputMaskValidate(object sender, CancelEventArgs e)
		{
			TextBox tb = sender as TextBox;
			var val = GetValue(tb);
			Regex rex = new Regex(val.Regex);
			Match match = rex.Match(tb.Text);
			if(match != Match.Empty && match.Length == tb.Text.Length)
			{
				if(val.ErrorState)
				{
					tb.ForeColor = val.OldForeColor;
					tb.BackColor = val.OldBackColor;
					val.ErrorState = false;
				}
			}
			else
			{
				e.Cancel = val.TrapFocus;
				if(!val.ErrorState)
				{
					val.OldForeColor = tb.ForeColor;
					val.OldBackColor = tb.BackColor;
					val.ErrorState = true;
				}
				tb.ForeColor = val.ForeColor;
				tb.BackColor = val.BackColor;
			}
		}
	}




}
