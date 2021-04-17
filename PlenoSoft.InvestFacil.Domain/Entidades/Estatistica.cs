using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Estatistica
	{
		[JsonIgnore]
		public AtivoFinanceiro AtivoFinanceiro { get;  }

		public string Papel => AtivoFinanceiro.Papel;
		public decimal? ValorAtual => AtivoFinanceiro.Cotacao;

		private IEnumerable<Movimento> Lancamentos { get; }
		private IEnumerable<Movimento> LancamentosDeCompra => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Compra);
		private IEnumerable<Movimento> LancamentosDeVenda => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Venda);
		private IEnumerable<Movimento> LancamentosDeRenda => Lancamentos.Where(l => l.TipoMovimento == Movimento.Enum.Rendimento);


		public int VolumeDeCompra => LancamentosDeCompra.Sum(l => l.Quantidade);
		public int VolumeDeVenda => LancamentosDeVenda.Sum(l => l.Quantidade);
		public int VolumeDeRenda => LancamentosDeRenda.Sum(l => l.Quantidade);
		public int QuantidadeAtual => VolumeDeCompra - VolumeDeVenda;


		public decimal CustoDeAquisicao => LancamentosDeCompra.Sum(l => l.CustoTotal);
		public decimal ValorResgatado => LancamentosDeVenda.Sum(l => l.CustoTotal);
		public decimal ValorRendimento => LancamentosDeRenda.Sum(l => l.CustoTotal);
		public decimal ValorTaxas => Lancamentos.Sum(l => l.Taxas);


		public decimal CustoMedioDeCompra => decimal.Round(CustoDeAquisicao / Math.Max(VolumeDeCompra, 1), 2);
		public decimal PrecoMedioDeVenda => decimal.Round(ValorResgatado / Math.Max(VolumeDeVenda, 1), 2);
		public decimal ValorMedioDeRenda => decimal.Round(ValorRendimento / Math.Max(VolumeDeRenda, 1), 2);

		//public decimal ValorMedioAtual => decimal.Round((ValorResgatado - ValorInvestido) / Math.Max(VolumeDeCompra - VolumeDeVenda, 1), 2);

		public decimal ValorPatrimonial => QuantidadeAtual * AtivoFinanceiro.VPA;
		public decimal PrecoMercado => QuantidadeAtual * AtivoFinanceiro.Cotacao;


		public decimal ResultadoPatrimonial => (QuantidadeAtual * AtivoFinanceiro.VPA) + ValorResgatado - CustoDeAquisicao;
		public decimal ResultadoMercado => (QuantidadeAtual * AtivoFinanceiro.Cotacao) + ValorResgatado - CustoDeAquisicao;
		public decimal Realizado => ValorResgatado - CustoDeAquisicao;

		public Estatistica(AtivoFinanceiro ativoFinanceiro, IEnumerable<Movimento> lancamentos)
		{
			AtivoFinanceiro = ativoFinanceiro;
			Lancamentos = lancamentos;
		}
	}
}