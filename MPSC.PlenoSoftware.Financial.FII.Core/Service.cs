using System;
using System.Linq;

namespace MPSC.PlenoSoftware.Financial.FII.Core
{
	public class Service
	{
		public string GetExcelOfFII()
		{
			var fullhtml = Util.GetHtml("https://www.fundsexplorer.com.br/ranking");
			var htmlRows = GetHtmlRows(fullhtml);
			var fundos = Fundo.ObterTodos(htmlRows);
			var fundosOrdenados = fundos.OrderBy(f => f.CódigoDoFundo);
			var excel = Fundo.ExportToExcel(fundosOrdenados);
			return excel;
		}

		private string[] GetHtmlRows(string fullhtml)
		{
			var posicao = fullhtml.IndexOf(@"id=""table-ranking""");
			var table = fullhtml.Substring(posicao - 7);
			var tbody = table.Substring(0, table.IndexOf("</tbody>")).Substring(table.IndexOf("<tbody>") + 7);

			while (tbody.Contains("  "))
				tbody = tbody.Replace("  ", " ");

			while (tbody.Contains(" <"))
				tbody = tbody.Replace(" <", "<");

			while (tbody.Contains("\n\n"))
				tbody = tbody.Replace("\n\n", "\n");

			tbody = tbody.Replace("</tr>\n", "").Replace("\n<tr>\n", "<->");

			return tbody.Split("<->", StringSplitOptions.RemoveEmptyEntries);
		}
	}
}