// <copyright file="ProductService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Modals.Services;
using GeniaWebApp.Source.Main.Modules.Projects.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Services;
using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Products.Services;

public class ProductService
{
	public ProductService(ILogger<ProjectService> logger, DiscordNotifier discordNotifier, ProductRepo productRepo,
		SeasonalityRepo seasonalityRepo, ModalService modalService)
	{
		Logger = logger;
		DiscordNotifier = discordNotifier;
		ProductRepo = productRepo;
		SeasonalityRepo = seasonalityRepo;
		ModalService = modalService;
	}

	private ILogger<ProjectService> Logger { get; set; }

	private DiscordNotifier DiscordNotifier { get; set; }

	private ProductRepo ProductRepo { get; set; }

	private SeasonalityRepo SeasonalityRepo { get; set; }

	private ModalService ModalService { get; set; }

	public Task DeleteAll(List<Product> products)
	{
		return Task.WhenAll(products.Select(Delete));
	}

	/// <summary>
	/// Delete a product
	/// </summary>
	/// <param name="product"></param>
	/// <returns></returns>
	/// <exception cref="GeniaGenericException"></exception>
	public Task Delete(Product product)
	{
		return ModalService.DeleteAll(product.Modals)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					Logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [Delete] {task.Exception.Message}");
					throw new GeniaGenericException(string.Empty);
				}

				return ProductRepo.DeleteProduct(product.Id).Result;
			})
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					Logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [Delete] {task.Exception.Message}");
					throw new GeniaGenericException(string.Empty);
				}

				return product.SeasonalityId != null
					? SeasonalityRepo.DeleteSeasonality(product.SeasonalityId)
					: Task.CompletedTask;
			}).ContinueWith(task =>
			{
				if (task.Exception is not null)
					Logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [Delete] {task.Exception.Message}");
				return task;
			});
	}

	/// <summary>
	/// Create a new product
	/// </summary>
	/// <param name="product"></param>
	/// <returns></returns>
	public Task Create(Product product)
	{
		return ProductRepo.CreateProduct(product);
	}

	public Task Update(Product product)
	{
		return ProductRepo.UpdateProduct(product.Id, product)
			.ContinueWith(task =>
			{
				var tasks = product.Modals?.Select(modal => modal.Id.HasValue
					? ModalService.Update(modal)
					: ModalService.Create(modal));
				return Task.WhenAll(tasks);
			});
	}

	public Task<Product> GetProductById(long id)
	{
		return ProductRepo.GetProductById(id);
	}
}