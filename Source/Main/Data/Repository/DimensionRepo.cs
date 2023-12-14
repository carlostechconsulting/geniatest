// <copyright file="DimensionRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class DimensionRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public DimensionRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportDimensionsToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/dimensions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/dimensions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportDimensionsToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/dimensions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/dimensions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<Dimension>> GetDimensions(Query query = null)
	{
		var items = dBContext.Dimensions.AsQueryable();

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

		OnDimensionsRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<Dimension> GetDimensionById(int id)
	{
		var items = dBContext.Dimensions
			.AsNoTracking()
			.Where(i => i.Id == id);

		OnGetDimensionById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnDimensionGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<Dimension> CreateDimension(Dimension dimension)
	{
		OnDimensionCreated(dimension);

		var existingItem = dBContext.Dimensions
			.Where(i => i.Id == dimension.Id)
			.FirstOrDefault();

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.Dimensions.Add(dimension);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(dimension).State = EntityState.Detached;
			throw;
		}

		OnAfterDimensionCreated(dimension);

		return dimension;
	}

	public async Task<Dimension> CancelDimensionChanges(Dimension item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Dimension> UpdateDimension(int id, Dimension dimension)
	{
		OnDimensionUpdated(dimension);

		var itemToUpdate = dBContext.Dimensions
			.Where(i => i.Id == dimension.Id)
			.FirstOrDefault();

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(dimension);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterDimensionUpdated(dimension);

		return dimension;
	}

	public async Task<Dimension> DeleteDimension(int id)
	{
		var itemToDelete = dBContext.Dimensions
			.Where(i => i.Id == id)
			.Include(i => i.TremTipos)
			.FirstOrDefault();

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnDimensionDeleted(itemToDelete);

		dBContext.Dimensions.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterDimensionDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnDimensionsRead(ref IQueryable<Dimension> items);

	partial void OnDimensionGet(Dimension item);

	partial void OnGetDimensionById(ref IQueryable<Dimension> items);

	partial void OnDimensionCreated(Dimension item);

	partial void OnAfterDimensionCreated(Dimension item);

	partial void OnDimensionUpdated(Dimension item);

	partial void OnAfterDimensionUpdated(Dimension item);

	partial void OnDimensionDeleted(Dimension item);

	partial void OnAfterDimensionDeleted(Dimension item);
}