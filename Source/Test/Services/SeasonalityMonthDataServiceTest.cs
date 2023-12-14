using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Exceptions.Products;
using GeniaWebApp.Source.Main.Exceptions.SeasonalityCalculation;
using GeniaWebApp.Source.Main.Modules.Seasonalities.Services;
using NUnit.Framework;

namespace Tests.Services;

public class SeasonalityMonthDataServiceTest
{
	private readonly SeasonalityMonthDataService _target = new();

	[Description("Teste Recalculate Seasonality")]
	[TestCaseSource(nameof(RecalculateSeasonalityMockParameters))]
	public void RecalculateSeasonality(
		List<SeasonalityMonthData> currentSeasonalityMonthDataList,
		SeasonalityMonthData changedMonth,
		decimal? ProductVolume,
		List<SeasonalityMonthData> expectedList,
		bool hasError)
	{

		if (hasError)
		{
			Assert.That(
				() => _target.Recalculate(currentSeasonalityMonthDataList, changedMonth, ProductVolume),
				Throws.TypeOf<SeasonalityRecalculationException>());
		}
		else
		{
			var actual = _target.Recalculate(currentSeasonalityMonthDataList, changedMonth, ProductVolume);
			Assert.NotNull(actual);
			Assert.AreEqual(expectedList.Count(), actual.Count());
			Assert.AreEqual(actual.Sum(i => i.Percentage), 100m);
			for (int i = 0; i < expectedList.Count(); i++)
			{
				Assert.AreEqual(
					expectedList.ElementAt(i).Percentage,
					actual.ElementAt(i).Percentage);
				Assert.AreEqual(
					expectedList.ElementAt(i).Value,
					actual.ElementAt(i).Value);
				Assert.IsFalse(actual.ElementAt(i).Changeable);
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private static object[] RecalculateSeasonalityMockParameters =
	{
		// ERROR: not enough changeable months
		// new object[]
		// {
		// 	new List<SeasonalityMonthData>()
		// 	{
		// 		new()
		// 		{
		// 			Id = 1,
		// 			MonthName = MonthNames.JANUARY,
		// 			Percentage = 10,
		// 			Changeable = true
		// 		},
		// 		new()
		// 		{
		// 			Id = 2,
		// 			MonthName = MonthNames.FEBRUARY,
		// 			Percentage = 10,
		// 			Changeable = false
		// 		},
		// 	},
		// 	new SeasonalityMonthData()
		// 	{
		// 		Id = 1,
		// 		MonthName = MonthNames.JANUARY,
		// 		Percentage = 20,
		// 		Changeable = true
		// 	},
		// 	1000m,
		// 	true
		// },
		// SUCCESS: recalculate seasonality
			// 8,91+++
		new object[]
		{
			// currentSeasonalityMonthDataList
			new List<SeasonalityMonthData>()
			{
				new()
				{
					Id = 1,
					MonthName = MonthNames.JANUARY,
					Percentage = 2m,
					Changeable = true
				},
				new()
				{
					Id = 2,
					MonthName = MonthNames.FEBRUARY,
					Percentage = 8.91m,
					Changeable = true
				},
				// will be changed to 1
				new()
				{
					Id = 3,
					MonthName = MonthNames.MARCH,
					Percentage = 8.91m,
					Changeable = true
				},
				new()
				{
					Id = 4,
					MonthName = MonthNames.APRIL,
					Percentage = 8.91m,
					Changeable = true
				},
				new()
				{
					Id = 5,
					MonthName = MonthNames.MAY,
					Percentage = 3.91m,
					Changeable = false
				},
				new()
				{
					Id = 6,
					MonthName = MonthNames.JUNE,
					Percentage = 11.41m,
					Changeable = false
				},
				new()
				{
					Id = 7,
					MonthName = MonthNames.JULY,
					Percentage = 11.41m,
					Changeable = false
				},
				new()
				{
					Id = 8,
					MonthName = MonthNames.AUGUST,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 9,
					MonthName = MonthNames.SEPTEMBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 10,
					MonthName = MonthNames.OCTOBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 11,
					MonthName = MonthNames.NOVEMBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 12,
					MonthName = MonthNames.DECEMBER,
					Percentage = 8.90m,
					Changeable = false
				},
			},
			// changedMonth
			new SeasonalityMonthData()
			{
				Id = 3,
				MonthName = MonthNames.MARCH,
				Percentage = 1
			},
			// ProductVolume
			1000m,
			// ExpectedList
			new List<SeasonalityMonthData>()
			{
				new()
				{
					Id = 1,
					MonthName = MonthNames.JANUARY,
					Percentage = 9.24m,
					Changeable = true
				},
				new()
				{
					Id = 2,
					MonthName = MonthNames.FEBRUARY,
					Percentage = 9.24m,
					Changeable = true
				},
				new()
				{
					Id = 3,
					MonthName = MonthNames.MARCH,
					Percentage = 1,
					Changeable = true
				},
				new()
				{
					Id = 4,
					MonthName = MonthNames.APRIL,
					Percentage = 9.24m,
					Changeable = true
				},
				new()
				{
					Id = 5,
					MonthName = MonthNames.MAY,
					Percentage = 3.91m,
					Changeable = false
				},
				new()
				{
					Id = 6,
					MonthName = MonthNames.JUNE,
					Percentage = 11.41m,
					Changeable = false
				},
				new()
				{
					Id = 7,
					MonthName = MonthNames.JULY,
					Percentage = 11.41m,
					Changeable = false
				},
				new()
				{
					Id = 8,
					MonthName = MonthNames.AUGUST,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 9,
					MonthName = MonthNames.SEPTEMBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 10,
					MonthName = MonthNames.OCTOBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 11,
					MonthName = MonthNames.NOVEMBER,
					Percentage = 8.91m,
					Changeable = false
				},
				new()
				{
					Id = 12,
					MonthName = MonthNames.DECEMBER,
					Percentage = 8.90m,
					Changeable = false
				},
			},
			// hasError
			false
		}
	};
}