// <copyright file="AppFeatureToggleMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class AppFeatureToggleMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<AppFeatureToggle>()
			.Property(p => p.Value)
			.HasDefaultValue(false);

		builder
			.Entity<AppFeatureToggle>()
			.Property(e => e.Key)
			.HasConversion(new EnumToStringConverter<FeatureToggleKey>());
		
		builder.Entity<AppFeatureToggle>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}