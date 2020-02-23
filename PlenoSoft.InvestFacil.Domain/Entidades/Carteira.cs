using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Carteira : Entidade
	{
		public string Nome { get; set; }

		[JsonIgnore]
		public Investidor Investidor { get; internal set; }

		public ActionList<Lancamento> Lancamentos { get; set; }

		public Carteira()
		{
			Lancamentos = new ActionList<Lancamento>(c => c.Carteira = this, c => c.Carteira = null);
		}

		public Lancamento Comprar(int quantidade, Ativo ativo, decimal valorUnitario, decimal taxas = 0.0M)
		{
			return NovoLancamento(quantidade, ativo, valorUnitario, taxas, Lancamento.Enum.Compra);
		}

		public Lancamento Vender(int quantidade, Ativo ativo, decimal valorUnitario, decimal taxas = 0.0M)
		{
			return NovoLancamento(-quantidade, ativo, valorUnitario, taxas, Lancamento.Enum.Venda);
		}

		public Lancamento Rentabilizar(int quantidade, Ativo ativo, decimal valorUnitario, decimal taxas = 0.0M)
		{
			return NovoLancamento(-quantidade, ativo, valorUnitario, taxas, Lancamento.Enum.Rendimento);
		}

		private Lancamento NovoLancamento(int quantidade, Ativo ativo, decimal valorUnitario, decimal taxas, Lancamento.Enum tipo)
		{
			var lancamento = new Lancamento { Ativo = ativo, Data = DateTime.UtcNow, Quantidade = quantidade, Taxas = taxas, Tipo = tipo, ValorUnitario = valorUnitario };
			return Movimentar(lancamento);
		}

		public Lancamento Movimentar(Lancamento lancamento)
		{
			return Lancamentos.Add(lancamento);
		}

		public IEnumerable<Estatistica> ObterEstatisticas(params string[] ativos)
		{
			var lancamentos = (ativos?.Any()).GetValueOrDefault() ? Lancamentos.Where(l => ativos.Contains(l.Ticker)) : Lancamentos;
			var lancamentosAgrupados = lancamentos.GroupBy(l => l.Ticker);
			return lancamentosAgrupados.Select(g => new Estatistica(g.Key, g));
		}
	}
}