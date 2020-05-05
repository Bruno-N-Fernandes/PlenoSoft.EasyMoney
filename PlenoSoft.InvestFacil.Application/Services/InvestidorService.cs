using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Application.Repository;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Services
{
	public class InvestidorService
	{
		private IEnumerable<AtivoFinanceiro> _ativosFinanceiros => Cache.Ativo.Setup(() => new AtivoFinanceiroRepository().ObterTodos().Result);
		private readonly CarteiraRepository _carteiraRepository = new CarteiraRepository();
		private readonly MovimentoRepository _ativoFinanceiroMovimentoRepository = new MovimentoRepository();

		public async Task Preencher(Investidor investidor)
		{
			if (investidor != null)
			{
				var carteiras = await _carteiraRepository.ObterPor(investidor.Id);
				investidor.Carteiras.Add(carteiras);
			}
		}

		public async Task Preencher(Carteira carteira)
		{
			if (carteira != null)
			{
				var lancamentos = await _ativoFinanceiroMovimentoRepository.ObterPorCarteiraId(carteira.Id);
				carteira.Lancamentos.Add(lancamentos);
				await Preencher(lancamentos);
			}
		}

		public async Task Preencher(IEnumerable<Movimento> lancamentos)
		{
			foreach (var lancamento in lancamentos)
				await Preencher(lancamento);
		}

		public async Task Preencher(Movimento ativoFinanceiroMovimento)
		{
			await Task.Run(() =>
			{
				if (ativoFinanceiroMovimento != null)
				{
					var id = ativoFinanceiroMovimento.AtivoFinanceiroId;
					ativoFinanceiroMovimento.AtivoFinanceiro = ObterAtivoFinanceiroPorId(id);
				}
			});
		}

		private AtivoFinanceiro ObterAtivoFinanceiroPorId(long id)
		{
			return _ativosFinanceiros.FirstOrDefault(af => af.Id == id);
		}

		public AtivoFinanceiro ObterAtivoFinanceiroPorPapel(string papel)
		{
			return _ativosFinanceiros.FirstOrDefault(af => af.Papel == papel);
		}
	}
}