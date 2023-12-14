// <copyright file="ProductConfigMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class ProductConfigMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<ProductConfig>()
			.HasOne(i => i.Seasonality)
			.WithMany(i => i.ProductConfigs)
			.HasForeignKey(i => i.SeasonalityId)
			.HasPrincipalKey(i => i.Id);

		builder.Entity<ProductConfig>()
			.HasIndex(p => p.Name)
			.IsUnique();

		builder.Entity<ProductConfig>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<ProductConfig>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}