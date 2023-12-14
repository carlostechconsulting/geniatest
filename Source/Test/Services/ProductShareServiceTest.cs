using GeniaWebApp;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Exceptions.Products;
using GeniaWebApp.Source.Main.Modules.Products.models;
using GeniaWebApp.Source.Main.Modules.Products.Services;
using NUnit.Framework;

namespace Tests.Services;

public class ProductShareServiceTest: ProductShareServiceTestScenarios
{
	private readonly ProductShareService _target = new();

	[Description("Teste de calculo de porcentagem de modais")]
	[TestCaseSource(nameof(CalculateSharesInProductMockParameters))]
	public void CalculateSharesWithValidation(Product product, bool hasError)
	{
		if (hasError)
		{
			Assert.That(
				() => _target.CalculateSharesByProduct(product),
				Throws.TypeOf<ProductSharesRecalculationException>());
		}
		else
		{
			var actual = _target.CalculateSharesByProduct(product);
			Assert.NotNull(actual);
		}
	}
	
	[Description("Test CalculateGlobal Share Percentages By Flow Type Parameters")]
	[TestCaseSource(nameof(CalculateGlobalSharePercentagesByFlowTypeParameters))]
	public void CalculateGlobalSharePercentagesByFlowType(Project projectInput, Dictionary<FlowTypes, TotalSharePercentages> Expected)
	{
		var actual = _target.CalculateGlobalSharePercentagesByFlowType(projectInput);
		Assert.NotNull(actual);

		if (projectInput is null)
		{
			Assert.IsEmpty(actual);
		}
		else
		{
			Assert.IsNotEmpty(actual);
			Assert.AreEqual(Expected.Count, actual.Count);
			foreach (var (ExpectedKey, ExpectedValue) in Expected)
			{
				Assert.AreEqual(ExpectedValue.Ferroviario, actual[ExpectedKey].Ferroviario);
				Assert.AreEqual(ExpectedValue.Hidroviario, actual[ExpectedKey].Hidroviario);
				Assert.AreEqual(ExpectedValue.Maritimo, actual[ExpectedKey].Maritimo);
				Assert.AreEqual(ExpectedValue.Rodoviario, actual[ExpectedKey].Rodoviario);
			}
		}
	}
}