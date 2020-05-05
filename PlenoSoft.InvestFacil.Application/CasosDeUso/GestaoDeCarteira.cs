using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Application.Repository;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.CasosDeUso
{
	public class GestaoDeCarteira
	{
		public async Task<IEnumerable<Carteira>> ObterCarteiras(Investidor investidor)
		{
			var carteiraRepository = new CarteiraRepository();
			return await carteiraRepository.ObterPor(investidor.Id);
		}

		public async Task Gravar(Carteira carteira)
		{
			var transacao = Conexao.Ativa.BeginTransaction();
			try
			{
				var ativoFinanceiroMovimentoRepository = new MovimentoRepository();
				var lancamentos = carteira.Lancamentos.Where(l => l.IsNew).ToArray();
				await ativoFinanceiroMovimentoRepository.Incluir(lancamentos, transacao);
				transacao.Commit();
			}
			catch (Exception)
			{
				transacao.Rollback();
				throw;
			}
		}
	}
}