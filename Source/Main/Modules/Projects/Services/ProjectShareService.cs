using System.Linq.Dynamic.Core;
using GeniaWebApp.Extentions;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Exceptions.Products;
using GeniaWebApp.Source.Main.Modules.Products.models;
using GeniaWebApp.Source.Main.Modules.Products.Services;

namespace GeniaWebApp.Source.Main.Modules.Projects.Services;

public class ProjectShareService
{
	private static readonly Dictionary<FlowTypes, List<ModalTypes>> FLOW_TYPE_MODAL_TYPE_MAP =
		new()
		{
			{
				FlowTypes.EXPEDICAO, Enum.GetValues(typeof(ModalTypes))
					.ToDynamicList<ModalTypes>().ToList()
			},
			{
				FlowTypes.RECEPCAO, Enum.GetValues(typeof(ModalTypes))
					.ToDynamicList<ModalTypes>().ToList()
			}
		};

	ProductShareService productShareService;

	/// <summary>
	/// Calculate Global Share Percentages By Flow Type
	/// </summary>
	/// <param name="project"></param>
	/// <returns></returns>
	public Dictionary<FlowTypes, TotalSharePercentages> CalculateGlobalSharePercentagesByFlowType(Project project)
	{
		if (project is null)
			return new Dictionary<FlowTypes, TotalSharePercentages>();

		var groupByFlowAndType = GetGroupByFlowThenType(project);
		ValidateGlobalShareValues(groupByFlowAndType);
		
		return BuildTotalSharePercentagesByFlowTypes(groupByFlowAndType);
	}

	/// <summary>
	/// Validate percentages
	/// </summary>
	/// <param name="groupByFlowAndType"></param>
	/// <exception cref="ProductSharesRecalculationException"></exception>
	private static void ValidateGlobalShareValues(
		Dictionary<FlowTypes?, Dictionary<ModalTypes?, List<Modal>>> groupByFlowAndType)
	{
		foreach (var pair in FLOW_TYPE_MODAL_TYPE_MAP)
		{
			var flowType = pair.Key;
			var modalTypes = pair.Value;

			foreach (var modalType in modalTypes)
			{
				var shareSum = groupByFlowAndType.GetAndMap(flowType,
					dictionary => dictionary.GetAndMap(modalType, modals => modals.Sum(i => i.Share)));

				if (shareSum > 100)
					throw new ProductSharesRecalculationException(
						$"A soma das porcentagens dos modais por tipo não pode ser maior que 100%: {flowType} - {modalType} - {shareSum}%");
			}
		}
	}

	/// <summary>
	/// Group By Flow Then Type
	/// </summary>
	/// <param name="project"></param>
	/// <returns></returns>
	private Dictionary<FlowTypes?, Dictionary<ModalTypes?, List<Modal>>> GetGroupByFlowThenType(Project project)
	{
		return project.Products.OrElseEmpty()
			.Where(product => product.Modals.HasAny())
			.SelectMany(product => product.Modals)
			.Where(modal => modal.FlowType.HasValue)
			.GroupBy(modal => modal.FlowType)
			.ToDictionary(grouping => grouping.Key, grouping =>
			{
				return grouping.Where(modal => modal.Type.HasValue)
					.GroupBy(modal => modal.Type)
					.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			});
	}

	/// <summary>
	/// Build Total Share Percentages By Flow Types
	/// </summary>
	/// <param name="entry"></param>
	/// <returns></returns>
	private Dictionary<FlowTypes, TotalSharePercentages> BuildTotalSharePercentagesByFlowTypes(
		Dictionary<FlowTypes?, Dictionary<ModalTypes?, List<Modal>>> groups)
	{
		return groups.Select(keyValuePair => new KeyValuePair<FlowTypes, TotalSharePercentages>(
			keyValuePair.Key.Value,
			new TotalSharePercentages()
			{
				Rodoviario = keyValuePair.Value
					.GetAndMap(ModalTypes.RODOVIARIO, modals => modals.Sum(i => i.Share)),
				Maritimo = keyValuePair.Value
					.GetAndMap(ModalTypes.MARITIMO, modals => modals.Sum(i => i.Share)),
				Ferroviario = keyValuePair.Value
					.GetAndMap(ModalTypes.FERROVIARIO, modals => modals.Sum(i => i.Share)),
				Hidroviario = keyValuePair.Value
					.GetAndMap(ModalTypes.HIDROVIARIO, modals => modals.Sum(i => i.Share)),
			})).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
	}
}