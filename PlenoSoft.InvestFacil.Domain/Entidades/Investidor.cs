using PlenoSoft.InvestFacil.Domain.Abstracao;
using System;
using System.Linq;

namespace PlenoSoft.InvestFacil.Domain.Entidades
{
	public class Investidor : Pessoa
	{
		public ActionList<Carteira> Carteiras { get; set; }
		public DateTime DataHoraInclusao { get; set; }
		public DateTime UltimaAlteracao { get; set; }

		public Investidor()
		{
			Carteiras = new ActionList<Carteira>(c => c.Investidor = this, c => c.Investidor = null);
		}

		public Carteira ObterCarteira(string nomeCarteira)
		{
			return Carteiras.SingleOrDefault(c => c.Nome.Contains(nomeCarteira)) ?? NovaCarteira(nomeCarteira);
		}

		public Carteira NovaCarteira(string nomeCarteira)
		{
			return Carteiras.Add(new Carteira { Nome = nomeCarteira });
		}
	}
}