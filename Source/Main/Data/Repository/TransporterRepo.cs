// <copyright file="TransporterRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class TransporterRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public TransporterRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportTransportersToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/transporters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/transporters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportTransportersToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/transporters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/transporters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public Task<IQueryable<Transporter>> GetTransporters(Query query = null)
	{
		var items = dBContext.Transporters.AsQueryable();

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

		OnTransportersRead(ref items);

		return Task.FromResult(items);
	}

	public Transporter GetByName(string name)
	{
		return dBContext.Transporters
			.AsNoTracking()
			.FirstOrDefault(i => i.Name == name);
	}

	public async Task<Transporter> GetTransporterById(long? id)
	{
		var items = dBContext.Transporters
			.AsNoTracking()
			.Where(i => i.Id == id);
		OnGetTransporterById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnTransporterGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<Transporter> CreateTransporter(Transporter entity)
	{
		OnTransporterCreated(entity);

		var existingItem = dBContext.Transporters
			.FirstOrDefault(i => i.Id == entity.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			entity.UpdatedAt = DateTime.UtcNow;
			dBContext.Transporters.Add(entity);
			dBContext.SaveChanges();
		}
		catch (Exception exception)
		{
			dBContext.Entry(entity).State = EntityState.Detached;
			throw;
		}

		OnAfterTransporterCreated(entity);

		return entity;
	}

	public async Task<Transporter> CancelTransporterChanges(Transporter item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Transporter> UpdateTransporter(
		long? id,
		Transporter entity)
	{
		OnTransporterUpdated(entity);

		var itemToUpdate = dBContext.Transporters
			.FirstOrDefault(i => i.Id == entity.Id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		entity.UpdatedAt = DateTime.UtcNow;
		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(entity);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterTransporterUpdated(entity);

		return entity;
	}

	public async Task<Transporter> DeleteTransporter(long? id)
	{
		var itemToDelete = dBContext.Transporters
			.Where(i => i.Id == id)
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnTransporterDeleted(itemToDelete);

		dBContext.Transporters.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterTransporterDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnTransportersRead(ref IQueryable<Transporter> items);

	partial void OnTransporterGet(Transporter item);

	partial void OnGetTransporterById(ref IQueryable<Transporter> items);

	partial void OnTransporterCreated(Transporter item);

	partial void OnAfterTransporterCreated(Transporter item);

	partial void OnTransporterUpdated(Transporter item);

	partial void OnAfterTransporterUpdated(Transporter item);

	partial void OnTransporterDeleted(Transporter item);

	partial void OnAfterTransporterDeleted(Transporter item);
}