using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace System
{
	public static class mhObjectExtensions
	{
		public static dynamic AsDynamic<T>(this T ob)
		{
			return ob;
		}

		public static T GetEditor<T>(this object ob) where T : class
		{
			return TypeDescriptor.GetEditor(ob, typeof(T)) as T;
		}

		public static Megahard.Data.Visualization.IDataVisualizer CreateDataVisualizer(this object ob, Megahard.Data.Visualization.VisualizerStyle style = Megahard.Data.Visualization.VisualizerStyle.Normal)
		{
			System.Diagnostics.Debug.Assert(ob != null);
			if (ob == null)
				throw new ArgumentNullException("ob", "CreateDataVisualizer ob param cannot be null");
			Megahard.Data.Visualization.IDataVisualizer vis = null;
			var ed = ob.GetEditor<Megahard.Data.Visualization.VisualTypeEditor>();
			if (ed != null)
				vis = ed.CreateVisualizer(style);
			else
				vis = Megahard.Data.Visualization.VisualUITypeEditor.CreateVisualizer();
			vis.Data = new Megahard.Data.DataObject(ob);
			return vis;
		}

		public static Megahard.Data.Visualization.IDataVisualizer CreateDataVisualizer(this object ob, Megahard.Data.PropertyPath prop, Megahard.Data.Visualization.VisualizerStyle style = Megahard.Data.Visualization.VisualizerStyle.Normal)
		{
			var data = new Megahard.Data.DataObject(ob, prop);
			var ed = data.GetEditor<Megahard.Data.Visualization.VisualTypeEditor>();
			var vis = ed != null ? ed.CreateVisualizer(style) : Megahard.Data.Visualization.VisualUITypeEditor.CreateVisualizer();
			vis.Data = data;
			return vis;
		}
	}
}
