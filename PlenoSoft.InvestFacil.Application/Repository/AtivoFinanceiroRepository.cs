using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class AtivoFinanceiroRepository
	{
		public async Task<IEnumerable<AtivoFinanceiro>> ObterTodos()
		{
			var tipos = await new TipoAtivoFinanceiroRepository().ObterTodos();
			var ativos= await Conexao.Ativa.QueryAsync<AtivoFinanceiro>(cSql_Select);
			foreach (var ativo in ativos)
			{
				var tipoAtivoFinanceiro = tipos.FirstOrDefault(t => t.Id == ativo.TipoAtivoFinanceiroId);
				ativo.TipoAtivoFinanceiro = tipoAtivoFinanceiro;
			}

			return ativos;
		}

		public async Task Incluir(AtivoFinanceiro ativoFinanceiro, IDbTransaction transacao)
		{
			ativoFinanceiro.DataHoraInclusao = DateTime.Now;
			await PreSave(ativoFinanceiro);
			ativoFinanceiro.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, ativoFinanceiro, transacao);
		}

		public async Task Alterar(AtivoFinanceiro ativoFinanceiro, IDbTransaction transacao)
		{
			await PreSave(ativoFinanceiro);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, ativoFinanceiro, transacao);
		}

		public async Task Excluir(AtivoFinanceiro ativoFinanceiro, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, ativoFinanceiro, transacao);
		}

		private async Task PreSave(AtivoFinanceiro ativoFinanceiro)
		{
			await Task.Run(() => ativoFinanceiro.UltimaAlteracao = DateTime.Now);
		}

		private const string cSql_Select = @"
Select Id, TipoAtivoFinanceiroId, Papel, Descricao, VPA From AtivoFinanceiro ";

		private const string cSql_Insert = @"
Insert Into AtivoFinanceiro (TipoAtivoFinanceiroId, Papel, Descricao, VPA)
Values (@tipoAtivoFinanceiroId, @papel, @descricao, @vpa);
Select Scope_Identity() As Id;";

		private const string cSql_Update = @"
Update AtivoFinanceiro Set
	TipoAtivoFinanceiroId = @tipoAtivoFinanceiroId,
	Papel = @papel,
	Descricao = @descricao,
	VPA = @vpa
Where (Id = @id);";

		private const string cSql_Delete = @"
Delete From AtivoFinanceiro Where (Id = @id);";
	}
}
