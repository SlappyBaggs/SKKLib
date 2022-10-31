using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	[ToolboxItem(true)]
	public partial class CollectionVisualizer : DataVisualizer
	{
		public CollectionVisualizer()
		{
			InitializeComponent();
			UpdateCmds();
		}

		[Category("Data")]
		[DefaultValue(null)]
		public string DisplayMember
		{
			get { return listBox_.DisplayMember; }
			set
			{
				listBox_.DisplayMember = value;
			}
		}


		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			base.OnObjectChanged(args);
			if (args.PropertyName == "AllowAdd")
			{
				addCmd_.Enabled = AllowAdd;
			}
			if (args.PropertyName == "AllowDelete")
			{
				delCmd_.Enabled = AllowDelete;
			}
			if (args.PropertyName == "AllowReorder")
			{
				moveUpCmd_.Enabled = AllowReorder;
				moveDownCmd_.Enabled = AllowReorder;
			}
			if (args.PropertyName == "ReorderBarPosition")
			{
				if (ReorderBarPosition == LeftRight.Left)
					reorderBar_.Dock = DockStyle.Left;
				else if (ReorderBarPosition == LeftRight.Right)
					reorderBar_.Dock = DockStyle.Right;
			}

			if (args.PropertyName == "Data")
			{
				var oldData = args.OldValue as Megahard.Data.DataObject;
				var oldValue = oldData != null ? oldData.GetValue() : null;
				var newData = args.NewValue as Megahard.Data.DataObject ?? Data;
				var newValue = newData != null ? newData.GetValue() : null;

				if(oldValue is IObservableCollection)
				{
					var obCol = oldValue as IObservableCollection;
					obCol.CollectionChanged -= Items_CollectionChanged;
				}

				SelectedIndex = -1;

				if(newValue is IObservableCollection)
				{
					var obCol = newValue as IObservableCollection;
					obCol.CollectionChanged += Items_CollectionChanged;
				}
				RefreshList();
			}
		}

		ICollection CollectionData
		{
			get { return base.Data != null ? base.Data.GetValue() as ICollection : null; }
		}
		IList ListData
		{
			get { return base.Data != null ? base.Data.GetValue() as IList : null; }
		}

		IBindingList BindingListData
		{
			get { return Data != null ? Data.GetValueAs<IBindingList>() : null; }
		}

		public event EventHandler<CollectionVisualizerAddingNew> AddingNew;
		protected virtual void OnAddingNew(CollectionVisualizerAddingNew arg)
		{
			var copy = AddingNew;
			if(copy != null)
				copy(this, arg);
		}
		private void addCmd__Execute(object sender, EventArgs e)
		{
				var args = new CollectionVisualizerAddingNew();
				OnAddingNew(args);
				var item = args.NewItem;
				if (item != null)
				{
					ListData.Add(item);
					return;
				}

				var blist = BindingListData;
				if (blist != null && blist.AllowNew)
				{
					blist.AddNew();
					return;
				}
				throw new NotSupportedException("CollectionVisualizer cannot add a new item to this collection");
		}

		private void delCmd__Execute(object sender, EventArgs e)
		{
			if (SelectedIndex == -1)
				return;
			ListData.RemoveAt(SelectedIndex);
		}

		public event EventHandler SelectedIndexChanged;
		protected virtual void OnSelectedIndexChanged()
		{
			if (suspendIndexChgEvent_ > 0)
				return;
			UpdateCmds();


			var copy = SelectedIndexChanged;
			if(copy != null)
				copy(this, EventArgs.Empty);
			base.RaiseObjectChanged(new ObjectChangedEventArgs("SelectedItem", null, SelectedItem));
		}

		private void UpdateCmds()
		{
			if (SelectedIndex == -1)
			{
				delCmd_.Enabled = false;
				moveUpCmd_.Enabled = false;
				moveDownCmd_.Enabled = false;
			}
			else
			{
				delCmd_.Enabled = true;
				moveUpCmd_.Enabled = SelectedIndex > 0;
				moveDownCmd_.Enabled = SelectedIndex < (CollectionData.Count - 1);
			}
		}
		

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get { return listBox_.SelectedIndex; }
			set
			{
				if (SelectedIndex == value)
					return;
				listBox_.SelectedIndex = value;
			}
		}

		[Browsable(false)]
		public object SelectedItem
		{
			get
			{
				var ld = ListData;
				if (ld == null)
					return null;
				var index = SelectedIndex;
				return index == -1 ? null : ld[index];
			}
		}

		public T GetSelectedItemAs<T>() where T : class
		{
			return SelectedItem as T;
		}

		void PauseRefreshList()
		{
			suspendRefreshList_ += 1;
		}

		void ResumeRefreshList()
		{
			suspendRefreshList_ -= 1;
			System.Diagnostics.Debug.Assert(suspendRefreshList_ >= 0);
			if (suspendRefreshList_ == 0)
				RefreshList();
		}
		int suspendRefreshList_;

		int suspendIndexChgEvent_;
		int indexAtPause_;
		void PauseIndexChangeEvent()
		{
			if (suspendIndexChgEvent_ == 0)
				indexAtPause_ = SelectedIndex;
			suspendIndexChgEvent_ += 1;
		}

		void ResumeIndexChangeEvent()
		{
			suspendIndexChgEvent_ -= 1;
			System.Diagnostics.Debug.Assert(suspendIndexChgEvent_ >= 0);
			if(suspendIndexChgEvent_ == 0 && indexAtPause_ != SelectedIndex)
				OnSelectedIndexChanged();
		}
		
		void Items_CollectionChanged(object sender, Megahard.Data.CollectionChangeEventArgs e)
		{
			RefreshList();
		}

		void RefreshList()
		{
			if (suspendRefreshList_ > 0)
				return;
			try
			{
				PauseIndexChangeEvent();
				int oldSelectedIndex = SelectedIndex;

				try
				{
					listBox_.BeginUpdate();
					listBox_.SuspendLayout();
					SelectedIndex = -1;
					listBox_.Items.Clear();

					if(ListData == null)
					{
						addCmd_.Enabled = false;
						return;
					}
					
					foreach (var item in CollectionData)
					{
						listBox_.Items.Add(item);
					}
				}
				finally
				{
					listBox_.EndUpdate();
					listBox_.ResumeLayout();
				}

				if (oldSelectedIndex == -1 && listBox_.Items.Count > 0)
				{
					SelectedIndex = 0;
				}
				else if (listBox_.Items.Count <= oldSelectedIndex)
				{
					SelectedIndex = listBox_.Items.Count - 1;
				}
				else
				{
					SelectedIndex = oldSelectedIndex;
				}
				addCmd_.Enabled = true;
			}
			finally
			{
				ResumeIndexChangeEvent();
			}
		}

		private void listBox__SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSelectedIndexChanged();
		}

		private void moveUpCmd__Execute(object sender, EventArgs e)
		{
			var list = ListData;
			int index = SelectedIndex;
			var item = list[index];
			try
			{
				PauseRefreshList();
				list.RemoveAt(index);
				list.Insert(index - 1, item);
			}
			finally
			{
				ResumeRefreshList();
			}
			SelectedIndex = index - 1;
		}

		private void moveDownCmd__Execute(object sender, EventArgs e)
		{
			var list = ListData;
			int index = SelectedIndex;
			var item = list[index];
			try
			{
				PauseRefreshList();
				list.RemoveAt(index);
				list.Insert(index + 1, item);
			}
			finally
			{
				ResumeRefreshList();
			}
			SelectedIndex = index + 1;
		}

		#region public bool AllowAdd { get; set; }
		bool propAllowAdd_ = true;
		[DefaultValue(true)]
		[Megahard.Data.ObservableProperty]
		public bool AllowAdd
		{
			get { return propAllowAdd_; }
			set
			{
				if(AllowAdd == value)
					return;
				RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("AllowAdd", value));
				var oldVal = AllowAdd;
				propAllowAdd_ = value;
				RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("AllowAdd", oldVal, value));
			}
		}
		#endregion

		#region public bool AllowReorder { get; set; }
		bool propAllowReorder_ = true;
		[DefaultValue(true)]
		[Megahard.Data.ObservableProperty]
		public bool AllowReorder
		{
			get { return propAllowReorder_; }
			set
			{
				if(AllowReorder == value)
					return;
				RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("AllowReorder", value));
				var oldVal = AllowReorder;
				propAllowReorder_ = value;
				RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("AllowReorder", oldVal, value));
			}
		}
		#endregion

		#region public bool AllowDelete { get; set; }
		bool propAllowDelete_ = true;
		[DefaultValue(true)]
		[Megahard.Data.ObservableProperty]
		public bool AllowDelete
		{
			get { return propAllowDelete_; }
			set
			{
				if(AllowDelete == value)
					return;
				RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("AllowDelete", value));
				var oldVal = AllowDelete;
				propAllowDelete_ = value;
				RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("AllowDelete", oldVal, value));
			}
		}
		#endregion

		public void RegisterAddObject(string description, Image img, Func<object> creationFunc)
		{
			EventHandler handler = (s,args)=>ListData.Add(creationFunc());
			var item = new KryptonContextMenuItem(description, img, handler);
			if (img != null)
				addMenuItems_.ImageColumn = true;
			addMenuItems_.Items.Add(item);
			if (addMenuItems_.Items.Count == 1)
			{
				addButtonSpec_.KryptonCommand = null;
				addButtonSpec_.KryptonContextMenu = addContextMenu_;
				addButtonSpec_.Type = PaletteButtonSpecStyle.DropDown;
				addButtonSpec_.Text = "Add";
			}
		}

		#region public LeftRight ReorderBarPosition { get; set; }
		LeftRight propReorderBarPosition_ = LeftRight.Left;
		[DefaultValue(LeftRight.Left)]
		[Megahard.Data.ObservableProperty]
		public LeftRight ReorderBarPosition
		{
			get { return propReorderBarPosition_; }
			set
			{
				if(ReorderBarPosition == value)
					return;
				RaiseObjectChanging(new Megahard.Data.ObjectChangingEventArgs("ReorderBarPosition", value));
				var oldVal = ReorderBarPosition;
				propReorderBarPosition_ = value;
				RaiseObjectChanged(new Megahard.Data.ObjectChangedEventArgs("ReorderBarPosition", oldVal, value));
			}
		}
		#endregion

		
	}

	

	public class CollectionVisualizerAddingNew : EventArgs
	{
		public CollectionVisualizerAddingNew()
		{
			
		}
		public object NewItem
		{
			get;
			set;
		}
	}
}
