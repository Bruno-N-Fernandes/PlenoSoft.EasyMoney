using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Movimento : Entidade
	{
		public enum Enum { Indefinido, Compra, Venda, Rendimento }

		[JsonIgnore]
		public Carteira Carteira { get; internal set; }
		public Int64? CarteiraId => Carteira?.Id;


		public Enum TipoMovimento { get; set; }
		public char Tipo { get { return TipoMovimento.ToString().FirstOrDefault(); } set { TipoMovimento = Parse(value); } }

		[JsonIgnore]
		public AtivoFinanceiro AtivoFinanceiro { get; set; }
		public Int64 AtivoFinanceiroId { get { return AtivoFinanceiro?.Id ?? 0; } set { AtivoFinanceiro = AtivoFinanceiro.Empty(value); } }

		public string Papel => AtivoFinanceiro?.Papel;
		public DateTime Data { get; set; }
		public int Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
		public decimal Taxas { get; set; }

		public DateTime DataHoraInclusao { get; set; }
		public DateTime UltimaAlteracao { get; set; }

		public decimal CustoTotal => ObterValorTotal();
		private decimal ObterValorTotal()
		{
			switch (TipoMovimento)
			{
				case Enum.Compra:
					return Quantidade * PrecoUnitario + Taxas;
				case Enum.Venda:
					return Quantidade * PrecoUnitario - Taxas;
				case Enum.Rendimento:
					return Quantidade * PrecoUnitario;
				default:
					return 0;
			}
		}

		private Enum Parse(char value)
		{
			switch (value)
			{
				case 'C': return Enum.Compra;
				case 'V': return Enum.Venda;
				case 'R': return Enum.Rendimento;
				default: return Enum.Indefinido;
			}
		}
	}
}