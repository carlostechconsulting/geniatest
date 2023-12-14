using GeniaWebApp.Extentions;
using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Exceptions.SeasonalityCalculation;
using GeniaWebApp.Source.Main.Modules.Shared.Utils;

namespace GeniaWebApp.Source.Main.Modules.Seasonalities.Services;

public class SeasonalityMonthDataService
{
	private static readonly bool CHANGEABLE = true;
	private static readonly bool NOT_CHANGEABLE = false;

	/// <summary>
	/// Reset to default values of seasonality month data.
	/// </summary>
	/// <param name="seasonalityMonthDatas"></param>
	/// <returns></returns>
	public ICollection<SeasonalityMonthData> ResetPercentagesToDefault(List<SeasonalityMonthData> seasonalityMonthDatas)
	{
		var newList = seasonalityMonthDatas.Select(current =>
		{
			current.Percentage = new decimal(8.33);
			return current;
		}).ToList();

		var sum = newList.Sum(i => i.Percentage);
		var remainder = 100 - sum;
		newList[0].Percentage += remainder;
		return newList;
	}

	/// <summary>
	/// TODO: understand better the parallel behavior.
	/// </summary>
	/// <param name="currentValues"></param>
	/// <param name="changedMonth"></param>
	/// <returns></returns>
	/// <exception cref="SeasonalityRecalculationException"></exception>
	public ICollection<SeasonalityMonthData> Recalculate(
		ICollection<SeasonalityMonthData> currentValues,
		SeasonalityMonthData changedMonth, decimal? ProductVolume = null)
	{
		currentValues = UpdateChangedMonth(currentValues, changedMonth);

		if (currentValues.Sum(i => i.Percentage) > 100)
			throw new SeasonalityRecalculationException(
				string.Empty,
				TransKeys.SEASONALITY_RECALCULATION_SUM_GREATER_THAN_100);

		var groupByChangeable = currentValues.GroupBy(month => month.Changeable)
			.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());


		var remainingMonthsToDistribute = groupByChangeable[CHANGEABLE]
			.Where(month => month.Id != changedMonth.Id);

		var remainingPercentageToRedistribute = 100 - (groupByChangeable[NOT_CHANGEABLE]
			.Sum(i => i.Percentage) + changedMonth.Percentage);

		var remainingPercentagePerMonth = remainingPercentageToRedistribute / remainingMonthsToDistribute.Count();
		foreach (var month in remainingMonthsToDistribute)
		{
			month.Percentage = remainingPercentagePerMonth.Value;
		}

		return RecalculateValuesBasedOnCurrentPercentages(currentValues, ProductVolume);
	}

	private ICollection<SeasonalityMonthData> UpdateChangedMonth(ICollection<SeasonalityMonthData> currentValues,
		SeasonalityMonthData changedMonth)
	{
		var seasonalityMonthDatas = currentValues.Where(month => month.Id == changedMonth.Id);

		foreach (var seasonalityMonthData in seasonalityMonthDatas)
		{
			seasonalityMonthData.Percentage = changedMonth.Percentage;
		}

		return currentValues;
	}

	/// <summary>
	/// Recalculate Seasonality Values
	/// </summary>
	/// <param name="currentValues"></param>
	/// <param name="productVolume"></param>
	/// <returns></returns>
	public ICollection<SeasonalityMonthData> RecalculateValuesBasedOnCurrentPercentages(
		ICollection<SeasonalityMonthData> currentValues, decimal? productVolume)
	{
		if (!productVolume.HasValue)
			return currentValues;

		return currentValues.Select(item =>
		{
			var newValue = productVolume * (item.Percentage / 100);
			item.Value = DecimalUtils.Round(newValue);
			return item;
		}).ToList();
	}
}