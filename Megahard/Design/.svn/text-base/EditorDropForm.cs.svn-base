using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Megahard.Design
{
	internal partial class EditorDropForm : Form
	{
		public EditorDropForm()
		{
			InitializeComponent();
		}

		bool open_;
		Control refControl_;
		Control dropCtl_;
		public void DoCloseDropDown()
		{
			open_ = false;
		}

		public void DoDropDown(Control dropCtl, Control refControl)
		{
			if (open_ == true)
				return;

			refControl_ = refControl;
			dropCtl_ = dropCtl;
			Form owner = refControl.FindForm();
			try
			{
				refControl.Capture = false;
				var minDesktopBounds = refControl.GetScreenBounds();

				System.Diagnostics.Debug.Assert(Controls.Count == 0);
				dropCtl.Visible = true;
				dropCtl.CreateControl();
				var origDockStyle = dropCtl.Dock;
				Controls.Add(dropCtl);
				dropCtl.Dock = DockStyle.Fill;
				Size prefSize = dropCtl.PreferredSize;
				Size = new Size(Math.Max(prefSize.Width, minDesktopBounds.Width), Math.Max(prefSize.Height, minDesktopBounds.Height));
				this.CenterAtPoint(minDesktopBounds.Right - Width / 2, minDesktopBounds.Bottom + Height / 2);
				LinkToOwner(owner);
				Show();
				open_ = true;
				while (open_)
				{
					MsgWaitForMultipleObjects(0, 0, true, 250, 255);
					Application.DoEvents();
				}
				dropCtl.Dock = origDockStyle;
				Visible = false;
			}
			finally
			{
				open_ = false;
				Controls.Clear();
				UnLinkFromOwner(owner);
				refControl_ = null;
				dropCtl_ = null;
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			//dropCtl_.Focus();
		}

		void LinkToOwner(Form owner)
		{
			if (owner == null)
				return;
			owner.AddOwnedForm(this);
			owner.Activated += owner_Activated;

			owner.LocationChanged += new EventHandler(owner_LocationChanged);
		}

		void owner_LocationChanged(object sender, EventArgs e)
		{
			var minDesktopBounds = refControl_.GetScreenBounds();
			this.CenterAtPoint(minDesktopBounds.Right - Width / 2, minDesktopBounds.Bottom + Height / 2);
		}

		void UnLinkFromOwner(Form owner)
		{
			if (owner == null)
				return;
			owner.RemoveOwnedForm(this);
			owner.Activated -= owner_Activated;
			owner.LocationChanged -= owner_LocationChanged;
		}

		void owner_Activated(object sender, EventArgs e)
		{
			if(open_)
				Activate();
		}

		protected override void OnDeactivate(EventArgs e)
		{
			if(Form.ActiveForm == this)
				DoCloseDropDown();
			base.OnDeactivate(e);
		}

		[DllImport("user32.dll")]
		public static extern int MsgWaitForMultipleObjects(
			int nCount,		// number of handles in array
			int pHandles,	// object-handle array
			bool bWaitAll,	// wait option
			int dwMilliseconds,	// time-out interval
			int dwWakeMask	// input-event type
			);

	}
}
