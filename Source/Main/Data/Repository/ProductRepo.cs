// <copyright file="ProductRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class ProductRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public ProductRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportProductsToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportProductsToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public Task<IQueryable<Product>> GetProducts(Query query = null)
	{
		var items = dBContext.Products.AsQueryable()
			.AsNoTracking();

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

		OnProductsRead(ref items);

		return Task.FromResult(items);
	}

	public Task<Product> GetProductById(long id)
	{
		var items = dBContext.Products
			.AsNoTracking()
			.Where(i => i.Id == id);

		items = items.Include(i => i.Project);
		items = items.Include(i => i.Seasonality);

		OnGetProductById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnProductGet(itemToReturn);

		return Task.FromResult(itemToReturn);
	}

	public async Task<Product> CreateProduct(Product product)
	{
		OnProductCreated(product);

		var existingItem = dBContext.Products
			.FirstOrDefault(i => i.Id == product.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.Products.Add(product);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(product).State = EntityState.Detached;
			throw;
		}

		OnAfterProductCreated(product);

		return product;
	}

	public async Task<Product> CancelProductChanges(Product item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Product> UpdateProduct(long? id, Product product)
	{
		OnProductUpdated(product);

		var itemToUpdate = dBContext.Products
			.FirstOrDefault(i => i.Id == product.Id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(product);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterProductUpdated(product);

		return product;
	}

	public async Task<Product> DeleteProduct(long? id)
	{
		var itemToDelete = dBContext.Products
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnProductDeleted(itemToDelete);

		dBContext.Products.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterProductDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnProductsRead(ref IQueryable<Product> items);

	partial void OnProductGet(Product item);

	partial void OnGetProductById(ref IQueryable<Product> items);

	partial void OnProductCreated(Product item);

	partial void OnAfterProductCreated(Product item);

	partial void OnProductUpdated(Product item);

	partial void OnAfterProductUpdated(Product item);

	partial void OnProductDeleted(Product item);

	partial void OnAfterProductDeleted(Product item);
}