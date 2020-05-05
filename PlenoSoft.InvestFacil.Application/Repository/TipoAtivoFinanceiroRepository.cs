using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class TipoAtivoFinanceiroRepository
	{
		public async Task<IEnumerable<TipoAtivoFinanceiro>> ObterTodos()
		{
			return await Conexao.Ativa.QueryAsync<TipoAtivoFinanceiro>(cSql_Select);
		}

		public async Task Incluir(TipoAtivoFinanceiro tipoAtivoFinanceiro, IDbTransaction transacao)
		{
			tipoAtivoFinanceiro.DataHoraInclusao = DateTime.Now;
			await PreSave(tipoAtivoFinanceiro);
			tipoAtivoFinanceiro.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, tipoAtivoFinanceiro, transacao);
		}

		public async Task Alterar(TipoAtivoFinanceiro tipoAtivoFinanceiro, IDbTransaction transacao)
		{
			await PreSave(tipoAtivoFinanceiro);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, tipoAtivoFinanceiro, transacao);
		}

		public async Task Excluir(TipoAtivoFinanceiro tipoAtivoFinanceiro, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, tipoAtivoFinanceiro, transacao);
		}

		private async Task PreSave(TipoAtivoFinanceiro tipoAtivoFinanceiro)
		{
			await Task.Run(() => tipoAtivoFinanceiro.UltimaAlteracao = DateTime.Now);
		}

		private const string cSql_Select = @"
Select Id, Tipo, Descricao From TipoAtivoFinanceiro ";

		private const string cSql_Insert = @"
Insert Into TipoAtivoFinanceiro (Tipo, Descricao)
Values (@tipo, @descricao);
Select Scope_Identity() As Id;";

		private const string cSql_Update = @"
Update TipoAtivoFinanceiro Set
	Tipo = @tipo,
	Descricao = @descricao
Where (Id = @id);";

		private const string cSql_Delete = @"
Delete From TipoAtivoFinanceiro Where (Id = @id);";
	}
}
