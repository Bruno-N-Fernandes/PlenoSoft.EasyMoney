using Dapper;
using PlenoSoft.InvestFacil.Application.Abstracao;
using PlenoSoft.InvestFacil.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PlenoSoft.InvestFacil.Application.Repository
{
	public class MovimentoRepository
	{
		public async Task<IEnumerable<Movimento>> ObterPorCarteiraId(long carteiraId)
		{
			return await Conexao.Ativa.QueryAsync<Movimento>(cSql_Select_PorCarteiraId, new { carteiraId });
		}

		public async Task Incluir(IEnumerable<Movimento> ativosFinanceiroMovimento, IDbTransaction transacao)
		{
			foreach (var lancamento in ativosFinanceiroMovimento)
				await Incluir(lancamento, transacao);
		}

		public async Task Incluir(Movimento ativoFinanceiroMovimento, IDbTransaction transacao)
		{
			ativoFinanceiroMovimento.DataHoraInclusao = DateTime.Now;
			await PreSave(ativoFinanceiroMovimento);
			ativoFinanceiroMovimento.Id = await Conexao.Ativa.ExecuteScalarAsync<Int64>(cSql_Insert, ativoFinanceiroMovimento, transacao);
		}

		public async Task Alterar(Movimento ativoFinanceiroMovimento, IDbTransaction transacao)
		{
			await PreSave(ativoFinanceiroMovimento);
			await Conexao.Ativa.ExecuteAsync(cSql_Update, ativoFinanceiroMovimento, transacao);
		}

		public async Task Excluir(Movimento ativoFinanceiroMovimento, IDbTransaction transacao)
		{
			await Conexao.Ativa.ExecuteAsync(cSql_Delete, ativoFinanceiroMovimento, transacao);
		}

		private async Task PreSave(Movimento ativoFinanceiroMovimento)
		{
			await Task.Run(() => ativoFinanceiroMovimento.UltimaAlteracao = DateTime.Now);
		}

		private const string cSql_Select = @"
Select Id, CarteiraId, AtivoFinanceiroId, Observacao, Data, Tipo, Quantidade, PrecoUnitario, Taxas, ValorTotal From Movimento ";

		private const string cSql_Select_PorCarteiraId = cSql_Select + @" Where (CarteiraId = @carteiraId);";



		private const string cSql_Insert = @"
Insert Into Movimento (CarteiraId, AtivoFinanceiroId, Observacao, Data, Tipo, Quantidade, PrecoUnitario, Taxas, ValorTotal)
Values (@carteiraId, @ativoFinanceiroId, @observacao, @data, @tipo, @quantidade, @precoUnitario, @taxas, @valorTotal);
Select Scope_Identity() As Id;";

		private const string cSql_Update = @"
Update Movimento Set
	CarteiraId = @carteiraId,
	AtivoFinanceiroId = @ativoFinanceiroId,
	Observacao = @observacao,
	Data = @data,
	Tipo = @tipo,
	Quantidade = @quantidade,
	PrecoUnitario = @precoUnitario,
	Taxas = @taxas,
	ValorTotal = @valorTotal
Where (Id = @id);";

		private const string cSql_Delete = @"
Delete From Movimento Where (Id = @id);";
	}
}
