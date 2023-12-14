// <copyright file="SeasonalityMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class SeasonalityMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<Seasonality>()
			.HasMany(s => s.SeasonalitiesMonthData)
			.WithOne(i => i.Seasonality)
			.HasForeignKey(s => s.SeasonalityId)
			.HasPrincipalKey(sd => sd.Id);

		builder.Entity<Seasonality>()
			.HasMany(s => s.ProductConfigs)
			.WithOne(i => i.Seasonality);

		builder.Entity<Seasonality>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Seasonality>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}