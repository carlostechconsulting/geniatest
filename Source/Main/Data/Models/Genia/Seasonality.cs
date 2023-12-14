// <copyright file="Seasonality.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("seasonality", Schema = "public")]
public partial class Seasonality
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("id")]
	public long? Id { get; set; }

	[Column("year")] public int Year { get; set; }

	public ICollection<SeasonalityMonthData> SeasonalitiesMonthData { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	public ICollection<ProductConfig> ProductConfigs { get; set; }

	public ICollection<Modal> Modals { get; set; }

	public ICollection<Product> Products { get; set; }

	public Seasonality CloneNew()
	{
		var seasonality = (Seasonality)MemberwiseClone();
		seasonality.Id = null;
		seasonality.Modals = null;
		seasonality.Products = null;
		seasonality.CreatedAt = null;
		seasonality.UpdatedAt = null;
		seasonality.ProductConfigs = null;
		seasonality.SeasonalitiesMonthData = seasonality.SeasonalitiesMonthData
			.Select(item => item.CloneNew()).ToList();

		return seasonality;
	}
}