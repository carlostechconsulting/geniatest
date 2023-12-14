// <copyright file="DimensionMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class DimensionMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<Dimension>()
			.Property(p => p.Height)
			.HasPrecision(10, 4);

		builder.Entity<Dimension>()
			.Property(p => p.Length)
			.HasPrecision(10, 4);

		builder.Entity<Dimension>()
			.Property(p => p.Width)
			.HasPrecision(10, 4);
		builder.Entity<Dimension>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Dimension>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}