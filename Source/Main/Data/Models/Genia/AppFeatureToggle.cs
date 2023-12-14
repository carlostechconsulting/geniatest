// <copyright file="AppFeatureToggle.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;

namespace GeniaWebApp.Source.Main.Data.Models.Genia;

[Table("app_feature_toggle", Schema = "public")]
public partial class AppFeatureToggle
{
	[Key] [Column("key")] public FeatureToggleKey Key { get; set; }

	[Column("value")] public bool Value { get; set; }

	[Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}