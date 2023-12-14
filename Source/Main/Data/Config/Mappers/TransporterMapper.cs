// <copyright file="TransporterMapper.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeniaWebApp.Source.Main.Data.Config.Mappers;

public static class TransporterMapper
{
	public static void MapEntity(ModelBuilder builder)
	{
		builder
			.Entity<Transporter>()
			.Property(e => e.ApplicableModalType)
			.HasConversion(new EnumToStringConverter<ModalTypes>());
		builder.Entity<Transporter>()
			.Property(p => p.CreatedAt)
			.HasDefaultValueSql(@"now()");
		builder.Entity<Transporter>()
			.Property(p => p.UpdatedAt)
			.HasDefaultValueSql(@"now()");
	}
}