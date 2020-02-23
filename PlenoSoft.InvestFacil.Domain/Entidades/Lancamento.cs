using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Text.Json.Serialization;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Lancamento : Entidade
	{
		public enum Enum { Compra, Venda, Rendimento }

		[JsonIgnore]
		public Carteira Carteira { get; internal set; }
		public Enum Tipo { get; set; }

		[JsonIgnore]
		public Ativo Ativo { get; set; }
		public string Ticker => Ativo?.Ticker;
		public DateTime Data { get; set; }
		public int Quantidade { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal Taxas { get; set; }
		public decimal ValorTotal => decimal.Round((Quantidade * ValorUnitario) + Taxas, 2);
	}
}