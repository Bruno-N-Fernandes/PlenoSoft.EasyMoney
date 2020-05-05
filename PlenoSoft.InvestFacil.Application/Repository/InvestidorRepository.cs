using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class InvestidorRepository
	{
		public async Task<Investidor> ObterPor(string login, string senha)
		{
			return await Conexao.Ativa.QueryFirstOrDefaultAsync<Investidor>(cSql_Select_CPF + " Union " + cSql_Select_EMail, new { eMail = login, cpf = ToCPF(login), senha });
		}

		private Int64 ToCPF(string login)
		{
			var cpf = Regex.Replace(login, "[^0-9]", string.Empty);
			return cpf.Any(Char.IsDigit) && cpf.All(Char.IsDigit) ? Convert.ToInt64(cpf) : 0L;
		}

		public async Task Incluir(Investidor investidor, IDbTransaction transacao)
		{
			investidor.DataHoraInclusao = DateTime.Now;
			await PreSave(investidor);
			investidor.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, investidor, transacao);
		}

		public async Task Alterar(Investidor investidor, IDbTransaction transacao)
		{
			await PreSave(investidor);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, investidor, transacao);
		}

		public async Task Excluir(Investidor investidor, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, investidor, transacao);
		}

		private async Task PreSave(Investidor investidor)
		{
			await Task.Run(() => investidor.UltimaAlteracao = DateTime.Now);
		}

		private const string cSql_Select = @"Select Id, Nome, CPF, EMail, Senha From Investidor I ";

		private const string cSql_Select_EMail = cSql_Select + @" Where (I.EMail = @eMail) And (I.Senha = @senha)";

		private const string cSql_Select_CPF = cSql_Select + @" Where (I.CPF = @cpf) And (I.Senha = @senha)";


		private const string cSql_Insert = @"
Insert Into Investidor (Nome, CPF, EMail, Senha)
Values (@nome, @cpf, @eMail, @senha);
Select Scope_Identity() As Id;";

		private const string cSql_Update = @"
Update Investidor Set
	Nome = @nome,
	CPF = @cpf,
	EMail = @eMail,
	Senha = @senha
Where (Id = @id);";

		private const string cSql_Delete = @"
Delete From Investidor Where (Id = @id);";
	}
}
