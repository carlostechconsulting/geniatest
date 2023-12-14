// <copyright file="Product.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("product", Schema = "public")]
public partial class Product
{
	[Key]
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? Id { get; set; }

	[Column("seasonality_id")] public long? SeasonalityId { get; set; }

	[Column("project_id")] public long? ProjectId { get; set; }

	[Column("name")] public string Name { get; set; }

	[Column("volume")] public decimal? Volume { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	public Project Project { get; set; }

	public Seasonality Seasonality { get; set; }

	public List<Modal> Modals { get; set; }

	[NotMapped] public decimal ReceptionShare { get; set; } = new(0.0);

	[NotMapped] public decimal ExpeditionShare { get; set; } = new(0.0);
}