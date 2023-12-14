// <copyright file="ProductConfig.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("product_config", Schema = "public")]
public partial class ProductConfig
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("id")]
	public long? Id { get; set; }

	[Column("seasonality_id")]
	[ForeignKey("Seasonality")]
	public long? SeasonalityId { get; set; }

	[Column("name")] public string Name { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	public Seasonality Seasonality { get; set; }
}