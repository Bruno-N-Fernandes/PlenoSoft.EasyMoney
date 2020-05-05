using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Application.Repository;
using PlenoSoft.InvestFacil.Application.Services;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.CasosDeUso
{
	public class Seguranca
	{
		private readonly InvestidorRepository _investidorRepository = new InvestidorRepository();
		private readonly InvestidorService _investidorService = new InvestidorService();

		public async Task<Investidor> Autenticar(string login, string senha)
		{
			var investidor = await _investidorRepository.ObterPor(login, senha);

			await _investidorService.Preencher(investidor);

			return investidor;
		}

		public async Task Incluir(Investidor investidor)
		{
			var transacao = Conexao.Ativa.BeginTransaction();
			try
			{
				var investidorRepository = new InvestidorRepository();
				await investidorRepository.Incluir(investidor, transacao);
				Cache.Ativo.Incluir(investidor);
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
