using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class TipoAtivoFinanceiro : Entidade
	{
		public string Tipo { get; set; }
		public string Descricao { get; set; }
		public DateTime DataHoraInclusao { get; set; }
		public DateTime UltimaAlteracao { get; set; }

		public TipoAtivoFinanceiro() { }

		private static List<TipoAtivoFinanceiro> _empties = new List<TipoAtivoFinanceiro>();
		internal static TipoAtivoFinanceiro Empty(long id)
		{
			var ativo = _empties.FirstOrDefault(a => a.Id == id);
			if (ativo == null)
				_empties.Add(ativo = new TipoAtivoFinanceiro(id));

			return ativo;
		}
		private TipoAtivoFinanceiro(long id) => Id = id;
	}
}