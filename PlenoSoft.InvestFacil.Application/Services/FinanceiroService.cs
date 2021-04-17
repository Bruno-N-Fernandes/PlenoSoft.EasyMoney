using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Services
{
	/// <summary>
	/// https://arquivos.b3.com.br/apinegocios/ticker/FVPQ11/2020-05-05
	/// http://cotacao.b3.com.br/mds/api/v1/DailyFluctuationHistory/FVPQ11
	/// https://fiis.com.br/knri/cotacoes/?periodo=12+months
	/// https://chart.fundsexplorer.com.br/scpf/cotacoes/?periodo=12+months
	/// https://www.fundsexplorer.com.br/funds/{papel}
	/// https://api.cotacoes.uol.com/asset/list?fields=id,abbreviation&size=10&letters=PETR4
	/// fields=id,name,abbreviation,type,companies,exchange
	/// https://api.cotacoes.uol.com/{Market}/{Type}?size=10&item=485&fields=abbreviation,price,date
	/// {Market} = asset, index, indicators, inflation, commodities, currency
	/// {Type} = intraday/list/{Period}, interday/list/{Period}, summary, list
	/// {Period} = {none}, paged, week, month, year
	/// fields=id,price,exchangeasset,high,low,open,volume,close,bid,ask,change,pctChange,date,abbreviation
	/// 
	/// https://api.cotacoes.uol.com/asset/interday/list/paged?size=10&item=485&fields=id,price,exchangeasset,high,low,open,volume,close,bid,ask,change,pctChange,date,abbreviation
	/// https://api.cotacoes.uol.com/asset/summary?size=10&item=485&fields=id,price,exchangeasset,high,low,open,volume,close,bid,ask,change,pctChange,date,abbreviation
	/// 
	/// http://cotacoes.economia.uol.com.br/ws/asset/484/intraday?size=400
	/// http://cotacoes.economia.uol.com.br/ws/asset/stock/list?size=10000
	/// https://economia.uol.com.br/cotacoes/bolsas/acoes/bvsp-bovespa/fvpq11-sa/
	/// https://api.cotacoes.uol.com/index/list?fields=id,name,abbreviation,type,companies,exchange
	/// https://api.cotacoes.uol.com/asset/list?fields=id,name,abbreviation,type,companies,exchange
	/// https://api.cotacoes.uol.com/commodities/list?fields=id,name,abbreviation,type,companies,exchange
	/// https://api.cotacoes.uol.com/inflation/list?fields=id,name,abbreviation,type,companies,exchange
	/// https://api.cotacoes.uol.com/indicators/list?fields=id,name,abbreviation,type,companies,exchange
	/// </summary>
	public class FinanceiroService
	{
		public static readonly IFormatProvider pt_BR = new CultureInfo("pt-BR");

		public static decimal? ObterValor(string ticker)
		{
			try
			{
				return ObterValorAsync(ticker).Result;
			}
			catch
			{
				return null;
			}
		}

		public async static Task<decimal?> ObterValorAsync(string papel)
		{
			try
			{
				var url = $@"https://www.fundsexplorer.com.br/funds/{papel}";
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(url);
				var html = await response.Content.ReadAsStringAsync();
				var pageDocument = new HtmlDocument();
				pageDocument.LoadHtml(html);

				var node = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@id,'stock-price')]//span)[1]");
				var valor = node.InnerText.Replace("R$", "").Trim();

				return decimal.TryParse(valor, NumberStyles.Any, pt_BR, out decimal result) ? result : (decimal?)null;
			}
			catch
			{
				return null;
			}
		}

		public static decimal? ObterCotacoes(string papel)
		{
			return ObterCotacoesDoDiaAsync(papel).Result;
		}

		public async static Task<decimal?> ObterCotacoesAsync(string papel)
		{
			try
			{
				var dia = DateTime.Today.ToString("yyyy-MM-dd");
				var url = $@"https://arquivos.b3.com.br/apinegocios/ticker/{papel}/{dia}";
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(url);
				var jsonString = await response.Content.ReadAsStringAsync();

				var cotacoes = JsonConvert.DeserializeObject<Cotacao>(jsonString);
				var cotacao = cotacoes.Values.OrderByDescending(c => c.DataHora).Select(c => c.Valor).FirstOrDefault();

				return cotacao;
			}
			catch
			{
				return null;
			}
		}

		public async static Task<decimal?> ObterCotacoesDoDiaAsync(string papel)
		{
			try
			{
				var url = $@"http://cotacao.b3.com.br/mds/api/v1/DailyFluctuationHistory/{papel}";
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(url);
				var jsonString = await response.Content.ReadAsStringAsync();

				var cotacoes = JsonConvert.DeserializeObject<CotacaoDia>(jsonString);
				var cotacao = cotacoes.tradgFlr.scty.lstQtn.OrderByDescending(c => c.dtTm).Select(c => c.closPric).FirstOrDefault();

				return cotacao;
			}
			catch
			{
				return null;
			}
		}
	}


	public class CotacaoDia
	{
		public Dictionary<string, object> BizSts { get; set; }
		public Dictionary<string, object> Msg { get; set; }
		public TradgFlr tradgFlr { get; set; }

		public class TradgFlr
		{
			public object TradgFlrSts { get; set; }
			public object Date { get; set; }
			public Scty scty { get; set; }
		}

		public class Scty
		{
			public LstQtn[] lstQtn { get; set; }
		}

		public class LstQtn
		{
			public decimal closPric { get; set; }
			public TimeSpan dtTm { get; set; }
			public double prcFlcn { get; set; }
		}
	}



	public class Cotacao
{
	private static readonly IFormatProvider pt_BR = new CultureInfo("pt-BR");
	public string Name { get; set; }
	public string FriendlyName { get; set; }
	public Column[] Columns { get; set; }
	public Value[] Values { get; set; }

	public class Column
	{
		public string Name { get; set; }
		public string FriendlyName { get; set; }
		public string FriendlyNamePt { get; set; }
		public string FriendlyNameEn { get; set; }
		public int Type { get; set; }
		public int ColumnAlignment { get; set; }
		public int ValueAlignment { get; set; }
	}

	public class Value : List<object>
	{
		public string Papel => (string)this[0];
		public Int64 Quantidade => (Int64)this[1];
		public decimal Valor => Convert.ToDecimal(this[2]);
		private Int64 TradeId => (Int64)this[3];
		private DateTime Data => DateTime.ParseExact((string)this[4], "yyyy-MM-dd", pt_BR);
		private TimeSpan Hora => TimeSpan.ParseExact((string)this[5], "g", pt_BR);
		public DateTime DataHora => Data.Add(Hora);
	}
}
}