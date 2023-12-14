// <copyright file="ProductMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class ProductMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<Product>()
			.HasOne(i => i.Project)
			.WithMany(i => i.Products)
			.HasForeignKey(i => i.ProjectId)
			.HasPrincipalKey(i => i.Id);

		builder.Entity<Product>()
			.HasOne(i => i.Seasonality)
			.WithMany(i => i.Products)
			.HasForeignKey(i => i.SeasonalityId)
			.HasPrincipalKey(i => i.Id);
		builder.Entity<Product>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Product>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}