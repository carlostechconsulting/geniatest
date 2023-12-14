using GeniaWebApp.Extentions;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Modules.Projects.Services;
using NUnit.Framework;
using Tests.Services.Models;

namespace Tests.Services;

public class ProjectShareServiceTest
{
	private readonly ProjectShareService target = new();

	[Description("Teste Recalculate Seasonality")]
	[TestCaseSource(nameof(GetGroupByFlowThenTypeMockParameters))]
	public void GetGroupByFlowThenType(
		Project project,
		ModalShareSumByGroups modalShareSumByGroups)
	{
		try
		{
			var groupByFlowThenType = target.CalculateGlobalSharePercentagesByFlowType(project);
			Assert.NotNull(groupByFlowThenType);
			Assert.IsFalse(modalShareSumByGroups.hasError);

			Assert.AreEqual(
				modalShareSumByGroups.ExpedicaoRodoviario,
				groupByFlowThenType.GetAndMap(FlowTypes.EXPEDICAO,
					total => total.Rodoviario)
			);

			Assert.AreEqual(
				modalShareSumByGroups.ExpedicaoFerroviario,
				groupByFlowThenType.GetAndMap(FlowTypes.EXPEDICAO,
					total => total.Ferroviario)
			);
			Assert.AreEqual(
				modalShareSumByGroups.ExpedicaoHidroviario,
				groupByFlowThenType.GetAndMap(FlowTypes.EXPEDICAO,
					total => total.Hidroviario)
			);
			Assert.AreEqual(
				modalShareSumByGroups.ExpedicaoMaritimo,
				groupByFlowThenType.GetAndMap(FlowTypes.EXPEDICAO,
					total => total.Maritimo)
			);

			Assert.AreEqual(
				modalShareSumByGroups.RececaoRodoviario,
				groupByFlowThenType.GetAndMap(FlowTypes.RECEPCAO,
					total => total.Rodoviario)
			);
			Assert.AreEqual(
				modalShareSumByGroups.RececaoFerroviario,
				groupByFlowThenType.GetAndMap(FlowTypes.RECEPCAO,
					total => total.Ferroviario)
			);
			Assert.AreEqual(
				modalShareSumByGroups.RececaoHidroviario,
				groupByFlowThenType.GetAndMap(FlowTypes.RECEPCAO,
					total => total.Hidroviario)
			);
			Assert.AreEqual(
				modalShareSumByGroups.RececaoMaritimo,
				groupByFlowThenType.GetAndMap(FlowTypes.RECEPCAO,
					total => total.Maritimo)
			);
		}
		catch (Exception e)
		{
			Assert.IsTrue(modalShareSumByGroups.hasError);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private static object[] GetGroupByFlowThenTypeMockParameters =
	{
		new object[]
		{
			new Project()
			{
				Products = new List<Product>()
				{
					// Product 1
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 50, Type = ModalTypes.FERROVIARIO },
							new()
							{
								FlowType = FlowTypes.EXPEDICAO, Share = 49.99m, Type = ModalTypes.FERROVIARIO
							},
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 16, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 89, Type = ModalTypes.MARITIMO },
						},
					},
					// Product 2
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 10.5m, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 10, Type = ModalTypes.MARITIMO },
						},
					},
					// Product 3
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 1, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 1, Type = ModalTypes.MARITIMO },
						},
					}
				}
			},
			new ModalShareSumByGroups(
				ExpedicaoRodoviario: 0,
				ExpedicaoFerroviario: 99.99m,
				ExpedicaoHidroviario: 0,
				ExpedicaoMaritimo: 27.5m,
				RececaoRodoviario: 80,
				RececaoFerroviario: 0,
				RececaoHidroviario: 0,
				RececaoMaritimo: 100
			)
		},
		// entry with error
		new object[]
		{
			new Project()
			{
				Products = new List<Product>()
				{
					// Product 1
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 50, Type = ModalTypes.FERROVIARIO },
							new()
							{
								FlowType = FlowTypes.EXPEDICAO, Share = 49.99m, Type = ModalTypes.FERROVIARIO
							},
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 16, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 100, Type = ModalTypes.MARITIMO },
						},
					},
					// Product 2
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 10.5m, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 10, Type = ModalTypes.MARITIMO },
						},
					},
					// Product 3
					new()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 1, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 1, Type = ModalTypes.MARITIMO },
						},
					}
				}
			},
			new ModalShareSumByGroups(
				ExpedicaoRodoviario: 0,
				ExpedicaoFerroviario: 99.99m,
				ExpedicaoHidroviario: 0,
				ExpedicaoMaritimo: 27.5m,
				RececaoRodoviario: 80,
				RececaoFerroviario: 0,
				RececaoHidroviario: 0,
				RececaoMaritimo: 111,
				hasError: true
			)
		},
	};
}