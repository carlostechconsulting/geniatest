// <copyright file="SeasonalityMonthDataMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class SeasonalityMonthDataMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<SeasonalityMonthData>()
			.HasOne(s => s.Seasonality)
			.WithMany(i => i.SeasonalitiesMonthData)
			.HasForeignKey(s => s.SeasonalityId)
			.HasPrincipalKey(sd => sd.Id);
		builder.Entity<SeasonalityMonthData>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<SeasonalityMonthData>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");

		builder
			.Entity<SeasonalityMonthData>()
			.Property(e => e.MonthName)
			.HasConversion(new EnumToStringConverter<MonthNames>());
	}
}