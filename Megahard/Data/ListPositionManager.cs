using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.Data
{
	public class ListPositionManager<T> : ObservableObject
	{
		public ListPositionManager(IList<T> col)
		{
			_list = col;
		}

		public IList<T> List
		{
			get
			{
				return _list;
			}
		}

		public T Current
		{
			get
			{
				return _list[Position];
			}
			set
			{
				int i = _list.IndexOf(value);
				if (i == -1)
					throw new InvalidOperationException("ListPositionManager - cannot set current to value not in the list");
				Position = i;
			}
		}

		public void MoveNext(bool wrap)
		{
			int count = _list.Count;
			if (count == 0)
			{
				Position = 0;
			}
			else if (Position == (count - 1))
			{
				if (wrap)
					Position = 0;
			}
			else
			{
				Position += 1;
			}
		}

		public void MovePrevious(bool wrap)
		{
			int count = _list.Count;
			if (count == 0)
			{
				Position = 0;
			}
			else if (Position == 0)
			{
				if (wrap)
					Position = count - 1;
			}
			else
			{
				Position -= 1;
			}
		}

		public bool CurrentIsValid
		{
			get { return Position >= 0 && Position < _list.Count; }
		}

		public void ResetPosition()
		{
			if (IsValidPosition(0))
				Position = 0;
		}
		public bool IsValidPosition(int pos)
		{
			return pos >= 0 && pos <= MaxPosition;
		}

		public int Position
		{
			get { return _pos; }
			set
			{
				if (_pos == value)
					return;
				if(value < 0 || value >= _list.Count)
					throw new ArgumentOutOfRangeException("Position");
				base.RaiseObjectChanging(new ObjectChangingEventArgs("Position", value));
				base.RaiseObjectChanging(new ObjectChangingEventArgs("Current", _list[value]));
				var old = _pos;
				_pos = value;
				base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Position", old, value));
				base.RaiseObjectChanged(new ObjectChangedEventArgs<T>("Current", _list[old], _list[value]));
			}
		}
		public int MaxPosition
		{
			get { return _list.Count - 1; }
		}

		int _pos;
		readonly IList<T> _list;
	}
}
