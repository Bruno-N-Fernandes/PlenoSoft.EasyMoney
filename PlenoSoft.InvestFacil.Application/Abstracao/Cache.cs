using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PlenoSoft.InvestFacil.Application.Abstracao
{
	public partial class Cache
	{
		public static readonly Cache Ativo = new Cache(TimeSpan.FromMinutes(10));
	}

	public partial class Cache
	{
		private readonly Dictionary<Type, Object> _caches = new Dictionary<Type, Object>();
		private readonly TimeSpan _timeOut;
		public Cache(TimeSpan timeOut) => _timeOut = timeOut;

		public IEnumerable<TEntidade> Setup<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd = null) where TEntidade : IUniqueId
		{
			var cache = ObterCache(getCollectionFromSource, onAdd);
			return cache.Obter(null);
		}

		public IEnumerable<TEntidade> Obter<TEntidade>(Func<TEntidade, Boolean> filter) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			return cache.Obter(filter);
		}

		public void Incluir<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Incluir(entidade);
		}

		public void Excluir<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Excluir(entidade);
		}

		public void Alterar<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Alterar(entidade);
		}

		private CacheOf<TEntidade> ObterCache<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd) where TEntidade : IUniqueId
		{
			var type = typeof(TEntidade);
			if (!_caches.TryGetValue(type, out var cache))
				cache = _caches[type] = new CacheOf<TEntidade>(_timeOut, getCollectionFromSource, onAdd);
			return ((CacheOf<TEntidade>)cache);
		}

		private IEnumerable<TEntidade> Empty<TEntidade>() { yield break; }
	}

	public sealed class CacheOf<TEntidade> where TEntidade : IUniqueId
	{
		private readonly TimeSpan _timeOut;
		private readonly Func<IEnumerable<TEntidade>> _getCollectionFromSource;
		private readonly Action<TEntidade, Int64> _onAdd;
		private readonly List<TEntidade> _collection = new List<TEntidade>();
		private DateTime _lastUpdate = DateTime.Now;
		private Int64 _id = 0;

		internal CacheOf(TimeSpan timeOut, Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd)
		{
			_timeOut = timeOut;
			_getCollectionFromSource = getCollectionFromSource;
			_onAdd = onAdd;
		}

		public Boolean EhValido => (_collection.Any() && (_lastUpdate.Add(_timeOut) > DateTime.Now));
		public List<TEntidade> ListaAtualizada => EhValido ? _collection : ColocarEmCache();

		public IEnumerable<TEntidade> Obter(Func<TEntidade, Boolean> filter)
		{
			return (filter == null) ? ListaAtualizada : ListaAtualizada.Where(filter);
		}

		public void Incluir(TEntidade entidade)
		{
			ListaAtualizada.Add(entidade);
			_onAdd?.Invoke(entidade, Interlocked.Increment(ref _id));
		}

		public void Alterar(TEntidade entidade)
		{
			Excluir(entidade);
			Incluir(entidade);
		}

		public void Excluir(TEntidade entidade)
		{
			ListaAtualizada.RemoveAll(d => d.UId == entidade.UId);
		}

		private List<TEntidade> ColocarEmCache()
		{
			var newCollection = _getCollectionFromSource?.Invoke();
			return AtualizarDados(newCollection ?? new TEntidade[0]);
		}

		private List<TEntidade> AtualizarDados(IEnumerable<TEntidade> newCollection)
		{
			_lastUpdate = DateTime.Now;
			_collection.Clear();
			_collection.AddRange(newCollection);
			_id = _collection.Any() ? Convert.ToInt64(_collection.Max(e => e.UId)) : 0L;
			return _collection;
		}
	}
}