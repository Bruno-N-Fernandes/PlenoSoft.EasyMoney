using System;
using System.Collections.Generic;
using System.Linq;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Estatistica
	{
		public string Papel { get; }
		public decimal? ValorAtual =>  0;

		private IEnumerable<Movimento> Lancamentos { get; }
		private IEnumerable<Movimento> LancamentosDeCompra => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Compra);
		private IEnumerable<Movimento> LancamentosDeVenda => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Venda);
		private IEnumerable<Movimento> LancamentosDeRenda => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Rendimento);


		public int VolumeDeCompra => LancamentosDeCompra.Sum(l => l.Quantidade);
		public int VolumeDeVenda => -LancamentosDeVenda.Sum(l => l.Quantidade);
		public int VolumeDeRenda => LancamentosDeRenda.Sum(l => l.Quantidade);
		public int QuantidadeAtual => VolumeDeCompra - VolumeDeVenda;


		public decimal ValorInvestido => LancamentosDeCompra.Sum(l => l.CustoTotal);
		public decimal ValorResgatado => -LancamentosDeVenda.Sum(l => l.CustoTotal);
		public decimal ValorRendimento => LancamentosDeRenda.Sum(l => l.CustoTotal);
		public decimal ValorTaxas => Lancamentos.Sum(l => l.Taxas);


		public decimal PrecoMedioDeCompra => decimal.Round(ValorInvestido / Math.Max(VolumeDeCompra, 1), 2);
		public decimal PrecoMedioDeVenda => decimal.Round(ValorResgatado / Math.Max(VolumeDeVenda, 1), 2);
		public decimal ValorMedioDeRenda => decimal.Round(ValorRendimento / Math.Max(VolumeDeRenda, 1), 2);

		public decimal ValorMedioAtual => decimal.Round((ValorInvestido - ValorResgatado) / Math.Max(VolumeDeCompra - VolumeDeVenda, 1), 2);



		public Estatistica(string papel, IEnumerable<Movimento> lancamentos)
		{
			Papel = papel;
			Lancamentos = lancamentos;
		}
	}
}