using System;
using System.Collections.Generic;

namespace PlenoSoft.InvestFacil.Domain.Abstracao
{
	public class ActionList<TItem> : List<TItem>
	{
		private readonly Action<TItem> _onAdd;
		private readonly Action<TItem> _onRemove;

		public ActionList(Action<TItem> onAdd = null, Action<TItem> onRemove = null)
		{
			_onAdd = onAdd;
			_onRemove = onRemove;
		}

		public ActionList<TItem> Add(params TItem[] list)
		{
			return Add(list as IEnumerable<TItem>);
		}

		public ActionList<TItem> Add(IEnumerable<TItem> list)
		{
			foreach (var item in list)
				Add(item);
			return this;
		}

		public new TItem Add(TItem item)
		{
			_onAdd?.Invoke(item);
			base.Add(item);
			return item;
		}

		public void Remove(IEnumerable<TItem> list)
		{
			foreach (var item in list)
				Remove(item);
		}

		public new void Remove(TItem item)
		{
			base.Remove(item);
			_onRemove?.Invoke(item);
		}

		public new void Clear()
		{
			foreach (var item in this)
				_onRemove?.Invoke(item);
			base.Clear();
		}

		public void Relink(Action<TItem> onRelink = null)
		{
			foreach (var item in this)
			{
				_onAdd?.Invoke(item);
				onRelink?.Invoke(item);
			}
		}

		public override string ToString()
		{
			return $"{GetType().Name}<{typeof(TItem).Name}>(Count = {Count})";
		}
	}
}
