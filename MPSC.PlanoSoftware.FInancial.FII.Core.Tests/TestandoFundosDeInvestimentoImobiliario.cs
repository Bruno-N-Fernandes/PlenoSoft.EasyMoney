using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSC.PlenoSoftware.Financial.FII.Core;

namespace MPSC.PlanoSoftware.FInancial.FII.Core.Tests
{
	[TestClass]
	public class TestandoFundosDeInvestimentoImobiliario
	{
		[TestMethod]
		public void QuandoSolicitaExportacaoParaExcel_DeveGerarUmaStringComTodasAsInformacoes()
		{
			var service = new Service();
			var excel = service.GetExcelOfFII();
			Assert.IsNotNull(excel);
		}
	}
}