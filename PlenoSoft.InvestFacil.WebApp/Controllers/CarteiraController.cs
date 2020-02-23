using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlenoSoft.InvestFacil.Application.Services;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System.Collections.Generic;

namespace PlenoSoft.InvestFacil.WebApp.Controllers
{
	[ApiController, Route("[controller]")]
	public class CarteiraController : ControllerBase
	{
		private static Repositorio Repositorio { get; } = RepositorioFactory.GetRepositorio("Bruno N Fernandes", "Modal Mais");

		private readonly ILogger<CarteiraController> _logger;
		public CarteiraController(ILogger<CarteiraController> logger) => _logger = logger;

		[HttpGet]
		public IEnumerable<Estatistica> Get(string nomeInvestidor, string nomeCarteira, string ativos)
		{
			var investidor = Repositorio.ObterInvestidor(nomeInvestidor);
			var carteira = investidor.ObterCarteira(nomeCarteira);
			var estatisticas = carteira.ObterEstatisticas(ativos?.Split(','));
			return estatisticas;
		}
	}
}
