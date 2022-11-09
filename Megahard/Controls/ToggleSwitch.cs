using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Controls
{
	public enum ToggleOffMode
	{
		FlipHorizontal,
		FlipVertical
	}

	[DefaultEvent("ToggleSwitchToggled")]
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.Switch.ico")]
	[ToolboxItem(true)]
	public partial class ToggleSwitch : UserControlBase, ICustomTypeDescriptor
	{
		[Category("Toggle Switch")]
		[Description("Event fired when the toggle switch fires")]
		public event EventHandler<ToggledChangedEventArgs> ToggledChanged;

		public ToggleSwitch()
		{
			InitializeComponent();

			//ToggleImageOn = Properties.Resources.ToggleSwitch01;
			imageOff_ = null;
			toggled_ = false;
			imageAttributes_.SetColorKey(Color.Magenta, Color.Magenta);
			toggleOffMode_ = ToggleOffMode.FlipHorizontal;
		}

		[Category("Toggle Switch")]
		[Description("Is the switched toggled")]
		[DefaultValue(false)]
		public bool Toggled
		{
			get { return toggled_; }
			set
			{ 
				toggled_ = value;

				if (ToggledChanged != null)
					ToggledChanged(this, new ToggledChangedEventArgs(toggled_));
				Invalidate(); 
			}
		}
		private bool toggled_;

		[Category("Toggle Switch")]
		[Description("Image to use for toggle switch")]
		public Image ToggleImageOn
		{
			get { return imageOn_; }
			set
			{
				//if (value == null)
					//value = Properties.Resources.ToggleSwitch01;

				if (value == ToggleImageOn)
					return;
				imageOn_ = value;
				Invalidate(); 
			}
		}

		bool ShouldSerializeToggleImageOn()
		{
			return false;//return ToggleImageOn == Properties.Resources.ToggleSwitch01;
		}
		void ResetToggleImageOn()
		{
			//ToggleImageOn = Properties.Resources.ToggleSwitch01;
		}
		private Image imageOn_;

		[Category("Toggle Switch")]
		[Description("Image to use for toggle switch")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.All)]
		public Image ToggleImageOff
		{
			get { return imageOff_; }
			set
			{
				imageOff_ = value;
				Invalidate();
			}
		}
		private Image imageOff_;

		[Category("Toggle Switch")]
		[Description("Toggle switch off mode")]
		[DefaultValue(ToggleOffMode.FlipHorizontal)]
		public ToggleOffMode ToggleOffMode
		{
			get { return toggleOffMode_; }
			set
			{
				if (value == toggleOffMode_)
					return;
				toggleOffMode_ = value;
				Invalidate();
			}
		}
		private ToggleOffMode toggleOffMode_;

		private void ToggleSwitch_Click(object sender, EventArgs e)
		{
			Toggled = !Toggled;
		}

		private System.Drawing.Imaging.ImageAttributes imageAttributes_ = new System.Drawing.Imaging.ImageAttributes();
		private void ToggleSwitch_Paint(object sender, PaintEventArgs e)
		{
			if (Toggled)
			{
				e.Graphics.DrawImage(imageOn_, Point.Empty);
			}
			else
			{
				if (imageOff_ != null)
				{
					e.Graphics.DrawImage(imageOff_, Point.Empty);
				}
				else
				{
					RotateFlipType rft = RotateFlipType.RotateNoneFlipNone;
					if (ToggleOffMode == ToggleOffMode.FlipHorizontal) rft = RotateFlipType.RotateNoneFlipY;
					else if (ToggleOffMode == ToggleOffMode.FlipVertical) rft = RotateFlipType.RotateNoneFlipX;
					imageOn_.RotateFlip(rft);

					e.Graphics.DrawImage(imageOn_, Point.Empty);
					//e.Graphics.DrawImage(imageOn_, rect_, 0, 0, Size.Width, Size.Height, GraphicsUnit.Pixel, imageAttributes_);
					imageOn_.RotateFlip(rft);
				}
			}
		}
		//  ICustomTypeDescriptor Implementation	
		public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
		public String GetClassName() { return TypeDescriptor.GetClassName(this, true); }
		public String GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
		public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }
		public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
		public PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
		public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }
		public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
		public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }
		public object GetPropertyOwner(PropertyDescriptor pd) { return this; }
		public PropertyDescriptorCollection GetProperties(Attribute[] attr) { return GetProperties(); }
		public PropertyDescriptorCollection GetProperties()
		{
			PropertyDescriptorCollection pdsRet = new PropertyDescriptorCollection(null);
			PropertyDescriptorCollection pdsOld = TypeDescriptor.GetProperties(this, true);
			foreach (PropertyDescriptor pd in pdsOld)
				if (pd.Category != "Toggle Switch" || pd.Name != "ToggleOffMode" || imageOff_ == null)
					pdsRet.Add(pd);
			return pdsRet;
		}
	}

	public class ToggledChangedEventArgs : EventArgs
	{
		public ToggledChangedEventArgs(bool b) { toggled_ = b; }
		readonly bool toggled_;
		public bool Toggled { get { return toggled_; } }
	}
}
