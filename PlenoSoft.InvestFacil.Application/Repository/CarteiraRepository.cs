using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class CarteiraRepository
	{
		public async Task<IEnumerable<Carteira>> ObterPor(long investidorId)
		{
			return await Conexao.Ativa.QueryAsync<Carteira>(cSql_Select_PorInvestidorId, new { investidorId });
		}

		public async Task<Carteira> ObterPorId(long id)
		{
			return await Conexao.Ativa.QueryFirstOrDefaultAsync<Carteira>(cSql_Select_PorId, new { id });
		}

		public async Task Incluir(Carteira carteira, IDbTransaction transacao)
		{
			carteira.DataHoraInclusao = DateTime.Now;
			await PreSave(carteira);
			carteira.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, carteira, transacao);
		}

		public async Task Alterar(Carteira carteira, IDbTransaction transacao)
		{
			await PreSave(carteira);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, carteira, transacao);
		}

		public async Task Excluir(Carteira carteira, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, carteira, transacao);
		}

		private async Task PreSave(Carteira carteira)
		{
			await Task.Run(() => carteira.UltimaAlteracao = DateTime.Now);
		}

		private const String cSql_Select = @"Select Id, InvestidorId, Nome From Carteira ";

		private const String cSql_Select_PorInvestidorId = cSql_Select + @" Where (InvestidorId = @investidorId);";

		private const String cSql_Select_PorId = cSql_Select + @" Where (Id = @id);";


		private const String cSql_Insert = @"
Insert Into Carteira (InvestidorId, Nome)
Values (@investidorId, @nome);
Select Scope_Identity() As Id;";

		private const String cSql_Update = @"
Update Carteira Set
	InvestidorId = @investidorId,
	Nome = @nome
Where (Id = @id);";

		private const String cSql_Delete = @"
Delete From Carteira Where (Id = @id);";

	}
}
