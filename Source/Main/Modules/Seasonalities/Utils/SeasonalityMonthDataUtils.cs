// <copyright file="SeasonalityMonthDataUtils.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Linq.Dynamic.Core;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;

namespace GeniaWebApp.Source.Main.Modules.Seasonalities.Utils;

/// <summary>
/// SeasonalityMonthDataUtils.
/// </summary>
public class SeasonalityMonthDataUtils
{
	private static readonly Dictionary<MonthNames, int> MonthNameList = Enum.GetValues(typeof(MonthNames))
		.ToDynamicList<MonthNames>()
		.ToDictionary(i => i, i => (int)i);

	public static int GetMonthNumber(MonthNames monthName)
	{
		return MonthNameList[monthName];
	}

	public static Seasonality GetDefaultSeasonality()
	{
		return new Seasonality()
		{
			Year = DateTime.Now.Year,
			SeasonalitiesMonthData = GetDefaultList(),
		};
	}
	private static ICollection<SeasonalityMonthData> GetDefaultList()
	{
		var seasonalityMonthDatas = MonthNameList.Keys.Select(monthName => new SeasonalityMonthData()
		{
			MonthName = monthName,
			Percentage = new decimal(8.33),
		}).ToList();
		var sum = seasonalityMonthDatas.Sum(i => i.Percentage);
		var remainder = 100 - sum;
		seasonalityMonthDatas[0].Percentage += remainder;
		return seasonalityMonthDatas;
	}
}