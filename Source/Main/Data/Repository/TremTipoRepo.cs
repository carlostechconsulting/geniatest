// <copyright file="TremTipoRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class TremTipoRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public TremTipoRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportTremTiposToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/tremtipos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/tremtipos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportTremTiposToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/tremtipos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/tremtipos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<TremTipo>> GetTremTipos(Query query = null)
	{
		var items = dBContext.TremTipos.AsQueryable();

		items = items.Include(i => i.Dimension);

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

		OnTremTiposRead(ref items);

		return await Task.FromResult(items);
	}

	public TremTipo GetByName(string name)
	{
		return dBContext.TremTipos
			.AsNoTracking()
			.FirstOrDefault(i => i.Name == name);
	}

	public async Task<TremTipo> GetTremTipoById(long? id)
	{
		var items = dBContext.TremTipos
			.AsNoTracking()
			.Where(i => i.Id == id);

		items = items.Include(i => i.Dimension);

		OnGetTremTipoById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnTremTipoGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<TremTipo> CreateTremTipo(TremTipo tremtipo)
	{
		OnTremTipoCreated(tremtipo);

		var existingItem = dBContext.TremTipos
			.FirstOrDefault(i => i.Id == tremtipo.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			tremtipo.UpdatedAt = DateTime.UtcNow;
			dBContext.TremTipos.Add(tremtipo);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(tremtipo).State = EntityState.Detached;
			throw;
		}

		OnAfterTremTipoCreated(tremtipo);

		return tremtipo;
	}

	public async Task<TremTipo> CancelTremTipoChanges(TremTipo item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<TremTipo> UpdateTremTipo(
		long? id,
		TremTipo tremtipo)
	{
		OnTremTipoUpdated(tremtipo);

		var itemToUpdate = dBContext.TremTipos
			.FirstOrDefault(i => i.Id == id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		tremtipo.UpdatedAt = DateTime.UtcNow;
		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(tremtipo);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterTremTipoUpdated(tremtipo);

		return tremtipo;
	}

	public async Task<TremTipo> DeleteTremTipo(long? id)
	{
		var itemToDelete = dBContext.TremTipos
			.Where(i => i.Id == id)
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnTremTipoDeleted(itemToDelete);

		dBContext.TremTipos.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterTremTipoDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnTremTiposRead(ref IQueryable<TremTipo> items);

	partial void OnTremTipoGet(TremTipo item);

	partial void OnGetTremTipoById(ref IQueryable<TremTipo> items);

	partial void OnTremTipoCreated(TremTipo item);

	partial void OnAfterTremTipoCreated(TremTipo item);

	partial void OnTremTipoUpdated(TremTipo item);

	partial void OnAfterTremTipoUpdated(TremTipo item);

	partial void OnTremTipoDeleted(TremTipo item);

	partial void OnAfterTremTipoDeleted(TremTipo item);
}