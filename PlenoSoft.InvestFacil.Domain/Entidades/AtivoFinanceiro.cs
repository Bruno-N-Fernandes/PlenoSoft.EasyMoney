using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class AtivoFinanceiro : Entidade
	{
		public string Papel { get; set; }
		public TipoAtivoFinanceiro TipoAtivoFinanceiro { get; set; }
		public long TipoAtivoFinanceiroId { get { return TipoAtivoFinanceiro?.Id ?? 0L; } set { TipoAtivoFinanceiro = TipoAtivoFinanceiro.Empty(value); } }

		public string Descricao { get; set; }
		public decimal VPA { get; set; }

		public DateTime DataHoraInclusao { get; set; }
		public DateTime UltimaAlteracao { get; set; }

		public AtivoFinanceiro() { }

		private static List<AtivoFinanceiro> _empties = new List<AtivoFinanceiro>();
		internal static AtivoFinanceiro Empty(long id)
		{
			var ativo = _empties.FirstOrDefault(a => a.Id == id);
			if (ativo == null)
				_empties.Add(ativo = new AtivoFinanceiro(id));

			return ativo;
		}
		private AtivoFinanceiro(long id) => Id = id;
	}
}