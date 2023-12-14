// <copyright file="GeniaContext.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Config.Mappers;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Config;

public partial class GeniaContext : DbContext
{
	public GeniaContext()
	{
	}

	public GeniaContext(DbContextOptions<GeniaContext> options)
		: base(options)
	{
	}

	public DbSet<Dimension> Dimensions { get; set; }

	public DbSet<Modal> Modals { get; set; }

	public DbSet<Product> Products { get; set; }

	public DbSet<ProductConfig> ProductConfigs { get; set; }

	public DbSet<Project> Projects { get; set; }

	public DbSet<Seasonality> Seasonalities { get; set; }

	public DbSet<SeasonalityMonthData> SeasonalityMonthData { get; set; }

	public DbSet<TremTipo> TremTipos { get; set; }

	public DbSet<Transporter> Transporters { get; set; }

	public DbSet<AppFeatureToggle> AppFeatureToggle { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		ModalMapper.MapEntity(builder);
		ProductMapper.MapEntity(builder);
		ProductConfigMapper.MapEntity(builder);
		ProjectMapper.MapEntity(builder);
		SeasonalityMapper.MapEntity(builder);
		DimensionMapper.MapEntity(builder);
		SeasonalityMonthDataMapper.MapEntity(builder);
		TremTipoMapper.MapEntity(builder);
		AppFeatureToggleMapper.MapEntity(builder);
		TransporterMapper.MapEntity(builder);
		this.OnModelBuilding(builder);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
	}

	partial void OnModelBuilding(ModelBuilder builder);
}