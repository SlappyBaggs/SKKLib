using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Megahard.Data
{
	[Editor(typeof(Design.DropDownEditor<DataBinderEditor>), typeof(System.Drawing.Design.UITypeEditor))]
	public class DataBinder : IDisposable
	{
		public DataBinder(DataObject src, DataObject dest, bool twoWay)
		{
			System.Diagnostics.Debug.Assert(src != null && dest != null);
			propSource_ = src;
			propDestination_ = dest;
			TwoWay = twoWay;

			Source.ObjectChanged += Handle_ObjectChanged;
			if (TwoWay)
				Destination.ObjectChanged += Handle_ObjectChanged;
		}

		bool processing_;
		void Handle_ObjectChanged(object sender, ObjectChangedEventArgs e)
		{
			if (processing_ || !e.PropertyName.IsEmpty)
				return;

			processing_ = true;
			try
			{
				if (sender == Source)
					Destination.SetValue(e.NewValue);
				else if (TwoWay && sender == Destination)
					Source.SetValue(e.NewValue);
			}
			catch
			{
			}
			finally
			{
				processing_ = false;
			}
		}


		public bool TwoWay
		{
			get;
			set;
		}



		#region DataObject Source { get; readonly; }
		readonly DataObject propSource_;
		[Editor(typeof(Design.DropDownEditor<DataObjectEditor>), typeof(System.Drawing.Design.UITypeEditor))]
		public DataObject Source
		{
			get { return propSource_; }
		}
		#endregion

		#region DataObject Destination { get; readonly; }
		readonly DataObject propDestination_;
		[Editor(typeof(Design.DropDownEditor<DataObjectEditor>), typeof(System.Drawing.Design.UITypeEditor))]
		public DataObject Destination
		{
			get { return propDestination_; }
		}
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Source.ObjectChanged -= Handle_ObjectChanged;
			if (TwoWay)
				Destination.ObjectChanged -= Handle_ObjectChanged;
		}

		#endregion
	}

	public class DataBinderCollection : List<DataBinder>
	{
	}
}
