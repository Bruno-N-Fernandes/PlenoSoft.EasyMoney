using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Cotacao : Entidade
	{
		public AtivoFinanceiro AtivoFinanceiro { get; set; }
		public Int64 AtivoFinanceiroId { get { return AtivoFinanceiro?.Id ?? 0; } set { AtivoFinanceiro = AtivoFinanceiro.Empty(value); } }

		public DateTime Data { get; set; }
		public decimal Valor { get; set; }
		public DateTime DataHoraInclusao { get; set; }
		public DateTime UltimaAlteracao { get; set; }
	}
}
