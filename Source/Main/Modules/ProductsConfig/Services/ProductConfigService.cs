// <copyright file="ProductConfigService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Modules.ProductsConfig.Services;

public class ProductConfigService
{
	private ProductConfigRepo productConfigRepo;
	private SeasonalityRepo seasonalityRepo;

	public ProductConfigService(ProductConfigRepo productConfigRepo, SeasonalityRepo seasonalityRepo)
	{
		this.productConfigRepo = productConfigRepo;
		this.seasonalityRepo = seasonalityRepo;
	}

	/// <summary>
	/// delete a product config and it children.
	/// </summary>
	/// <param name="productConfig"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public async Task DeleteAndCascade(ProductConfig productConfig)
	{
		await productConfigRepo.DeleteProductConfig(productConfig.Id.Value);

		if (productConfig.Seasonality != null)
		{
			seasonalityRepo.DeleteSeasonality(productConfig.Seasonality.Id.Value);
		}
	}

	/// <summary>
	/// create new.
	/// </summary>
	/// <param name="productConfig"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <exception cref="GeniaConstraintException"></exception>
	public async Task<ProductConfig> Create(ProductConfig productConfig)
	{
		return await productConfigRepo.GetByName(productConfig.Name)
			.ContinueWith(
				(previousTask) =>
				{
					if (previousTask.Exception is not null)
						throw new GeniaConstraintException(
							"Product config name should be unique",
							TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);
					
					return previousTask.Result;
				})
			.ContinueWith(
				(v) =>
					productConfigRepo.CreateProductConfig(productConfig).Result)
			.ContinueWith(
				(previousTask) =>
				{
					if (previousTask.Exception is not null)
					{
						throw new GeniaConstraintException(string.Empty);
					}

					return previousTask.Result;
				});
	}

	/// <summary>
	/// update existing.
	/// </summary>
	/// <param name="productConfig"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <exception cref="GeniaConstraintException"></exception>
	public async Task<ProductConfig> Update(ProductConfig productConfig)
	{
		var product = await productConfigRepo.GetByName(productConfig.Name);

		if (product is not null && product.Id != productConfig.Id)
			throw new GeniaConstraintException(
				"Product config name should be unique",
				TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);

		try
		{
			return await productConfigRepo.UpdateProductConfig(productConfig.Id.Value, productConfig);
		}
		catch (DbUpdateException e)
		{
			Console.WriteLine(e);
			throw new GeniaConstraintException(string.Empty, TransKeys.DATA_LAYER_GENERIC_ERROR);
		}
	}
}