using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlenoSoft.InvestFacil.Application.CasosDeUso;
using PlenoSoft.InvestFacil.Application.Services;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System.Collections.Generic;

namespace PlenoSoft.InvestFacil.WebApp.Controllers
{
	[ApiController, Route("[controller]")]
	public class CarteiraController : ControllerBase
	{
		private static Seguranca Seguranca { get; } = new Seguranca();
		private readonly InvestidorService _investidorService = new InvestidorService();

		private readonly ILogger<CarteiraController> _logger;
		public CarteiraController(ILogger<CarteiraController> logger) => _logger = logger;

		[HttpGet]
		public IEnumerable<Estatistica> Get(string nomeInvestidor, string nomeCarteira, string ativos)
		{
			var a = FinanceiroService.ObterCotacoes("FVPQ11");

			var investidor = Seguranca.Autenticar(nomeInvestidor, "123456").Result;
			var carteira = investidor.ObterCarteira(nomeCarteira);
			_investidorService.Preencher(carteira).Wait();
			var estatisticas = carteira.ObterEstatisticas(ativos?.Split(','));
			return estatisticas;
		}
	}
}