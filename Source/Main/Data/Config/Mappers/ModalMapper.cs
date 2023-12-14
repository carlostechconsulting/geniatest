// <copyright file="ModalMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class ModalMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder.Entity<Modal>()
			.HasOne(i => i.Seasonality)
			.WithMany(i => i.Modals)
			.HasForeignKey(i => i.SeasonalityId)
			.HasPrincipalKey(i => i.Id);

		builder.Entity<Modal>()
			.HasOne(i => i.TremTipo)
			.WithMany(i => i.Modals)
			.HasForeignKey(i => i.TremTipoId)
			.HasPrincipalKey(i => i.Id);

		builder.Entity<Modal>()
			.HasOne(i => i.Product)
			.WithMany(i => i.Modals)
			.HasForeignKey(i => i.ProductId)
			.HasPrincipalKey(i => i.Id);

		builder.Entity<Modal>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Modal>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
		builder
			.Entity<Modal>()
			.Property(e => e.FlowType)
			.HasConversion(new EnumToStringConverter<FlowTypes>());
		builder
			.Entity<Modal>()
			.Property(e => e.Type)
			.HasConversion(new EnumToStringConverter<ModalTypes>());
	}
}