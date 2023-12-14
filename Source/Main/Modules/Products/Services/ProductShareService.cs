// <copyright file="ProductShareService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Extentions;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Exceptions.Products;
using GeniaWebApp.Source.Main.Modules.Products.models;

namespace GeniaWebApp.Source.Main.Modules.Products.Services;

public class ProductShareService
{
	/// <summary>
	/// Recalculate shares of a product based on its models.
	/// </summary>
	/// <param name="product"></param>
	/// <returns></returns>
	public Product CalculateSharesByProduct(Product product)
	{
		if (!product.Modals.HasAny())
		{
			product.ReceptionShare = new decimal(0.0);
			product.ExpeditionShare = new decimal(0.0);
			return product;
		}

		var groupByFlowType = product.Modals
			.Where(modal => modal.FlowType.HasValue)
			.GroupBy(modal => modal.FlowType)
			.ToDictionary(c => c.Key, c => c.ToList());
		if (groupByFlowType.TryGetValue(FlowTypes.RECEPCAO, out _))
		{
			product.ReceptionShare = groupByFlowType[FlowTypes.RECEPCAO].Sum(modal => modal.Share);
		}

		if (groupByFlowType.TryGetValue(FlowTypes.EXPEDICAO, out _))
		{
			product.ExpeditionShare = groupByFlowType[FlowTypes.EXPEDICAO].Sum(modal => modal.Share);
		}

		return product;
	}

	public void ApplyValidations(Product product)
	{
		ValidatePercentagesByModalType(product.Modals);
		ValidatePercentagesByFlowType(product.Modals);
	}

	/// <summary>
	/// Validate Percentages By Modal Type
	/// </summary>
	/// <param name="Modals"></param>
	/// <exception cref="ProductSharesRecalculationException"></exception>
	public void ValidatePercentagesByModalType(ICollection<Modal> Modals)
	{
		Modals?.Where(modal => modal.Type.HasValue)
			.GroupBy(modal => modal.Type)
			.ToDictionary(grouping => grouping.Key, grouping => grouping.Sum(modal => modal.Share))
			.Where(keyValuePair => keyValuePair.Value > 100)
			.IfHasAny(enumerable =>
			{
				var parsed = enumerable
					.Select(entry => $"({entry.Key.ToString()}: {entry.Value}%)")
					.ToArray();

				throw new ProductSharesRecalculationException(
					$"A soma das porcentagens dos modais por Tipo e Fluxo não pode ser maior que 100%: {string.Join(",", parsed)}");
			});
	}

	/// <summary>
	/// TODO: future enhancement
	/// </summary>
	/// <param name="totalValue"></param>
	/// <param name="modalChangedValue"></param>
	/// <returns></returns>
	private decimal GetAvailableValue(decimal totalValue, decimal modalChangedValue)
	{
		return modalChangedValue + (100 - totalValue);
	}

	/// <summary>
	/// Validate Percentages By Flow Type
	/// </summary>
	/// <param name="Modals"></param>
	/// <exception cref="ProductSharesRecalculationException"></exception>
	private void ValidatePercentagesByFlowType(ICollection<Modal> Modals)
	{
		var groupByFlowType = Modals
			.Where(modal => modal.FlowType.HasValue)
			.GroupBy(modal => modal.FlowType)
			.ToDictionary(c => c.Key, c => c.Sum(modal => modal.Share))
			.Where(entry => entry.Value > 100);
		if (groupByFlowType.HasAny())
		{
			var parsed = groupByFlowType.Select(entry => $"({entry.Key.ToString()}: {entry.Value}%)")
				.ToArray();
			throw new ProductSharesRecalculationException(
				$"A soma das porcentagens dos modais por tipo não pode ser maior que 100%: {string.Join(",", parsed)}");
		}
	}

	/// <summary>
	/// Calculate Global Share Percentages By Flow Type
	/// </summary>
	/// <param name="Project"></param>
	/// <returns></returns>
	public Dictionary<FlowTypes, TotalSharePercentages> CalculateGlobalSharePercentagesByFlowType(Project Project)
	{
		if (Project is null)
			return new Dictionary<FlowTypes, TotalSharePercentages>();

		return Project.Products
			.Where(product => product.Modals.HasAny())
			.SelectMany(product => product.Modals)
			.Where(modal => modal.FlowType.HasValue)
			.GroupBy(modal => modal.FlowType)
			.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList())
			.Select(BuildTotalSharePercentagesByFlowTypes)
			.OrderByDescending(pair => pair.Key)
			.ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	/// <summary>
	/// Build Total Share Percentages By Flow Types
	/// </summary>
	/// <param name="entry"></param>
	/// <returns></returns>
	private KeyValuePair<FlowTypes, TotalSharePercentages> BuildTotalSharePercentagesByFlowTypes(
		KeyValuePair<FlowTypes?, List<Modal>> entry)
	{
		var shareSumByModalType = entry.Value
			.Where(modal => modal.Type.HasValue)
			.GroupBy(modal => modal.Type)
			.ToDictionary(e => e.Key,
				e => e.Select(modal => modal.Share).Sum());

		return new KeyValuePair<FlowTypes, TotalSharePercentages>(entry.Key.Value, new TotalSharePercentages()
		{
			Rodoviario = shareSumByModalType.GetValueOrDefault(ModalTypes.RODOVIARIO, new decimal(0.0)),
			Maritimo = shareSumByModalType.GetValueOrDefault(ModalTypes.MARITIMO, new decimal(0.0)),
			Ferroviario = shareSumByModalType.GetValueOrDefault(ModalTypes.FERROVIARIO, new decimal(0.0)),
			Hidroviario = shareSumByModalType.GetValueOrDefault(ModalTypes.HIDROVIARIO, new decimal(0.0)),
		});
	}
}