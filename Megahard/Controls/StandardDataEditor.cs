﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Collections;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	class VisualUITypeEditor : VisualEditor
	{
		public VisualUITypeEditor(DataObject data)
		{
			data_ = data;
		}

		readonly DataObject data_;

		public override void DisplayModal()
		{
			var frm = new KryptonForm() { Text = "Visual Editor" };
			frm.Controls.Add(new Controls.DataBox() { Dock = DockStyle.Fill, Data = data_ });
			frm.ShowDialog();
		}

		public static IDataVisualizer CreateVisualizer()
		{
			return new Editor();
		}


		[ToolboxItem(false)]
		class Editor : KryptonTextBox, IServiceProvider, IDataVisualizer
		{
			public Editor()
			{
				editorService_ = new Megahard.Design.EditorService(this);
				MinimumSize = new System.Drawing.Size(80, 0);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.Enabled = false;
					DataError = null;
					Canceled = null;
					Committed = null;
					Data = null;
				}
				base.Dispose(disposing);
			}

			public string GetStringRepresentation()
			{
				return Text;
			}

			public event EventHandler<DataErrorEventArgs> DataError;
			protected virtual void OnDataError(DataErrorEventArgs arg)
			{
				var copy = DataError;
				if (copy != null)
					copy(this, arg);
			}


			void IDataVisualizer.InvokePopupEditor()
			{
				LaunchEditor();	
			}
			void OnDataChanged()
			{
				try
				{
					if (IsDisposed || Disposing)
						return;
					System.Diagnostics.Debug.Assert(!InvokeRequired);
					// reset some shit first
					ButtonSpecs.Clear();
					allowScrolling_ = false;
					hasEditorButton_ = false;
					// done reseting

					if (data_ == null)
						return;

					TypeConverter converter = TypeDescriptor.GetConverter(data_);
					editor_ = TypeDescriptor.GetEditor(data_, typeof(UITypeEditor)) as UITypeEditor;
					object val = data_.GetValue();

					readOnly_ = (data_.IsReadOnly && !typeof(IList).IsAssignableFrom(data_.GetDataType())) || !allowEditing_;

					bool hasStandardValues = converter != null && converter.GetStandardValuesSupported();

					if (hasStandardValues)
					{
						allowScrolling_ = true;
					}

					if (hasStandardValues && (editor_ == null || editor_.GetEditStyle() == UITypeEditorEditStyle.None))
					{
						editor_ = new Design.StandardValuesUIEditor(editor_);
					}

					if (converter != null && converter.GetPropertiesSupported() && editor_ == null)
					{
						var ed = new Design.DropDownEditor<Megahard.Controls.DataPanel>();
						ed.SetupVisualizer = panel =>
							{
								panel.FlowDirection = FlowDirection.TopDown;
								var props = converter.GetProperties(val);
								panel.PropertyFilter = prop => prop.IsBrowsable && props.Contains(prop);
								panel.SetupDataBox= (dbox, prop) =>
									{
										dbox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
										dbox.Label.Placement = Megahard.Data.Controls.DataBox.DataLabel.LabelPlacement.Left;
										dbox.Label.Size = 35;
									};
							};
						editor_ = ed;
					}

					bool textEditable = true;
					if ((hasStandardValues && converter.GetStandardValuesExclusive()) || (!converter.CanConvertFrom(typeof(string))))
						textEditable = false;


					hasEditorButton_ = editor_ != null && editor_.GetEditStyle() != UITypeEditorEditStyle.None;
					if (hasEditorButton_)
					{
						var editorButton = new ButtonSpecAny();
						if (editor_.GetEditStyle() == UITypeEditorEditStyle.DropDown)
						{
							editorButton.Text = "";
							editorButton.Type = PaletteButtonSpecStyle.DropDown;
						}
						else
						{
							editorButton.Text = "...";
							editorButton.Type = PaletteButtonSpecStyle.Generic;
						}
						editorButton.Click += delegate { LaunchEditor(); };
						ButtonSpecs.Add(editorButton);
					}

					if (editor_ != null && editor_.GetPaintValueSupported())
					{
						var previewControl = new ButtonSpecAny();
						int w = 20;
						// We need the minus two, but i really wish i knew where it came from, i suspect it is a krypton setting, and -2 is the right value
						// for the palette i am testing with, oh well ++Jeff
						int h = Height - Margin.Top - Margin.Bottom - 2;
						previewImg_ = new Bitmap(w, h);
						previewControl.Image = previewImg_;
						previewControl.Type = PaletteButtonSpecStyle.Generic;
						previewControl.Edge = PaletteRelativeEdgeAlign.Near;
						ButtonSpecs.Add(previewControl);
					}


					ReadOnly = readOnly_ || !textEditable;
					UpdateDisplayedValue(val, converter);
					data_.ObjectChanged -= data_DataChanged; // cya
					data_.ObjectChanged += data_DataChanged;
				}
				catch (Exception ex)
				{
					var args = new DataErrorEventArgs(ex);
					OnDataError(args);
					if (!args.Handled)
						throw;
				}
			}

			public DataObject CurrentValue
			{
				get { return Data; }
			}

			public DataObject Data
			{
				get { return data_; }
				set
				{
					if (data_ == value)
						return;
					if (data_ != null)
					{
						data_.ObjectChanged -= data_DataChanged;
					}
					data_ = value;
					OnDataChanged();
				}
			}

			bool EffectiveAllowEdits
			{
				get
				{
					return Data != null;// && !readOnly_;
				}
			}

			enum ScrollDirection { UP, DOWN };
			void ScrollValue(ScrollDirection sd)
			{
				if (!allowScrolling_ || !EffectiveAllowEdits)
					return;

				object curVal = data_.GetValue();
				var values = (from object x in TypeDescriptor.GetConverter(data_).GetStandardValues() select x).ToArray();
				int pos = -1;
				for (int i = 0; i < values.Length; ++i)
				{
					if (values[i].Equals(curVal))
					{
						pos = i;
						break;
					}
				}

				if (sd == ScrollDirection.DOWN)
				{
					pos = pos <= 0 ? values.Length - 1 : pos - 1;
				}
				else
				{
					pos = (pos == values.Length - 1) ? 0 : pos + 1;
				}
				data_.SetValue(values[pos]);
			}

			protected override void OnMouseWheel(MouseEventArgs e)
			{
				if (e.Delta > 0)
					ScrollValue(ScrollDirection.UP);
				else if (e.Delta < 0)
					ScrollValue(ScrollDirection.DOWN);

				base.OnMouseWheel(e);
			}

			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Up)
				{
					ScrollValue(ScrollDirection.UP);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Down)
				{
					ScrollValue(ScrollDirection.DOWN);
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.F4 && EffectiveAllowEdits)
				{
					LaunchEditor();
					e.Handled = true;
				}

				base.OnKeyDown(e);
			}

			bool txtEdited_;
			bool TextHasBeenEdited
			{
				get
				{
					return txtEdited_;
				}

				set
				{
					if (txtEdited_ == value)
						return;
					txtEdited_ = value;
					if (txtEdited_)
					{
						StateCommon.Back.Color1 = Color.LightBlue;
					}
					else
					{
						StateCommon.Back.Color1 = Color.Empty;
					}
				}
			}
			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				if (!ReadOnly && EffectiveAllowEdits)
				{
					if (e.KeyChar == '\r')
					{
						e.Handled = true;
						CommitValueFromTextBox();
					}
				}
				base.OnKeyPress(e);
			}

			protected override void OnTextChanged(EventArgs e)
			{
				TextHasBeenEdited = true;
			}

			protected override void OnValidated(EventArgs e)
			{
				CommitValueFromTextBox();
				base.OnValidated(e);
			}

			void CommitValueFromTextBox()
			{
				if (!TextHasBeenEdited || !EffectiveAllowEdits)
					return;
				try
				{
					var conv = TypeDescriptor.GetConverter(data_);
					var newVal = conv.ConvertFromString(Text);
					data_.SetValue(newVal);
					Parent.Focus();
				}
				catch
				{
					StateCommon.Back.Color1 = Color.PaleVioletRed;
				}
			}

			void internalUpdateDisplay()
			{
				try
				{
					if (!Disposing && !IsDisposed)
					{
						var val = data_ != null ? data_.GetValue() : null;
						var conv = data_ != null ? TypeDescriptor.GetConverter(data_) : null;
						UpdateDisplayedValue(val, conv);
						Invalidate(true);
					}
					updatePending_ = false;
				}
				catch (Exception ex)
				{
					var args = new DataErrorEventArgs(ex);
					OnDataError(args);
					if (!args.Handled)
						MessageBox.Show(ex.Message, "StandardDataEditor Update Display");
				}
			}

			public void UpdateDisplay()
			{
				if (updatePending_ || Disposing || IsDisposed)
					return;
				updatePending_ = true;
				this.BeginInvoke((MethodInvoker)internalUpdateDisplay);
				//this.AutoBeginInvoke(internalUpdateDisplay);
			}

			volatile bool updatePending_ = false;
			void data_DataChanged(object sender, Megahard.Data.ObjectChangedEventArgs e)
			{
				UpdateDisplay();
			}

			void UpdateDisplayedValue(object val, TypeConverter conv)
			{
				System.Diagnostics.Debug.Assert(!InvokeRequired);
				if (previewImg_ != null)
				{
					using (Graphics g = Graphics.FromImage(previewImg_))
					{
						editor_.PaintValue(val, g, new Rectangle(0, 0, previewImg_.Width, previewImg_.Height));
					}
				}

				if (conv.CanConvertTo(typeof(string)))
					Text = conv.ConvertToString(val);
				else if (val == null)
					Text = "";
				else
					Text = val.ToString();
				TextHasBeenEdited = false;
			}

			Data.DataObject data_;
			bool readOnly_;
			bool hasEditorButton_;
			UITypeEditor editor_;
			readonly Design.EditorService editorService_;
			Bitmap previewImg_;
			bool allowScrolling_;

			void LaunchEditor()
			{
				if (!hasEditorButton_ || !EffectiveAllowEdits)
					return;
				if (TextHasBeenEdited)
					CommitValueFromTextBox();

				Focus();
				Enabled = false;

				Megahard.Data.DataObject data = Data;
				this.BeginInvoke((MethodInvoker)delegate
				{
					try
					{

						object newVal = null;
						object instance;

						Megahard.Data.PropertyPath prop = null;
						data.GetBindingDetails(out instance, out prop);
						if (prop != null)
						{
							newVal = editor_.EditValue(new Data.TypeDescriptorContext(Container, instance, prop.ResolveProperty(instance), this), this, data.GetValue());
						}
						else
						{
							newVal = editor_.EditValue(this, instance);
						}

						//
						// It is possible for data to be readonly and still edit the object, it just means we are editing the reference value 
						// but not setting it back into data, which is fine in cases of like collections or when a property returns a reference to
						// an internal value and thus provides no prop set func mostly just know this is not a bug
						//
						if (!data.IsReadOnly)
							data.SetValue(newVal);
					}
					catch (Exception ex)
					{
						var args = new DataErrorEventArgs(ex);
						OnDataError(args);
						if (!args.Handled)
							MessageBox.Show(ex.Message, "StandardDataEditor");
					}
					finally
					{
						if (!Disposing && !IsDisposed)
							Enabled = true;
					}
				});
			}

			#region IServiceProvider Members

			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))
					return editorService_;
				return GetService(serviceType);
			}

			#endregion

			#region IDataEditor Members


			bool allowEditing_ = true;
			bool Megahard.Data.Visualization.IDataVisualizer.AllowEditing
			{
				get
				{
					return allowEditing_;
				}
				set
				{
					if (allowEditing_ == value)
						return;
					allowEditing_ = value;
					if (Data != null)
						this.AutoBeginInvoke(() => OnDataChanged());
				}
			}

			public void CommitChanges()
			{
				if (Committed != null)
					Committed(this, EventArgs.Empty);
			}

			Data.Visualization.CommitMode commitMode_ = Megahard.Data.Visualization.CommitMode.Automatic;
			Data.Visualization.UpdateMode updateMode_ = Megahard.Data.Visualization.UpdateMode.Automatic;

			public Data.Visualization.CommitMode CommitMode
			{
				get { return commitMode_; }
				set
				{
					commitMode_ = value;
				}
			}

			public Data.Visualization.UpdateMode UpdateMode
			{
				get { return updateMode_; }
				set
				{
					updateMode_ = value;
				}
			}

			#endregion

			#region IDataEditor Members


			public void CancelChanges()
			{
				if (Canceled != null)
					Canceled(this, EventArgs.Empty);
			}

			public event EventHandler Canceled;
			public event EventHandler Committed;

			public Control GUIObject { get { return this; } }

			#endregion
		}
	}
}
