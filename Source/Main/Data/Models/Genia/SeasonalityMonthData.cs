// <copyright file="SeasonalityMonthData.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("seasonality_month_data", Schema = "public")]
public partial class SeasonalityMonthData
{
	[Key]
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? Id { get; set; }

	[Column("percentage")] public decimal? Percentage { get; set; }

	[Column("value")] public decimal? Value { get; set; }

	[NotMapped] public bool Changeable { get; set; }

	[Column("month_name")] public MonthNames MonthName { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	[Column("seasonality_id")] public long? SeasonalityId { get; set; }

	public Seasonality Seasonality { get; set; }

	public SeasonalityMonthData Copy()
	{
		return (SeasonalityMonthData)MemberwiseClone();
	}

	public SeasonalityMonthData CloneNew()
	{
		var seasonalityMonthData = (SeasonalityMonthData)MemberwiseClone();
		seasonalityMonthData.Id = null;
		seasonalityMonthData.Value = null;
		seasonalityMonthData.Seasonality = null;
		seasonalityMonthData.CreatedAt = null;
		seasonalityMonthData.UpdatedAt = null;

		return seasonalityMonthData;
	}
}