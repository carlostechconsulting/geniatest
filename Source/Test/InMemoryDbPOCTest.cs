// <copyright file="InMemoryDbPOCTest.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Modules.ProductsConfig.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Tests;

public class Tests
{
	private Mock<ProductConfigRepo> _productConfigRepoMock;
	private Mock<SeasonalityRepo> _seasonalityRepoMock;
	private ProductConfigService _target;

	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public async Task Test1()
	{
		var productConfig = new ProductConfig()
		{
			Id = 1,
			Name = "Test"
		};

		// ProductConfigRepoMock.Setup(repo => repo.GetByName(productConfig.Name))
		//     .Returns(Task.FromResult(productConfig));
		// var mockContext = new Mock<GeniaContext>();
		// var NavigationManagerMock = new Mock<NavigationManager>();
		//
		// mockContext.Setup<DbSet<ProductConfig>>(m => m.ProductConfigs)
		// 	.ReturnsDbSet(new List<ProductConfig>());
		
		
		// _productConfigRepoMock = new Mock<ProductConfigRepo>(new ProductConfigRepo(
		// 	mockContext.Object, NavigationManagerMock.Object));
		// _productConfigRepoMock.Setup(repo =>
		// 		repo.CreateProductConfig(productConfig))
		// 	.Returns(Task.FromResult(productConfig));
		//
		// _target = new ProductConfigService(_productConfigRepoMock.Object, _seasonalityRepoMock.Object);
		var db = GetMemoryContext();
		var target = new ProductConfigRepo(db, null);
		
		await target.CreateProductConfig(productConfig);
		Assert.True(true);
	}
	
	private GeniaContext GetMemoryContext()
	{
		var options = new DbContextOptionsBuilder<GeniaContext>()
			.UseInMemoryDatabase(databaseName: "InMemoryDatabase")
			.Options;
		return new GeniaContext(options);
	}
	
}