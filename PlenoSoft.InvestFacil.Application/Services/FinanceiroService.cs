using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Services
{
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
				var url = "https://www.fundsexplorer.com.br/funds/" + papel;
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

		public async static Task<decimal?> ObterCotacoesAsync(string papel)
		{
			try
			{
				return await Task.Run(() => (decimal?)null);
				//https://fiis.com.br/knri/cotacoes/?periodo=12+months
				//https://chart.fundsexplorer.com.br/scpf/cotacoes/?periodo=12+months
			}
			catch
			{
				return await Task.Run(() => (decimal?)null);
			}
		}
	}
}