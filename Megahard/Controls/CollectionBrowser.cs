﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Megahard.Data;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel.Design;

namespace Megahard.Controls
{
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.CollectionBrowser.ico")]
	[Designer(typeof(CollectionBrowser.Designer))]
	public partial class CollectionBrowser : UserControlBase
	{
		public CollectionBrowser()
		{
			InitializeComponent();
			memberHdr_ = new Header(header1_, DefaultMemberTitle);
			catHdr_ = new Header(header_, DefaultCategoryTitle);
			defaultSplitterDistance_ = splitContainer_.SplitterDistance;
			oldSplitDist_ = splitContainer_.SplitterDistance;
			categories_.CollectionChanged += categories__CollectionChanged;
			catList_.Sorted = true;
			memberList_.Sorted = true;
		}

		int oldSplitDist_;
		readonly int defaultSplitterDistance_;
		const string DefaultCategoryTitle = "Categories";
		const string DefaultMemberTitle = "Members";

		[DefaultValue(false)]
		[Category("Layout")]
		[Design.ShowInSmartPanel]
		public bool IsSplitterFixed
		{
			get { return splitContainer_.IsSplitterFixed; }
			set
			{
				splitContainer_.IsSplitterFixed = value;
			}
		}

		[Category("Layout")]
		public int SplitterDistance
		{
			get { return splitContainer_.SplitterDistance; }
			set
			{
				int oldCpy = oldSplitDist_;
				oldSplitDist_ = SplitterDistance;
				splitContainer_.SplitterDistance = value;
				base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("SplitterDistance", oldCpy, SplitterDistance));
			}
		}

		bool ShouldSerializeSplitterDistance()
		{
			return SplitterDistance != defaultSplitterDistance_;
		}

		void ResetSplitterDistance()
		{
			SplitterDistance = defaultSplitterDistance_;
		}
	
		[Browsable(false)]
		public IEnumerable<string> SelectedCategories
		{
			get 
			{
				foreach (var catName in (from cat in categories_ where cat.IsSelected select cat.Name))
					yield return catName;
			}
		}

		public void ClearSelectedCategories()
		{
			catList_.ClearSelected();
		}
		[Browsable(false)]
		public IEnumerable<object> SelectedObjects
		{
			get
			{
				foreach (var ob in memberList_.SelectedItems)
					yield return ob;
			}
		}

		public void ClearSelectedObjects()
		{
			memberList_.ClearSelected();
		}
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Category("Appearance")]
		public Header CategoryHeader
		{
			get { return catHdr_; }
		}

		[DefaultValue(Orientation.Vertical)]
		[Category("Appearance")]
		public Orientation Orientation
		{
			get { return splitContainer_.Orientation; }
			set { splitContainer_.Orientation = value; }
		}


		[DefaultValue(true)]
		[Category("Behavior")]
		public bool AllowMultipleCategorySelect
		{
			get { return catList_.SelectionMode == SelectionMode.MultiExtended; }
			set
			{
				catList_.SelectionMode = value ? SelectionMode.MultiExtended : SelectionMode.One;
				contextMenuItem_.Enabled = AllowMultipleCategorySelect;
			}
		}

		[DefaultValue(true)]
		[Category("Behavior")]
		public bool AllowMultipleObjectSelect
		{
			get { return memberList_.SelectionMode == SelectionMode.MultiExtended; }
			set
			{
				memberList_.SelectionMode = value ? SelectionMode.MultiExtended : SelectionMode.One;
				contextMenuItem1_.Enabled = AllowMultipleObjectSelect;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Category("Appearance")]
		[Design.ShowInSmartPanel]
		public Header MemberHeader
		{
			get { return memberHdr_; }
		}

		readonly Header memberHdr_;
		readonly Header catHdr_;

		void categories__CollectionChanged(object sender, Megahard.Data.CollectionChangeEventArgs<Category> e)
		{
			if (e.ChangeType == CollectionChangeType.ItemAdded)
			{
				catList_.Items.Add(e.Item);
			}
			else if (e.ChangeType == CollectionChangeType.ItemRemoved)
			{
				catList_.Items.RemoveAt(e.Index);
			}
			else if (e.ChangeType == CollectionChangeType.Reset)
			{
				catList_.Items.Clear();
				memberList_.Items.Clear();
				foreach (var cat in categories_)
				{
					catList_.Items.Add(cat);
				}
			}
			ShowCategories = categories_.Count > 1;
			if (catList_.Items.Count == 1)
				catList_.SelectedIndex = 0;
		}
		const string NoCat = "(none)";
		readonly CategoriesCollection categories_ = new CategoriesCollection();

		public event EventHandler<CreateCategoryEventArgs> CreateCategory;
		protected virtual void OnCreateCategory(CreateCategoryEventArgs args)
		{
			var copy = CreateCategory;
			if (copy != null)
				copy(this, args);
		}

		void AddToCategory(string catName, object val)
		{
			var cat = categories_[catName];
			if (cat == null)
			{
				cat = new Category(catName);
				var args = new CreateCategoryEventArgs(catName);
				OnCreateCategory(args);
				cat.Image = args.Image;
				categories_.Add(cat);
			}
			if (val != null)
			{
				cat.Members.Add(val);
				if (SelectedCategories.Contains(catName))
					memberList_.Items.Add(val);
			}
		}

		void RebuildMemberList()
		{
			try
			{
				memberList_.BeginUpdate();
				memberList_.Items.Clear();
				foreach (var memList in (from cat in categories_ where cat.IsSelected select cat.Members))
					memberList_.Items.AddRange(memList.ToArray());
			}
			finally
			{
				memberList_.EndUpdate();
			}

		}

		bool ShowCategories
		{
			get
			{
				return !splitContainer_.Panel1Collapsed;
			}
			set
			{
				splitContainer_.Panel1Collapsed = !value;
			}
		}

		[DefaultValue(null)]
		[Category("Data")]
		[Description("Property to retrieve Category Name from")]
		public string CategoryMember
		{
			get;
			set;
		}

		[DefaultValue("")]
		[Category("Data")]
		[Description("Property to retrieve value shown in Members list")]
		public string DisplayMember
		{
			get { return memberList_.DisplayMember; }
			set { memberList_.DisplayMember = value; }
		}

		public void Add(object val, string cat)
		{
			AddToCategory(cat, val);
		}
		public void Add(object val)
		{
			string cat = null;
			if (CategoryMember.HasChars())
			{
				var prop = TypeDescriptor.GetProperties(val)[CategoryMember];
				if (prop != null)
				{
					var propval = prop.GetValue(val);
					cat = propval != null ? propval.ToString() : NoCat;
				}
			}
			Add(val, cat);
		}
		public void Clear()
		{
			categories_.Clear();
		}

		

		private void catList__SelectedValueChanged(object sender, EventArgs e)
		{
			foreach (var cat in categories_)
				cat.IsSelected = false;
			foreach (Category cat in catList_.SelectedItems)
				cat.IsSelected = true;
			RebuildMemberList();
			base.RaiseObjectChanged(new ObjectChangedEventArgs("SelectedCategories"));
		}
		
		private void memberList__SelectedValueChanged(object sender, EventArgs e)
		{
			base.RaiseObjectChanged(new ObjectChangedEventArgs("SelectedObjects"));
		}

		public IEnumerable<string> Categories
		{
			get
			{
				foreach (var cat in categories_)
					yield return cat.Name;
			}
		}

		public IEnumerable<object> GetCategoryMembers(string catName)
		{
			var cat = categories_[catName];
			if (cat != null)
			{
				foreach (object ob in cat.Members)
					yield return ob;
			}
		}

		class CategoriesCollection : ObservableCollection<Category>
		{
			public CategoriesCollection()
			{
			}
		
			public new IDisposable SuspendChangeEvents()
			{
				base.SuspendChangeEvents();
				return new Disposer(ResumeChangeEvents);
			}

			public override string ToString()
			{
				if (Count == 0)
					return "(none)";
				if (Count == 1)
					return this[0].Name;
				return "(multiple)";
			}

			public Category this[string catName]
			{
				get
				{
					catName = catName ?? NoCat;
					return this.FirstOrDefault(cat => cat.Name == catName);
				}
			}
		}

		class Category : KryptonListItem
		{
			public Category(string name)
			{
				Name = name ?? NoCat;
				base.ShortText = Name;
			}
			public bool IsSelected { get; set; }
			public string Name { get; private set; }

			public override string ToString()
			{
				return Name;
			}

			public List<object> Members
			{
				get { return members_; }
			}
			readonly List<object> members_ = new List<object>();
		}

		private void contextMenuItem__Click(object sender, EventArgs e)
		{
			try
			{
				catList_.BeginUpdate();
				catList_.SelectedValueChanged -= catList__SelectedValueChanged;
				catList_.SelectedIndices.Clear();
				foreach (var i in Enumerable.Range(0, catList_.Items.Count))
					catList_.SelectedIndices.Add(i);
				foreach (var cat in categories_)
					cat.IsSelected = true;
				RebuildMemberList();
			}
			finally
			{
				catList_.SelectedValueChanged += catList__SelectedValueChanged;
				catList_.EndUpdate();
				base.RaiseObjectChanged(new ObjectChangedEventArgs("SelectedCategories"));
			}
		}

		private void contextMenuItem1__Click(object sender, EventArgs e)
		{
			try
			{
				memberList_.BeginUpdate();
				memberList_.SelectedValueChanged -= memberList__SelectedValueChanged;
				memberList_.SelectedIndices.Clear();
				foreach (var i in Enumerable.Range(0, memberList_.Items.Count))
					memberList_.SelectedIndices.Add(i);
			}
			finally
			{
				memberList_.SelectedValueChanged += memberList__SelectedValueChanged;
				memberList_.EndUpdate();
				base.RaiseObjectChanged(new ObjectChangedEventArgs("SelectedObjects"));
			}
		}

		private void contextItemSelectNoObs__Click(object sender, EventArgs e)
		{
			memberList_.SelectedIndices.Clear();
		}

		private void contextItemSelectNoCats__Click(object sender, EventArgs e)
		{
			catList_.SelectedIndices.Clear();
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public class Header
		{
			internal Header(KryptonHeader hdr, string heading)
			{
				hdr_ = hdr;
				hdr_.Values.Heading = heading;
				defaultHeading_ = heading;
				bspecs_.CollectionChanged += new EventHandler<CollectionChangeEventArgs<ButtonSpecAny>>(bspecs__CollectionChanged);
			}

			void bspecs__CollectionChanged(object sender, CollectionChangeEventArgs<ButtonSpecAny> e)
			{
				hdr_.ButtonSpecs.Clear();
				hdr_.ButtonSpecs.AddRange(bspecs_.ToArray());
			}
			readonly KryptonHeader hdr_;
			readonly string defaultHeading_;
			internal KryptonHeader Control
			{
				get { return hdr_; }
			}

			[DefaultValue("")]
			public string Description
			{
				get { return hdr_.Values.Description; }
				set { hdr_.Values.Description = value; }
			}
			
			public string Heading
			{
				get { return hdr_.Values.Heading; }
				set { hdr_.Values.Heading = value; }
			}
			bool ShouldSerializeHeading()
			{
				return Heading != defaultHeading_;
			}
			void ResetHeading()
			{
				Heading = defaultHeading_;
			}

			[DefaultValue(null)]
			public Image Image
			{
				get { return hdr_.Values.Image; }
				set { hdr_.Values.Image = value; }
			}
		
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
			public ObservableCollection<ButtonSpecAny> ButtonSpecs
			{
				get { return bspecs_; }
			}

			internal bool IsPtOverButtonSpec(Point x)
			{
				return Control.GetViewManager().ComponentFromPoint(Control.PointToClient(x)) != null;
			}

			internal IComponent SelectButtonSpec(Point pt)
			{
				return Control.GetViewManager().ComponentFromPoint(pt);
			}

			[Browsable(false)]
			public bool IsDefault
			{
				get
				{
					return Heading == defaultHeading_ && Image == null && ButtonSpecs.Count == 0 &&
						   Description == "";
				}
			}

			public override string ToString()
			{

				return IsDefault ? "" : "(modified)";
			}


			readonly ObservableCollection<ButtonSpecAny> bspecs_ = new ObservableCollection<ButtonSpecAny>();
		}


		class Designer : Design.ControlDesigner
		{
			public override void Initialize(IComponent component)
			{
				base.Initialize(component);
				var cb = component as CollectionBrowser;
				cb.MemberHeader.Control.MouseClick += (sender, args) =>
					{
						if(args.Button == MouseButtons.Left)
						{
							var bs = cb.MemberHeader.SelectButtonSpec(args.Location);
							if(bs != null)
							{
								GetService<ISelectionService>().SetSelectedComponents(new[] { bs }, SelectionTypes.Auto);
							}
						}
					};
				
				cb.CategoryHeader.Control.MouseClick += (sender, args) =>
					{
						if (args.Button == MouseButtons.Left)
						{
							var bs = cb.CategoryHeader.SelectButtonSpec(args.Location);
							if (bs != null)
							{
								GetService<ISelectionService>().SetSelectedComponents(new[] { bs }, SelectionTypes.Auto);
							}
						}
					};

				cb.CategoryHeader.Control.MouseDoubleClick += (sender, args) =>
					{
						if (args.Button == MouseButtons.Left)
						{
							var bs = cb.CategoryHeader.SelectButtonSpec(args.Location);
							if (bs != null)
							{
								GetService<IDesignerHost>().GetDesigner(bs).DoDefaultAction();
							}
						}
					};
				cb.MemberHeader.Control.MouseDoubleClick += (sender, args) =>
				{
					if (args.Button == MouseButtons.Left)
					{
						var bs = cb.MemberHeader.SelectButtonSpec(args.Location);
						if (bs != null)
						{
							GetService<IDesignerHost>().GetDesigner(bs).DoDefaultAction();
						}
					}
				};

				var chgServ = GetService<IComponentChangeService>();
				chgServ.ComponentRemoving += new ComponentEventHandler(chgServ_ComponentRemoving);
				cb.AttachObserver("SplitterDistance", args=>
					{
						chgServ.OnComponentChanged(Component, args.PropertyName.ResolveProperty(Component), args.OldValue, args.NewValue);
					}
				);
				

			}

			void chgServ_ComponentRemoving(object sender, ComponentEventArgs e)
			{
				var cb = Control as CollectionBrowser;
				if (e.Component == cb)
				{
					var host = GetService<IDesignerHost>();
					var chgServ = GetService<IComponentChangeService>();
					chgServ.OnComponentChanging(cb, null);
					foreach (var bs in cb.CategoryHeader.ButtonSpecs.ToArray())
					{
						host.DestroyComponent(bs);
					}
					foreach (var bs in cb.MemberHeader.ButtonSpecs.ToArray())
					{
						host.DestroyComponent(bs);
					}
					chgServ.OnComponentChanged(cb, null, null, null);
				}
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					var chgSrv = GetService<IComponentChangeService>();
					chgSrv.ComponentRemoving -= chgServ_ComponentRemoving;
				}
				base.Dispose(disposing);
			}


			protected override bool GetHitTest(Point point)
			{
				var cb = Control as CollectionBrowser;
				if (cb.CategoryHeader.IsPtOverButtonSpec(point) || cb.MemberHeader.IsPtOverButtonSpec(point))
					return true;
				cb.CategoryHeader.Control.GetViewManager().MouseLeave(EventArgs.Empty);
				cb.MemberHeader.Control.GetViewManager().MouseLeave(EventArgs.Empty);

				var src = cb.splitContainer_ as ISeparatorSource;
				// I don't know what exactly he was looking for here... SKKSEARCH... one of the panels maybe???
				//if (src.SeparatorClientRectangle.Contains(cb.splitContainer_.PointToClient(point)))
					//return true;

				return base.GetHitTest(point);
			}

	
			/*
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == (int)WindowsMessages.WM_LBUTTONUP)
				{
					var cb = Control as CollectionBrowser;
					var ptc = new PlatformInvoke.POINTC() { x = PlatformInvoke.LOWORD(m.LParam), y = PlatformInvoke.HIWORD(m.LParam) };
					PlatformInvoke.MapWindowPoints(cb.Handle, cb.CategoryHeader.Control.Handle, ptc, 1);
					if (SelectButtonSpec(cb.CategoryHeader.Control, new Point(ptc.x, ptc.y)))
						return;
					ptc = new PlatformInvoke.POINTC() { x = PlatformInvoke.LOWORD(m.LParam), y = PlatformInvoke.HIWORD(m.LParam) };
					PlatformInvoke.MapWindowPoints(cb.Handle, cb.MemberHeader.Control.Handle, ptc, 1);
					if (SelectButtonSpec(cb.MemberHeader.Control, new Point(ptc.x, ptc.y)))
						return;
				}
				base.WndProc(ref m);
			}
			 * */
			public override ICollection AssociatedComponents
			{
				get
				{
					var cb = Component as CollectionBrowser;
					ArrayList ar = new ArrayList();
					ar.AddRange(cb.MemberHeader.ButtonSpecs);
					ar.AddRange(cb.CategoryHeader.ButtonSpecs);
					return ar;
				}
			}

		}

		private void splitContainer__SplitterMoved(object sender, SplitterEventArgs e)
		{
			int oldCpy = oldSplitDist_;
			oldSplitDist_ = SplitterDistance;
	
			base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("SplitterDistance", oldCpy, SplitterDistance));
		}
	}

	public class CreateCategoryEventArgs : EventArgs
	{
		public CreateCategoryEventArgs(string catName)
		{
			CategoryName = catName;
		}
		public string CategoryName { get; private set; }
		public Image Image { get; set; }
	}
}
