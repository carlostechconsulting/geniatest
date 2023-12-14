using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Modules.Products.models;

namespace Tests.Services;

public class ProductShareServiceTestScenarios
{
	/// <summary>
	/// TODO: share n pode ser <= 0
	/// </summary>
	protected static object[] CalculateSharesInProductMockParameters =
	{
		// entry OK
		new object[]
		{
			new Product()
			{
				Modals = new List<Modal>()
				{
					new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.RODOVIARIO },
					new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.RODOVIARIO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 50, Type = ModalTypes.FERROVIARIO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = new decimal(49.99), Type = ModalTypes.FERROVIARIO },
				},
			},
			false
		},
		// Entry with error by flow type
		new object[]
		{
			new Product()
			{
				Modals = new List<Modal>()
				{
					new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.RODOVIARIO },
					new() { FlowType = FlowTypes.RECEPCAO, Share = 40, Type = ModalTypes.RODOVIARIO },

					new() { FlowType = FlowTypes.EXPEDICAO, Share = 20, Type = ModalTypes.FERROVIARIO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 40, Type = ModalTypes.FERROVIARIO },

					new() { FlowType = FlowTypes.RECEPCAO, Share = 20, Type = ModalTypes.MARITIMO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 42, Type = ModalTypes.MARITIMO },
				},
			},
			true
		},
		// Entry with error by modal type
		new object[]
		{
			new Product()
			{
				Modals = new List<Modal>()
				{
					new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.RODOVIARIO },
					new() { FlowType = FlowTypes.RECEPCAO, Share = 70, Type = ModalTypes.RODOVIARIO },

					new() { FlowType = FlowTypes.EXPEDICAO, Share = 50, Type = ModalTypes.FERROVIARIO },
					new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.FERROVIARIO },

					new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.MARITIMO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 30, Type = ModalTypes.MARITIMO },
					new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.MARITIMO },

					new() { FlowType = FlowTypes.EXPEDICAO, Share = 30, Type = ModalTypes.HIDROVIARIO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 30, Type = ModalTypes.HIDROVIARIO },
					new() { FlowType = FlowTypes.EXPEDICAO, Share = 44, Type = ModalTypes.HIDROVIARIO },
				},
			},
			true
		},
	};

	/// <summary>
	/// Calculate Global Share Percentages By Flow Type Parameters
	/// </summary>
	protected static object[] CalculateGlobalSharePercentagesByFlowTypeParameters =
	{
		// Project is null
		new object[]
		{
			null,
			new Dictionary<FlowTypes, TotalSharePercentages>()
		},
		// Project not null should calculate properly
		new object[]
		{
			new Project()
			{
				Products = new List<Product>()
				{
					new ()
					{
						Modals = new List<Modal>()
						{
							new() { FlowType = FlowTypes.RECEPCAO, Share = 50, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 30, Type = ModalTypes.RODOVIARIO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 32, Type = ModalTypes.MARITIMO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 12.22m, Type = ModalTypes.FERROVIARIO },
							new() { FlowType = FlowTypes.RECEPCAO, Share = 77, Type = ModalTypes.FERROVIARIO },
							
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 50.5m, Type = ModalTypes.FERROVIARIO },
							new() { FlowType = FlowTypes.EXPEDICAO, Share = 25.3m, Type = ModalTypes.FERROVIARIO },
						},
					}
				}
			},
			new Dictionary<FlowTypes, TotalSharePercentages>()
			{
				{
					FlowTypes.RECEPCAO, new TotalSharePercentages()
					{
						Ferroviario = 89.22m,
						Hidroviario = 0,
						Maritimo = 32,
						Rodoviario = 80,
					}
				},
				{
					FlowTypes.EXPEDICAO, new TotalSharePercentages()
					{
						Ferroviario = 75.8m,
						Hidroviario = 0,
						Maritimo = 0,
						Rodoviario = 0,
					}
				},

			}
		},
	};
}