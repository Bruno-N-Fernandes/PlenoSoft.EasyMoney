using System.Collections.Generic;
using System.Linq;
using static MPSC.PlenoSoftware.Financial.FII.Core.Util;

namespace MPSC.PlenoSoftware.Financial.FII.Core
{
	public class Fundo
	{
		private const string rootURL1 = "https://www.fundsexplorer.com.br/funds";
		private const string rootURL2 = "https://www.google.com/search?q";

		public string CódigoDoFundo { get; set; }
		public string Setor { get; set; }
		public decimal? PreçoAtual { get; set; }
		public decimal? LiquidezDiária { get; set; }
		public decimal? Dividendo { get; set; }
		public decimal? DY_Atual { get; set; }
		public decimal? DY_3M_Acumulado { get; set; }
		public decimal? DY_6M_Acumulado { get; set; }
		public decimal? DY_12M_Acumulado { get; set; }
		public decimal? DY_3M_Média { get; set; }
		public decimal? DY_6M_Média { get; set; }
		public decimal? DY_12M_Média { get; set; }
		public decimal? DY_Ano { get; set; }
		public decimal? VariaçãoPreço { get; set; }
		public decimal? Rentab_Período { get; set; }
		public decimal? Rentab_Acumulada { get; set; }
		public decimal? Patrimônio_Líq { get; set; }
		public decimal? VP { get; set; }
		public decimal? P_VP { get; set; }
		public decimal? DY_Patrimonial { get; set; }
		public decimal? VariaçãoPatrimonial { get; set; }
		public decimal? Rentab_Patr_no_Período { get; set; }
		public decimal? Rentab_Patr_Acumulada { get; set; }
		public decimal? VacânciaFísica { get; set; }
		public decimal? VacânciaFinanceira { get; set; }
		public int QuantidadeAtivos { get; set; }

		public static IEnumerable<Fundo> ObterTodos(string[] htmlRows)
		{
			return htmlRows.Select(tableRow => new Fundo(tableRow));
		}

		public Fundo(string tableRow)
		{
			var xmlNodes = GetPrimaryFields(tableRow);

			CódigoDoFundo = GetString(xmlNodes, 0, "data-index");
			Setor = GetString(xmlNodes, 1, "Value");
			PreçoAtual = GetDecimal(xmlNodes, 2);
			LiquidezDiária = GetDecimal(xmlNodes, 3);
			Dividendo = GetDecimal(xmlNodes, 4);
			DY_Atual = GetPercentual(xmlNodes, 5);
			DY_3M_Acumulado = GetPercentual(xmlNodes, 6);
			DY_6M_Acumulado = GetPercentual(xmlNodes, 7);
			DY_12M_Acumulado = GetPercentual(xmlNodes, 8);
			DY_3M_Média = GetPercentual(xmlNodes, 9);
			DY_6M_Média = GetPercentual(xmlNodes, 10);
			DY_12M_Média = GetPercentual(xmlNodes, 11);
			DY_Ano = GetPercentual(xmlNodes, 12);
			VariaçãoPreço = GetPercentual(xmlNodes, 13);
			Rentab_Período = GetPercentual(xmlNodes, 14);
			Rentab_Acumulada = GetPercentual(xmlNodes, 15);
			Patrimônio_Líq = GetDecimal(xmlNodes, 16);
			VP = GetDecimal(xmlNodes, 17);
			P_VP = GetDecimal(xmlNodes, 18);
			DY_Patrimonial = GetPercentual(xmlNodes, 19);
			VariaçãoPatrimonial = GetPercentual(xmlNodes, 20);
			Rentab_Patr_no_Período = GetPercentual(xmlNodes, 21);
			Rentab_Patr_Acumulada = GetPercentual(xmlNodes, 22);
			VacânciaFísica = GetPercentual(xmlNodes, 23);
			VacânciaFinanceira = GetPercentual(xmlNodes, 24);
			QuantidadeAtivos = GetInt32(xmlNodes, 25).GetValueOrDefault();
		}

		public string ExportToExcel(int i)
		{
			return
				$"=HIPERLINK('{rootURL1}/{CódigoDoFundo}'; '{CódigoDoFundo} F.Exp.')\t".Replace("'", "\"") +
				$"=HIPERLINK('{rootURL2}={CódigoDoFundo}'; '{CódigoDoFundo} Google')\t".Replace("'", "\"") +
				$"{QuantidadeAtivos}\t" +
				$"{LiquidezDiária}\t" +
				$"{Setor}\t" +
				$"{PreçoAtual}\t" +
				$"{Dividendo}\t" +
				$"{ToPercentual(DY_12M_Acumulado)}\t" +
				$"{ToPercentual(DY_6M_Acumulado)}\t" +
				$"{ToPercentual(DY_3M_Acumulado)}\t" +
				$"{ToPercentual(DY_Atual)}\t" +
				$"{ToPercentual(DY_3M_Média)}\t" +
				$"{ToPercentual(DY_6M_Média)}\t" +
				$"{ToPercentual(DY_12M_Média)}\t" +
				$"{ToPercentual(DY_Ano)}\t" +
				$"{ToPercentual(VariaçãoPreço)}\t" +
				$"{ToPercentual(Rentab_Período)}\t" +
				$"{ToPercentual(Rentab_Acumulada)}\t" +
				$"{Patrimônio_Líq}\t" +
				$"{VP}\t" +
				$"{P_VP}\t" +
				$"{ToPercentual(DY_Patrimonial)}\t" +
				$"{ToPercentual(VariaçãoPatrimonial)}\t" +
				$"{ToPercentual(Rentab_Patr_no_Período)}\t" +
				$"{ToPercentual(Rentab_Patr_Acumulada)}\t" +
				$"{ToPercentual(VacânciaFísica)}\t" +
				$"{ToPercentual(VacânciaFinanceira)}\t" +
				$"=E( H{i} > 0,06; K{i} >= L{i}; K{i} >= M{i}; K{i} >= N{i}; K{i} >= L{i}; L{i} >= M{i}; M{i} >= N{i} )\t" +
				"";
		}

		public static string GetHeader()
		{
			return
				"Fundo\t" +
				"Google\t" +
				"Ativos\t" +
				"Liquidez\t" +
				"Setor\t" +
				"Preço Atual\t" +
				"Dividendo\t" +
				"% 12M Ac\t" +
				"% 06M Ac\t" +
				"% 03M Ac\t" +
				"% Atual\t" +
				"% 03M Me\t" +
				"% 06M Me\t" +
				"% 12M Me\t" +
				"Rent_Ano\t" +
				"VariaPreço\t" +
				"Rent_Período\t" +
				"Rent_Acumulada\t" +
				"Patrimônio_Líq\t" +
				"VP\t" +
				"P / VP\t" +
				"Rent_Patrimonial\t" +
				"Variação Patrimonial\t" +
				"Rent_Patr_no_Período\t" +
				"Rent_Patr_Acumulada\t" +
				"Vacância Física\t" +
				"Vacância Financeira\t" +
				"Destaque\t" +
				"";
		}

		public static string ExportToExcel(IOrderedEnumerable<Fundo> fundos)
		{
			var linhas = fundos.Select((f, i) => f.ExportToExcel(i + 2));

			return GetHeader() + "\r\n" + string.Join("\r\n", linhas) + "\r\n";
		}
	}
}