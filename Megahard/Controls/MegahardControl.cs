using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using ComponentFactory.Krypton.Toolkit;
using System.Drawing.Drawing2D;

namespace Megahard.Controls
{
	[Designer(typeof(Megahard.Design.ControlDesigner), typeof(System.ComponentModel.Design.IDesigner))]
	public class ControlBase : Control
	{
		protected ControlBase() 
		{
			/*
			// (1) To remove flicker we use double buffering for drawing
			SetStyle(
				  //ControlStyles.Opaque | 
				  ControlStyles.AllPaintingInWmPaint |
				  ControlStyles.OptimizedDoubleBuffer |
				  ControlStyles.ResizeRedraw, true);

			//InitializeComponent();

			// (2) Cache the current global palette setting
			palette_ = KryptonManager.CurrentGlobalPalette;

			// (3) Hook into palette events
			if (palette_ != null)
				palette_.PalettePaint += OnPalettePaint;

			// (4) We want to be notified whenever the global palette changes
			KryptonManager.GlobalPaletteChanged += OnGlobalPaletteChanged;

			// (1) Create redirection object to the base palette
			paletteRedirect_ = new PaletteRedirect(palette_);

			// (2) Create accessor objects for the back, border and content
			paletteBack_ = new PaletteBackInheritRedirect(paletteRedirect_);
			paletteBorder_ = new PaletteBorderInheritRedirect(paletteRedirect_);
			paletteContent_ = new PaletteContentInheritRedirect(paletteRedirect_);
			 * */
		}
		//protected ControlBase(string text) : base(text) { }
		//protected ControlBase(Control parent, string text) : base(parent, text){ }
		/*
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// (10) Unhook from the palette events
				if (palette_ != null)
				{
					palette_.PalettePaint -= OnPalettePaint;
					palette_ = null;
				}

				// (11) Unhook from the static events, otherwise we cannot be garbage collected
				KryptonManager.GlobalPaletteChanged -= OnGlobalPaletteChanged;
			}

			base.Dispose(disposing);
		}
		private void OnPalettePaint(object sender, PaletteLayoutEventArgs e)
		{
			Invalidate();
		}
		private void OnGlobalPaletteChanged(object sender, EventArgs e)
		{
			// (5) Unhook events from old palette
			if (palette_ != null)
				palette_.PalettePaint -= OnPalettePaint;

			// (6) Cache the new IPalette that is the global palette
			palette_ = KryptonManager.CurrentGlobalPalette;

			// (7) Hook into events for the new palette
			if (palette_ != null)
				palette_.PalettePaint += OnPalettePaint;

			// (8) Change of palette means we should repaint to show any changes
			Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (palette_ != null)
			{
				// (3) Get the renderer associated with this palette
				IRenderer renderer = palette_.GetRenderer();

				// (4) Create the rendering context that is passed into all renderer calls
				using (RenderContext renderContext = new RenderContext(this, e.Graphics, e.ClipRectangle, renderer))
				{
					// (5) Set style required when rendering
					paletteBack_.Style = PaletteBackStyle.PanelClient;
					//paletteBack_.Style = PaletteBackStyle.ButtonStandalone;
					//paletteBorder_.Style = PaletteBorderStyle.ButtonStandalone;
					paletteBorder_.Style = PaletteBorderStyle.FormMain;
					//paletteContent_.Style = PaletteContentStyle.LabelNormalPanel;

					// (6) ...perform renderer operations...

					// Do we need to draw the background?
					if (paletteBack_.GetBackDraw(PaletteState.Normal) == InheritBool.True)
					{
						// (12) Get the background path to use for clipping the drawing
						using (GraphicsPath path = renderer.RenderStandardBorder.GetBackPath(renderContext,
																														 ClientRectangle,
																														 paletteBorder_,
																														 VisualOrientation.Top,
																														 PaletteState.Normal))
						{
							// Perform drawing of the background clipped to the path
							mementoBack_ = renderer.RenderStandardBack.DrawBack(renderContext,
																										  ClientRectangle,
																										  path,
																										  paletteBack_,
																										  VisualOrientation.Top,
																										  PaletteState.Normal,
																										  mementoBack_);
						}
					}

					// (10) Do we need to draw the border?
					if (paletteBorder_.GetBorderDraw(PaletteState.Normal) == InheritBool.True)
					{
						// (11) Draw the border inside the provided rectangle area
						renderer.RenderStandardBorder.DrawBorder(renderContext,
																				   ClientRectangle,
																				   paletteBorder_,
																				   VisualOrientation.Top,
																				   PaletteState.Normal);
					}
				}


			}
		}




		private PaletteRedirect paletteRedirect_;
		private PaletteBackInheritRedirect paletteBack_;
		private PaletteBorderInheritRedirect paletteBorder_;
		private PaletteContentInheritRedirect paletteContent_;
		private IDisposable mementoBack_;
		private IPalette palette_;
		 */
	}
}
