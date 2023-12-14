// <copyright file="ProjectMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class ProjectMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<Project>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Project>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}