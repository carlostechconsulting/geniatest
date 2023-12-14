// <copyright file="ProductConfigRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class ProductConfigRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public ProductConfigRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportProductConfigsToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/productconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/productconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportProductConfigsToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/productconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/productconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<ProductConfig>> GetProductConfigsWithChildren(Query query = null)
	{
		var items = dBContext.ProductConfigs.AsQueryable();

		items = items.Include(i => i.Seasonality)
			.ThenInclude(s => s.SeasonalitiesMonthData);

		if (query != null)
		{
			if (!string.IsNullOrEmpty(query.Expand))
			{
				var propertiesToExpand = query.Expand.Split(',');
				foreach (var p in propertiesToExpand)
				{
					items = items.Include(p.Trim());
				}
			}

			ApplyQuery(ref items, query);
		}

		OnProductConfigsRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<IQueryable<ProductConfig>> GetProductConfigs(Query query = null)
	{
		var items = dBContext.ProductConfigs.AsQueryable();

		items = items.Include(i => i.Seasonality);

		if (query != null)
		{
			if (!string.IsNullOrEmpty(query.Expand))
			{
				var propertiesToExpand = query.Expand.Split(',');
				foreach (var p in propertiesToExpand)
				{
					items = items.Include(p.Trim());
				}
			}

			ApplyQuery(ref items, query);
		}

		OnProductConfigsRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<ProductConfig> GetProductConfigById(int id)
	{
		var items = dBContext.ProductConfigs
			.AsNoTracking()
			.Where(i => i.Id == id);

		items = items.Include(i => i.Seasonality.SeasonalitiesMonthData);

		OnGetProductConfigById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnProductConfigGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public Task<ProductConfig> GetByName(string name)
	{
		return Task.Run(() => dBContext.ProductConfigs
			.AsNoTracking()
			.FirstOrDefault(i => i.Name == name));
	}

	public async Task<ProductConfig> CreateProductConfig(ProductConfig productconfig)
	{
		OnProductConfigCreated(productconfig);
		var existingItem = dBContext.ProductConfigs
			.FirstOrDefault(i => i.Id == productconfig.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.ProductConfigs.Add(productconfig);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(productconfig).State = EntityState.Detached;
			throw;
		}

		OnAfterProductConfigCreated(productconfig);

		return productconfig;
	}

	public async Task<ProductConfig> CancelProductConfigChanges(ProductConfig item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<ProductConfig> UpdateProductConfig(long id, ProductConfig productconfig)
	{
		OnProductConfigUpdated(productconfig);
		productconfig.UpdatedAt = DateTime.UtcNow;
		var itemToUpdate = dBContext.ProductConfigs
			.FirstOrDefault(i => i.Id == productconfig.Id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(productconfig);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterProductConfigUpdated(productconfig);

		return productconfig;
	}

	public async Task<ProductConfig> DeleteProductConfig(long id)
	{
		var itemToDelete = dBContext.ProductConfigs
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnProductConfigDeleted(itemToDelete);

		dBContext.ProductConfigs.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterProductConfigDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnProductConfigsRead(ref IQueryable<ProductConfig> items);

	partial void OnProductConfigGet(ProductConfig item);

	partial void OnGetProductConfigById(ref IQueryable<ProductConfig> items);

	partial void OnProductConfigCreated(ProductConfig item);

	partial void OnAfterProductConfigCreated(ProductConfig item);

	partial void OnProductConfigUpdated(ProductConfig item);

	partial void OnAfterProductConfigUpdated(ProductConfig item);

	partial void OnProductConfigDeleted(ProductConfig item);

	partial void OnAfterProductConfigDeleted(ProductConfig item);
}