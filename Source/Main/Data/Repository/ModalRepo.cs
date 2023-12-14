// <copyright file="ModalRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class ModalRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public ModalRepo(GeniaContext dbContext, NavigationManager navigationManager)
	{
		dBContext = dbContext;
		this.navigationManager = navigationManager;
	}

	public async Task ExportModalsToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/modals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/modals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportModalsToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/modals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/modals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task<IQueryable<Modal>> GetModals(Query query = null)
	{
		var items = dBContext.Modals.AsQueryable();

		items = items.Include(i => i.Seasonality);
		items = items.Include(i => i.TremTipo);

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

		OnModalsRead(ref items);

		return await Task.FromResult(items);
	}

	public async Task<Modal> GetModalById(int id)
	{
		var items = dBContext.Modals
			.AsNoTracking()
			.Where(i => i.Id == id);

		items = items.Include(i => i.Seasonality);
		items = items.Include(i => i.TremTipo);

		OnGetModalById(ref items);

		var itemToReturn = items.FirstOrDefault();

		OnModalGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public Task<Modal> CreateModal(Modal modal)
	{
		OnModalCreated(modal);

		var existingItem = dBContext.Modals
			.FirstOrDefault(i => i.Id == modal.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.Modals.Add(modal);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(modal).State = EntityState.Detached;
			throw;
		}

		OnAfterModalCreated(modal);

		return Task.FromResult(modal);
	}

	public async Task<Modal> CancelModalChanges(Modal item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Modal> UpdateModal(long? id, Modal modal)
	{
		OnModalUpdated(modal);

		var itemToUpdate = dBContext.Modals
			.FirstOrDefault(i => i.Id == modal.Id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(modal);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterModalUpdated(modal);

		return modal;
	}

	public async Task<Modal> DeleteModal(long? id)
	{
		var itemToDelete = dBContext.Modals
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnModalDeleted(itemToDelete);

		dBContext.Modals.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterModalDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnModalsRead(ref IQueryable<Modal> items);

	partial void OnModalGet(Modal item);

	partial void OnGetModalById(ref IQueryable<Modal> items);

	partial void OnModalCreated(Modal item);

	partial void OnAfterModalCreated(Modal item);

	partial void OnModalUpdated(Modal item);

	partial void OnAfterModalUpdated(Modal item);

	partial void OnModalDeleted(Modal item);

	partial void OnAfterModalDeleted(Modal item);
}