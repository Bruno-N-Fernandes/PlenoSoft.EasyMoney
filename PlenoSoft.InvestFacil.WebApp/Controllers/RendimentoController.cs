using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlenoSoft.InvestFacil.Application.CasosDeUso;
using PlenoSoft.InvestFacil.Application.Services;
using System;
using System.Text.Json;

namespace PlenoSoft.InvestFacil.WebApp.Controllers
{
	[ApiController, Route("[controller]")]
	public class RendimentoController : ControllerBase
	{
		private static Seguranca Seguranca { get; } = new Seguranca();
		private readonly InvestidorService _investidorService = new InvestidorService();

		private readonly ILogger<CarteiraController> _logger;
		public RendimentoController(ILogger<CarteiraController> logger) => _logger = logger;


		[HttpGet]
		public Rendimento[] Get(string nomeInvestidor, string nomeCarteira, string ativos)
		{
			var investidor = Seguranca.Autenticar(nomeInvestidor, "123456").Result;
			var carteira = investidor.ObterCarteira(nomeCarteira);
			_investidorService.Preencher(carteira).Wait();


			var jsonString = cRendimentos.Replace("'", "\"");
			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true,
				AllowTrailingCommas = true,
				IgnoreNullValues = true,
				IgnoreReadOnlyProperties = true,
				PropertyNameCaseInsensitive = true,
			};


			var rendimentos = JsonSerializer.Deserialize<Rendimento[]>(jsonString, options);
			
			foreach (var rendimento in rendimentos)
			{
				var ativo = _investidorService.ObterAtivoFinanceiroPorPapel(rendimento.Codigo);
				carteira.Rentabilizar(rendimento.Saldo, ativo, rendimento.Valor, rendimento.Pagamento.ToUniversalTime());
			}

			//new GestaoDeCarteira().Gravar(carteira).Wait();


			return rendimentos;
		}

		public class Rendimento
		{
			public string Codigo { get; set; }
			public DateTime Direito { get; set; }
			public DateTime Pagamento { get; set; }
			public decimal Valor { get; set; }
			public string Nome { get; set; }
			public int Saldo { get; set; }
			public decimal Pm { get; set; }
			public string TipoAtivo { get; set; }
			public decimal Total { get; set; }

		}

		private const string cRendimentos = @"
[
	{
		'codigo': 'SCPF11',
		'direito': '2020-02-06T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 0.106442,
		'nome': 'Rendimento',
		'saldo': 11,
		'pm': 13.593636363636364,
		'tipoAtivo': 'I',
		'total': 1.1708619999999999
	},
	{
		'codigo': 'SCPF11',
		'direito': '2020-03-05T03:00:00.000Z',
		'pagamento': '2020-03-13T03:00:00.000Z',
		'valor': 0.05348,
		'nome': 'Rendimento',
		'saldo': 34,
		'pm': 13.32764705882353,
		'tipoAtivo': 'I',
		'total': 1.81832
	},
	{
		'codigo': 'SCPF11',
		'direito': '2020-04-06T03:00:00.000Z',
		'pagamento': '2020-04-15T03:00:00.000Z',
		'valor': 0.035829,
		'nome': 'Rendimento',
		'saldo': 109,
		'pm': 10.396238532110092,
		'tipoAtivo': 'I',
		'total': 3.905361
	},
	{
		'codigo': 'XPML11',
		'direito': '2020-01-17T02:00:00.000Z',
		'pagamento': '2020-01-24T02:00:00.000Z',
		'valor': 0.61,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 140.03,
		'tipoAtivo': 'I',
		'total': 0.61
	},
	{
		'codigo': 'XPML11',
		'direito': '2020-02-14T02:00:00.000Z',
		'pagamento': '2020-02-21T03:00:00.000Z',
		'valor': 0.5,
		'nome': 'Rendimento',
		'saldo': 2,
		'pm': 139.56,
		'tipoAtivo': 'I',
		'total': 1
	},
	{
		'codigo': 'QAGR11',
		'direito': '2020-02-07T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 0.159336359,
		'nome': 'Rendimento',
		'saldo': 2,
		'pm': 108.53,
		'tipoAtivo': 'I',
		'total': 0.318672718
	},
	{
		'codigo': 'HGFF11',
		'direito': '2020-01-31T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 0.6,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 114.03,
		'tipoAtivo': 'I',
		'total': 0.6
	},
	{
		'codigo': 'HGFF11',
		'direito': '2020-02-28T03:00:00.000Z',
		'pagamento': '2020-03-13T03:00:00.000Z',
		'valor': 0.6,
		'nome': 'Rendimento',
		'saldo': 3,
		'pm': 112.54,
		'tipoAtivo': 'I',
		'total': 1.7999999999999999
	},
	{
		'codigo': 'HGFF11',
		'direito': '2020-03-31T03:00:00.000Z',
		'pagamento': '2020-04-15T03:00:00.000Z',
		'valor': 0.55,
		'nome': 'Rendimento',
		'saldo': 3,
		'pm': 112.54,
		'tipoAtivo': 'I',
		'total': 1.6500000000000002
	},
	{
		'codigo': 'HGFF11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.55,
		'nome': 'Rendimento',
		'saldo': 12,
		'pm': 94.78583333333332,
		'tipoAtivo': 'I',
		'total': 6.6000000000000009
	},
	{
		'codigo': 'XPLG11',
		'direito': '2020-01-31T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 0.64,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 138.03,
		'tipoAtivo': 'I',
		'total': 0.64
	},
	{
		'codigo': 'XPLG11',
		'direito': '2020-02-28T03:00:00.000Z',
		'pagamento': '2020-03-13T03:00:00.000Z',
		'valor': 0.64,
		'nome': 'Rendimento',
		'saldo': 2,
		'pm': 136.41,
		'tipoAtivo': 'I',
		'total': 1.28
	},
	{
		'codigo': 'XPLG11',
		'direito': '2020-03-31T00:00:00.000Z',
		'pagamento': '2020-04-14T00:00:00.000Z',
		'valor': 0.58,
		'nome': 'Rendimento',
		'saldo': 2,
		'pm': 136.41,
		'tipoAtivo': 'I',
		'total': 1.16
	},
	{
		'codigo': 'XPLG11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.54,
		'nome': 'Rendimento',
		'saldo': 10,
		'pm': 110.91799999999998,
		'tipoAtivo': 'I',
		'total': 5.4
	},
	{
		'codigo': 'FVPQ11',
		'direito': '2020-01-31T02:00:00.000Z',
		'pagamento': '2020-02-10T02:00:00.000Z',
		'valor': 1.74,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 225.07,
		'tipoAtivo': 'I',
		'total': 1.74
	},
	{
		'codigo': 'FVPQ11',
		'direito': '2020-02-28T03:00:00.000Z',
		'pagamento': '2020-03-09T03:00:00.000Z',
		'valor': 0.87,
		'nome': 'Rendimento',
		'saldo': 4,
		'pm': 217.165,
		'tipoAtivo': 'I',
		'total': 3.48
	},
	{
		'codigo': 'MFII11',
		'direito': '2020-01-31T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 1,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 132.03,
		'tipoAtivo': 'I',
		'total': 1
	},
	{
		'codigo': 'MFII11',
		'direito': '2020-02-28T03:00:00.000Z',
		'pagamento': '2020-03-13T03:00:00.000Z',
		'valor': 1.02,
		'nome': 'Rendimento',
		'saldo': 3,
		'pm': 131.82999999999999,
		'tipoAtivo': 'I',
		'total': 3.06
	},
	{
		'codigo': 'MFII11',
		'direito': '2020-03-31T03:00:00.000Z',
		'pagamento': '2020-04-15T03:00:00.000Z',
		'valor': 1.03,
		'nome': 'Rendimento',
		'saldo': 3,
		'pm': 131.82999999999999,
		'tipoAtivo': 'I',
		'total': 3.09
	},
	{
		'codigo': 'MFII11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 1.03,
		'nome': 'Rendimento',
		'saldo': 10,
		'pm': 114.43299999999999,
		'tipoAtivo': 'I',
		'total': 10.3
	},
	{
		'codigo': 'XPCM11',
		'direito': '2020-02-28T03:00:00.000Z',
		'pagamento': '2020-03-13T03:00:00.000Z',
		'valor': 0.84,
		'nome': 'Rendimento',
		'saldo': 4,
		'pm': 61.285000000000007,
		'tipoAtivo': 'I',
		'total': 3.36
	},
	{
		'codigo': 'XPCM11',
		'direito': '2020-03-31T03:00:00.000Z',
		'pagamento': '2020-04-15T03:00:00.000Z',
		'valor': 0.85,
		'nome': 'Rendimento',
		'saldo': 9,
		'pm': 49.50777777777778,
		'tipoAtivo': 'I',
		'total': 7.6499999999999999
	},
	{
		'codigo': 'XPCM11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.85,
		'nome': 'Rendimento',
		'saldo': 15,
		'pm': 51.67133333333334,
		'tipoAtivo': 'I',
		'total': 12.75
	},
	{
		'codigo': 'FLMA11',
		'direito': '2020-02-06T02:00:00.000Z',
		'pagamento': '2020-02-14T02:00:00.000Z',
		'valor': 0.0129,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 0,
		'tipoAtivo': 'I',
		'total': 0.0129
	},
	{
		'codigo': 'VINO11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.35,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 51.04,
		'tipoAtivo': 'I',
		'total': 0.35
	},
	{
		'codigo': 'BCFF11',
		'direito': '2020-04-07T00:00:00.000Z',
		'pagamento': '2020-04-15T00:00:00.000Z',
		'valor': 0.4,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 77.06,
		'tipoAtivo': 'I',
		'total': 0.4
	},
	{
		'codigo': 'SPTW11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-11T03:00:00.000Z',
		'valor': 0.44,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 64.05,
		'tipoAtivo': 'I',
		'total': 0.44
	},
	{
		'codigo': 'SPTW11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-11T03:00:00.000Z',
		'valor': 0.56,
		'nome': 'Amortização',
		'saldo': 1,
		'pm': 64.05,
		'tipoAtivo': 'I',
		'total': 0.56
	},
	{
		'codigo': 'VILG11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.5,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 100.09,
		'tipoAtivo': 'I',
		'total': 0.5
	},
	{
		'codigo': 'SDIL11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.46,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 94.08,
		'tipoAtivo': 'I',
		'total': 0.46
	},
	{
		'codigo': 'HSML11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-08T03:00:00.000Z',
		'valor': 0.22,
		'nome': 'Rendimento',
		'saldo': 13,
		'pm': 79.86923076923077,
		'tipoAtivo': 'I',
		'total': 2.86
	},
	{
		'codigo': 'MALL11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.12,
		'nome': 'Rendimento',
		'saldo': 12,
		'pm': 80.02416666666666,
		'tipoAtivo': 'I',
		'total': 1.44
	},
	{
		'codigo': 'KNRI11',
		'direito': '2020-04-30T00:00:00.000Z',
		'pagamento': '2020-05-15T00:00:00.000Z',
		'valor': 0.65,
		'nome': 'Rendimento',
		'saldo': 1,
		'pm': 150.05,
		'tipoAtivo': 'I',
		'total': 0.65
	},
	{
		'codigo': 'VISC11',
		'direito': '2020-04-30T03:00:00.000Z',
		'pagamento': '2020-05-15T03:00:00.000Z',
		'valor': 0.27,
		'nome': 'Rendimento',
		'saldo': 10,
		'pm': 98.929,
		'tipoAtivo': 'I',
		'total': 2.7
	}
]";
	}
}
