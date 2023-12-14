// <copyright file="Modal.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("modal", Schema = "public")]
public partial class Modal
{
	[Key]
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? Id { get; set; }

	[Column("class_name")] public string ClassName { get; set; }

	[Column("type")] public ModalTypes? Type { get; set; }

	[Column("flow_type")] public FlowTypes? FlowType { get; set; }

	[Column("share")] public decimal Share { get; set; }

	[Column("expedicao_capacity")] public decimal? ExpedicaoCapacity { get; set; }

	[Column("days_by_month")] public short DaysByMonth { get; set; }

	[Column("operational_cycle", TypeName = "jsonb")]
	public string OperationalCycle { get; set; }

	[Column("created_at")] public DateTime? CreatedAt { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }

	[Column("deleted_at")] public DateTime? DeletedAt { get; set; }

	[Column("seasonality_id")] public long? SeasonalityId { get; set; }

	public Seasonality Seasonality { get; set; }

	[Column("trem_tipo_id")] public long? TremTipoId { get; set; }

	public TremTipo TremTipo { get; set; }

	[Column("transporter_id")] public long? TransporterId { get; set; }

	public Transporter Transporter { get; set; }

	[Column("product_id")] public long? ProductId { get; set; }

	public Product Product { get; set; }
}