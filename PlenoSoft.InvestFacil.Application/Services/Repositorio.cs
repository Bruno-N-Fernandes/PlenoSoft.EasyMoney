using PlenoSoft.InvestFacil.Domain.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace PlenoSoft.InvestFacil.Application.Services
{
	public class Repositorio
	{
		private static readonly List<Ativo> _ativos = new List<Ativo>();
		private readonly List<Investidor> _investidores = new List<Investidor>();
		public Investidor[] Investidores => _investidores.ToArray();

		public Ativo ObterAtivo(string ativo)
		{
			var ticker = ativo?.Trim()?.ToUpper();
			return _ativos.SingleOrDefault(a => a.Ticker.Contains(ticker)) ?? NovoAtivo(ticker); ;
		}

		private Ativo NovoAtivo(string ticker)
		{
			var valorAsync = FinanceiroService.ObterValorAsync(ticker);
			var ativo = new Ativo(ticker, valorAsync) { Id = _investidores.Count + 1 };
			_ativos.Add(ativo);
			return ativo;
		}

		public Investidor ObterInvestidor(string nome)
		{
			return _investidores.SingleOrDefault(i => i.Nome.Contains(nome)) ?? NovoInvestidor(nome);
		}

		private Investidor NovoInvestidor(string nome)
		{
			var investidor = new Investidor { Id = _investidores.Count + 1, Nome = nome };
			_investidores.Add(investidor);
			return investidor;
		}
	}
}