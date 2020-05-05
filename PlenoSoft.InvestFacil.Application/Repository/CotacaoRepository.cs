using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class CotacaoRepository
	{
		public async Task<IEnumerable<Cotacao>> ObterTodos()
		{
			return await Conexao.Ativa.QueryAsync<Cotacao>(cSql_Select);
		}

		public async Task Incluir(Cotacao ativoFinanceiroCotacao, IDbTransaction transacao)
		{
			ativoFinanceiroCotacao.DataHoraInclusao = DateTime.Now;
			await PreSave(ativoFinanceiroCotacao);
			ativoFinanceiroCotacao.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, ativoFinanceiroCotacao, transacao);
		}

		public async Task Alterar(Cotacao ativoFinanceiroCotacao, IDbTransaction transacao)
		{
			await PreSave(ativoFinanceiroCotacao);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, ativoFinanceiroCotacao, transacao);
		}

		public async Task Excluir(Cotacao ativoFinanceiroCotacao, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, ativoFinanceiroCotacao, transacao);
		}

		private async Task PreSave(Cotacao ativoFinanceiroCotacao)
		{
			await Task.Run(() => ativoFinanceiroCotacao.UltimaAlteracao = DateTime.Now);
		}

		private const string cSql_Select = @"
Select Id, AtivoFinanceiroId, DataHora, Valor From Cotacao Where (Id = @id);";

		private const string cSql_Insert = @"
Insert Into Cotacao (AtivoFinanceiroId, DataHora, Valor)
Values (@ativoFinanceiroId, @dataHora, @valor);
Select Scope_Identity() As Id;";

		private const string cSql_Update = @"
Update Cotacao Set
	AtivoFinanceiroId = @ativoFinanceiroId,
	DataHora = @dataHora,
	Valor = @valor
Where (Id = @id);";

		private const string cSql_Delete = @"
Delete From Cotacao Where (Id = @id);";
	}
}
