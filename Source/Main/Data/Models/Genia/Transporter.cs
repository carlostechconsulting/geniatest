// <copyright file="Transporter.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("transporter", Schema = "public")]
public partial class Transporter
{
	[Key]
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? Id { get; set; }

	[Column("applicable_modal_type")] public ModalTypes? ApplicableModalType { get; set; }

	[Column("name")] public string? Name { get; set; }

	[Column("volume")] public decimal? Volume { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	public ICollection<Modal> Modals { get; set; }
}