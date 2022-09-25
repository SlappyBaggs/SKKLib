using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
namespace Megahard.ComponentModel
{
	public abstract class ProvideAdditionalAttributes : TypeDescriptionProvider
	{
		protected ProvideAdditionalAttributes(TypeDescriptionProvider baseProvider) : base(baseProvider)
		{
		}

		/// <summary>
		/// The type we are providing attributes to
		/// </summary>
		protected ProvideAdditionalAttributes(Type t) : base(TypeDescriptor.GetProvider(t)) { }

		protected abstract bool OverrideExisting { get; }
		protected abstract Collections.ImmutableArray<Attribute> AddedAttributes { get; }
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return new AddAttributesDescriptor(base.GetTypeDescriptor(objectType, instance), OverrideExisting, AddedAttributes);
		}
	}

	public abstract class ProvideAdditionalAttributes<T> : ProvideAdditionalAttributes
	{
		protected ProvideAdditionalAttributes() : base(typeof(T)) { }
	}

	public class ForwardAttribute<From, To, Attr> : ComponentModel.ProvideAdditionalAttributes<To> where Attr : Attribute
	{
		public ForwardAttribute()
		{
			var attr = TypeDescriptor.GetAttributes(typeof(From))[typeof(Attr)];
			if (attr != null)
				attrs_ = new Attribute[] { attr };
		}

		readonly Collections.ImmutableArray<Attribute> attrs_;
		protected override bool OverrideExisting
		{
			get { return true; }
		}
		protected override Collections.ImmutableArray<Attribute> AddedAttributes
		{
			get { return attrs_; }
		}
	}


	public class AddAttributesDescriptor : CustomTypeDescriptor
	{
		public AddAttributesDescriptor(ICustomTypeDescriptor existing, bool overrideExisting, params Attribute[] attrs)
			: base(existing)
		{
			attrs_ = Collections.ImmutableArray.Create(attrs ?? new Attribute[0]);
			overrideExisting_ = overrideExisting;
		}

		public AddAttributesDescriptor(ICustomTypeDescriptor existing, bool overrideExisting, Collections.ImmutableArray<Attribute> attrs)
			: base(existing)
		{
			attrs_ = attrs ?? Collections.ImmutableArray.Empty;
			overrideExisting_ = overrideExisting;
		}

		readonly bool overrideExisting_;
		readonly Collections.ImmutableArray<Attribute> attrs_;

		public override AttributeCollection GetAttributes()
		{
			var existing = base.GetAttributes() ?? new AttributeCollection(new Attribute[0]);
			if (attrs_.Length == 0)
				return existing;
			if (existing.Count == 0)
				return new AttributeCollection(attrs_.ToArray());

			List<Attribute> newAttrs = new List<Attribute>(existing.Count + attrs_.Length);
			if (!overrideExisting_)
			{
				foreach (Attribute attr in existing)
					newAttrs.Add(attr);
				foreach (Attribute attr in attrs_)
				{
					var dup = FindDuplicate(existing, attr);
					if (dup == null)
						newAttrs.Add(attr);
				}
			}
			else
			{
				newAttrs.AddRange(attrs_);
				foreach (Attribute attr in existing)
				{
					if (FindDuplicate(attrs_, attr) == null)
						newAttrs.Add(attr);
				}
			}
			return new AttributeCollection(newAttrs.ToArray());
		}

		Attribute FindDuplicate(IEnumerable<Attribute> attrs, Attribute find)
		{
			object findTypeID = find.TypeId;
			foreach (Attribute attr in attrs)
			{
				if (attr.TypeId.Equals(findTypeID))
					return attr;
			}
			return null;
		}

		Attribute FindDuplicate(AttributeCollection attrs, Attribute find)
		{
			object findTypeID = find.TypeId;
			foreach (Attribute attr in attrs)
			{
				if (attr.TypeId.Equals(findTypeID))
					return attr;
			}
			return null;
		}
	}
}
