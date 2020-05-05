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
		public long? InvestidorId => Investidor?.Id;


		public ActionList<Movimento> Lancamentos { get; set; }
		public DateTime UltimaAlteracao { get; set; }
		public DateTime DataHoraInclusao { get; set; }

		public Carteira()
		{
			Lancamentos = new ActionList<Movimento>(c => c.Carteira = this, c => c.Carteira = null);
		}

		public Movimento Comprar(int quantidade, AtivoFinanceiro ativo, decimal valorUnitario, DateTime? data, decimal taxas = 0.0M)
		{
			return NovoLancamento(quantidade, ativo, valorUnitario, data, taxas, Movimento.Enum.Compra);
		}

		public Movimento Vender(int quantidade, AtivoFinanceiro ativo, decimal valorUnitario, DateTime? data, decimal taxas = 0.0M)
		{
			return NovoLancamento(-quantidade, ativo, valorUnitario, data, taxas, Movimento.Enum.Venda);
		}

		public Movimento Rentabilizar(int quantidade, AtivoFinanceiro ativo, decimal valorUnitario, DateTime? data, decimal taxas = 0.0M)
		{
			return NovoLancamento(quantidade, ativo, valorUnitario, data, taxas, Movimento.Enum.Rendimento);
		}

		private Movimento NovoLancamento(int quantidade, AtivoFinanceiro ativo, decimal valorUnitario, DateTime? data, decimal taxas, Movimento.Enum tipo)
		{
			var lancamento = new Movimento { AtivoFinanceiro = ativo, Data = data ?? DateTime.UtcNow, Quantidade = quantidade, Taxas = taxas, TipoMovimento = tipo, PrecoUnitario = valorUnitario };
			return Movimentar(lancamento);
		}

		public Movimento Movimentar(Movimento lancamento)
		{
			return Lancamentos.Add(lancamento);
		}

		public IEnumerable<Estatistica> ObterEstatisticas(params string[] ativos)
		{
			var lancamentos = (ativos?.Any()).GetValueOrDefault() ? Lancamentos.Where(l => ativos.Contains(l.Papel)) : Lancamentos;
			var lancamentosAgrupados = lancamentos.GroupBy(l => l.Papel);
			return lancamentosAgrupados.Select(g => new Estatistica(g.Key, g));
		}
	}
}