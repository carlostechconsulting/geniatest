// <copyright file="TremTipoMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class TremTipoMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<TremTipo>()
			.HasOne(i => i.Dimension)
			.WithMany(i => i.TremTipos)
			.HasForeignKey(i => i.DimensionId)
			.HasPrincipalKey(i => i.Id);
		builder
			.Entity<TremTipo>()
			.Property(e => e.ApplicableModalType)
			.HasConversion(new EnumToStringConverter<ModalTypes>());
		builder.Entity<TremTipo>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<TremTipo>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}