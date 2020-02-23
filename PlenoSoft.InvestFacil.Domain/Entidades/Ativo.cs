using PlenoSoft.InvestFacil.Domain.Abstracao;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Ativo : Entidade
	{
		public string Ticker { get; private set; }
		public decimal? ValorAtual { get; private set; }

		public Ativo(string ticker, Task<decimal?> valorAtualAsync)
		{
			Ticker = ticker;
			valorAtualAsync.ContinueWith(valorAtual => ValorAtual = valorAtual.Result);
		}
	}
}