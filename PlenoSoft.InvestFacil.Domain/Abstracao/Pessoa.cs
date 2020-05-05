namespace PlenoSoft.InvestFacil.Domain.Abstracao
{
	public abstract class Pessoa : Entidade
	{
		public string Nome { get; set; }
		public string EMail { get; set; }
	}
}