// <copyright file="SeasonalityRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class SeasonalityRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public SeasonalityRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportSeasonalitiesToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/seasonalities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/seasonalities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportSeasonalitiesToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/seasonalities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/seasonalities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<Seasonality>> GetSeasonalities(Query query = null)
	{
		var items = dBContext.Seasonalities.AsQueryable();

		items = items.Include(i => i.SeasonalitiesMonthData);

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

		OnSeasonalitiesRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<Seasonality> GetSeasonalityById(int id)
	{
		var items = dBContext.Seasonalities
			.AsNoTracking()
			.Where(i => i.Id == id);

		items = items.Include(i => i.SeasonalitiesMonthData);

		OnGetSeasonalityById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnSeasonalityGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<Seasonality> CreateSeasonality(
		Seasonality seasonality)
	{
		OnSeasonalityCreated(seasonality);

		var existingItem = dBContext.Seasonalities
			.Where(i => i.Id == seasonality.Id)
			.FirstOrDefault();

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.Seasonalities.Add(seasonality);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(seasonality).State = EntityState.Detached;
			throw;
		}

		OnAfterSeasonalityCreated(seasonality);

		return seasonality;
	}

	public async Task<Seasonality> CancelSeasonalityChanges(
		Seasonality item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Seasonality> UpdateSeasonality(
		int id,
		Seasonality seasonality)
	{
		OnSeasonalityUpdated(seasonality);

		var itemToUpdate = dBContext.Seasonalities
			.Where(i => i.Id == seasonality.Id)
			.FirstOrDefault();

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(seasonality);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterSeasonalityUpdated(seasonality);

		return seasonality;
	}

	public async Task<Seasonality> DeleteSeasonality(long? id)
	{
		var itemToDelete = dBContext.Seasonalities
			.Where(i => i.Id == id)
			.Include(i => i.ProductConfigs)
			.Include(i => i.Modals)
			.Include(i => i.Products)
			.FirstOrDefault();

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnSeasonalityDeleted(itemToDelete);

		dBContext.Seasonalities.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterSeasonalityDeleted(itemToDelete);

		return itemToDelete;
	}

	public async Task ExportSeasonalityMonthDataToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/seasonalitymonthdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/seasonalitymonthdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportSeasonalityMonthDataToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/seasonalitymonthdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/seasonalitymonthdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<SeasonalityMonthData>> GetSeasonalityMonthData(
		Query query = null)
	{
		var items = dBContext.SeasonalityMonthData.AsQueryable();

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

		OnSeasonalityMonthDataRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<SeasonalityMonthData> GetSeasonalityMonthDatumById(int id)
	{
		var items = dBContext.SeasonalityMonthData
			.AsNoTracking()
			.Where(i => i.Id == id);

		OnGetSeasonalityMonthDatumById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnSeasonalityMonthDatumGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<SeasonalityMonthData> CreateSeasonalityMonthDatum(
		SeasonalityMonthData seasonalitymonthdatum)
	{
		OnSeasonalityMonthDatumCreated(seasonalitymonthdatum);

		var existingItem = dBContext.SeasonalityMonthData
			.Where(i => i.Id == seasonalitymonthdatum.Id)
			.FirstOrDefault();

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.SeasonalityMonthData.Add(seasonalitymonthdatum);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(seasonalitymonthdatum).State = EntityState.Detached;
			throw;
		}

		OnAfterSeasonalityMonthDatumCreated(seasonalitymonthdatum);

		return seasonalitymonthdatum;
	}

	public async Task<SeasonalityMonthData> CancelSeasonalityMonthDatumChanges(
		SeasonalityMonthData item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<SeasonalityMonthData> UpdateSeasonalityMonthDatum(
		int id,
		SeasonalityMonthData seasonalitymonthdatum)
	{
		OnSeasonalityMonthDatumUpdated(seasonalitymonthdatum);

		var itemToUpdate = dBContext.SeasonalityMonthData
			.Where(i => i.Id == seasonalitymonthdatum.Id)
			.FirstOrDefault();

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(seasonalitymonthdatum);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterSeasonalityMonthDatumUpdated(seasonalitymonthdatum);

		return seasonalitymonthdatum;
	}

	public async Task<SeasonalityMonthData> DeleteSeasonalityMonthDatum(int id)
	{
		var itemToDelete = dBContext.SeasonalityMonthData
			.Where(i => i.Id == id)
			.Include(i => i.Seasonality)
			.FirstOrDefault();

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnSeasonalityMonthDatumDeleted(itemToDelete);

		dBContext.SeasonalityMonthData.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterSeasonalityMonthDatumDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnSeasonalitiesRead(ref IQueryable<Seasonality> items);

	partial void OnSeasonalityGet(Seasonality item);

	partial void OnGetSeasonalityById(ref IQueryable<Seasonality> items);

	partial void OnSeasonalityCreated(Seasonality item);

	partial void OnAfterSeasonalityCreated(Seasonality item);

	partial void OnSeasonalityUpdated(Seasonality item);

	partial void OnAfterSeasonalityUpdated(Seasonality item);

	partial void OnSeasonalityDeleted(Seasonality item);

	partial void OnAfterSeasonalityDeleted(Seasonality item);

	partial void OnSeasonalityMonthDataRead(ref IQueryable<SeasonalityMonthData> items);

	partial void OnSeasonalityMonthDatumGet(SeasonalityMonthData item);

	partial void OnGetSeasonalityMonthDatumById(ref IQueryable<SeasonalityMonthData> items);

	partial void OnSeasonalityMonthDatumCreated(SeasonalityMonthData item);

	partial void OnAfterSeasonalityMonthDatumCreated(SeasonalityMonthData item);

	partial void OnSeasonalityMonthDatumUpdated(SeasonalityMonthData item);

	partial void OnAfterSeasonalityMonthDatumUpdated(SeasonalityMonthData item);

	partial void OnSeasonalityMonthDatumDeleted(SeasonalityMonthData item);

	partial void OnAfterSeasonalityMonthDatumDeleted(SeasonalityMonthData item);
}