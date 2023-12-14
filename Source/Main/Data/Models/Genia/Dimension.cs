// <copyright file="Dimension.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("dimension", Schema = "public")]
public partial class Dimension
{
	[Key]
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? Id { get; set; }

	[Column("height")] public decimal? Height { get; set; }

	[Column("length")] public decimal? Length { get; set; }

	[Column("width")] public decimal? Width { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	public ICollection<TremTipo> TremTipos { get; set; }
}