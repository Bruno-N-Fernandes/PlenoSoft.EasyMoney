using System;
using System.Collections.Generic;
using System.Linq;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Estatistica
	{
		public string Ticker { get; }
		public decimal? ValorAtual => Lancamentos?.FirstOrDefault()?.Ativo?.ValorAtual ?? 0;

		private IEnumerable<Lancamento> Lancamentos { get; }
		private IEnumerable<Lancamento> LancamentosDeCompra => Lancamentos.Where(l => l.Tipo == Lancamento.Enum.Compra);
		private IEnumerable<Lancamento> LancamentosDeVenda => Lancamentos.Where(l => l.Tipo == Lancamento.Enum.Venda);
		private IEnumerable<Lancamento> LancamentosDeRenda => Lancamentos.Where(l => l.Tipo == Lancamento.Enum.Rendimento);


		public int VolumeDeCompra => LancamentosDeCompra.Sum(l => l.Quantidade);
		public int VolumeDeVenda => -LancamentosDeVenda.Sum(l => l.Quantidade);
		public int VolumeDeRenda => LancamentosDeRenda.Sum(l => l.Quantidade);
		public int QuantidadeAtual => VolumeDeCompra - VolumeDeVenda;


		public decimal ValorInvestido => LancamentosDeCompra.Sum(l => l.ValorTotal);
		public decimal ValorResgatado => -LancamentosDeVenda.Sum(l => l.ValorTotal);
		public decimal ValorRendimento => LancamentosDeRenda.Sum(l => l.ValorTotal);
		public decimal ValorTaxas => Lancamentos.Sum(l => l.Taxas);


		public decimal PrecoMedioDeCompra => decimal.Round(ValorInvestido / Math.Max(VolumeDeCompra, 1), 2);
		public decimal PrecoMedioDeVenda => decimal.Round(ValorResgatado / Math.Max(VolumeDeVenda, 1), 2);
		public decimal ValorMedioDeRenda => decimal.Round(ValorRendimento / Math.Max(VolumeDeRenda, 1), 2);

		public decimal ValorMedioAtual => decimal.Round((ValorInvestido - ValorResgatado) / Math.Max(VolumeDeCompra - VolumeDeVenda, 1), 2);



		public Estatistica(string ticker, IEnumerable<Lancamento> lancamentos)
		{
			Ticker = ticker;
			Lancamentos = lancamentos;
		}
	}
}