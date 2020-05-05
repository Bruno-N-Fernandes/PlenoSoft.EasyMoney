using System;

namespace PlenoSoft.InvestFacil.Domain.Abstracao
{
	public interface IUniqueId
	{
		long UId { get; set; }
	}

	public interface IEntidade<TId> where TId : IComparable, IComparable<TId>, IConvertible, IEquatable<TId>, IFormattable
	{
		TId Id { get; set; }
	}

	public abstract class Entidade : IUniqueId, IEntidade<long>
	{
		public long Id { get; set; }
		long IUniqueId.UId { get => Id; set => Id = value; }
		public bool IsNew => Id <= 0;
	}
}