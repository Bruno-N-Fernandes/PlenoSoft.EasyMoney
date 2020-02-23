using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Globalization;
using System.Linq;

namespace PlenoSoft.InvestFacil.Application.Services
{
	public static class RepositorioFactory
	{
		public static Repositorio GetRepositorio(string nomeInvestidor, string nomeFundo)
		{
			var repositorio = new Repositorio();
			var investidor = repositorio.ObterInvestidor(nomeInvestidor);
			var carteira = investidor.ObterCarteira(nomeFundo);

			Setup(carteira, repositorio);

			return repositorio;
		}

		public static void Setup(Carteira carteira, Repositorio repositorio)
		{
			var fundos = cFundos.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

			var id = 0;
			foreach (var fundo in fundos)
			{
				var campos = fundo.Split(new[] { "\t" }, StringSplitOptions.None);
				var ativo = repositorio.ObterAtivo(campos[2]);
				var data = DateTime.TryParseExact(campos[3], "dd/MM/yyyy", FinanceiroService.pt_BR, DateTimeStyles.AdjustToUniversal, out var d) ? d : DateTime.UtcNow;
				var taxas = decimal.TryParse(campos[8], NumberStyles.Any, FinanceiroService.pt_BR, out decimal t) ? t : decimal.Zero;
				var tipo = (campos[5] == "Compra") ? Lancamento.Enum.Compra : (campos[5] == "Venda" ? Lancamento.Enum.Venda : Lancamento.Enum.Rendimento);
				var quantidade = int.TryParse(campos[6] + campos[10], out var q) ? q : 0;
				var valor = decimal.TryParse(campos[7] + campos[9], NumberStyles.Any, FinanceiroService.pt_BR, out decimal v) ? v : decimal.Zero;
				var valorUnitario = tipo == Lancamento.Enum.Rendimento ? valor / quantidade : valor;

				var lancamento = new Lancamento() { Ativo = ativo, Data = data, Id = ++id, Quantidade = quantidade, Taxas = taxas, Tipo = tipo, ValorUnitario = valorUnitario };
				carteira.Movimentar(lancamento);
			}
		}

		private const string cFundos = @"
PrecoAtual	Patrimonio	Fundo	Data	Comp	Operação	Qtd	ValorNegociado	Taxas	Rend.	Por	Aplicação	Saldo	Aportes		Fundo	Preço Médio	Qtd	Aplicação	Valor Atual	Patrimonio	Resultado	
13,29	106,32	SCPF11	10/01/2020	jan./2020	Compra	8	13,65	0,03			109,23	890,78	1.000,01
137,00	137,00	XPML11	10/01/2020	jan./2020	Compra	1	140,00	0,03			140,03	750,75	
111,20	111,20	HGFF11	13/01/2020	jan./2020	Compra	1	114,00	0,03			114,03	636,72	
100,40	100,40	QAGR11	13/01/2020	jan./2020	Compra	1	112,00	0,03			112,03	524,69	
131,50	131,50	XPLG11	14/01/2020	jan./2020	Compra	1	138,00	0,03			138,03	386,66	
213,99	213,99	FVPQ11	16/01/2020	jan./2020	Compra	1	225,00	0,07			225,07	161,59	
130,00	130,00	MFII11	16/01/2020	jan./2020	Compra	1	132,00	0,03			132,03	29,56	
13,29	26,58	SCPF11	21/01/2020	jan./2020	Compra	2	13,40	0,00			26,80	2,76	
137,00	0,00	XPML11	23/01/2020	jan./2020	Rend				0,61	1	0,00	3,37	
213,99	213,99	FVPQ11	03/02/2020	fev./2020	Compra	1	217,40	0,07			217,47	785,89	999,99
59,96	119,92	XPCM11	03/02/2020	fev./2020	Compra	2	62,53	0,04			125,10	660,79	
100,40	100,40	QAGR11	03/02/2020	fev./2020	Compra	1	105,00	0,03			105,03	555,76	
131,50	131,50	XPLG11	03/02/2020	fev./2020	Compra	1	134,75	0,04			134,79	420,97	
137,00	137,00	XPML11	03/02/2020	fev./2020	Compra	1	139,05	0,04			139,09	281,88	
130,00	130,00	MFII11	03/02/2020	fev./2020	Compra	1	133,35	0,04			133,39	148,49	
13,29	13,29	SCPF11	03/02/2020	fev./2020	Compra	1	13,50	0,00			13,50	134,99	
111,20	111,20	HGFF11	05/02/2020	fev./2020	Compra	1	112,00	0,03			112,03	22,96	
13,29	13,29	SCPF11	10/02/2020	fev./2020	Compra	1	13,34	0,00			13,34	9,62	
213,99	0,00	FVPQ11	07/02/2020	fev./2020	Rend				1,74	1	0,00	11,36	
3,96	3,96	FLMA11	07/02/2020	fev./2020	Compra	1	3,98	0,00			3,98	7,38	
13,29	0,00	SCPF11	13/02/2020	fev./2020	Rend				1,17	11	0,00	8,55	
130,00	0,00	MFII11	13/02/2020	fev./2020	Rend				1,00	1	0,00	9,55	
100,40	0,00	QAGR11	13/02/2020	fev./2020	Rend				0,31	2	0,00	9,86	
111,20	0,00	HGFF11	13/02/2020	fev./2020	Rend				0,60	1	0,00	10,46	
131,50	0,00	XPLG11	13/02/2020	fev./2020	Rend				0,64	1	0,00	11,10	
3,96	0,00	FLMA11	13/02/2020	fev./2020	Rend				0,01	1	0,00	1.011,11	1.000,00
137,00	0,00	XPML11	14/02/2020	fev./2020	Rend				1,00	2	0,00	1.012,11	
213,99	427,98	FVPQ11	19/02/2020	fev./2020	Compra	2	213,00				426,00	586,11	
130,00	130,00	MFII11	19/02/2020	fev./2020	Compra	1	130,00				130,00	456,11	
13,29	146,19	SCPF11	19/02/2020	fev./2020	Compra	11	13,20				145,20	310,91	
111,20	111,20	HGFF11	19/02/2020	fev./2020	Compra	1	111,50				111,50	199,41	
137,00	137,00	XPML11	19/02/2020	fev./2020	Compra	1	136,00				136,00	63,41	
59,96	59,96	XPCM11	19/02/2020	fev./2020	Compra	1	59,70				59,70	3,71	
100,40	200,80	QAGR11	19/02/2020	fev./2020	Venda	-2	101,50				203,00	206,71	
";
	}
}