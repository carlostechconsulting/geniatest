// <copyright file="ProjectRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text.Encodings.Web;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public partial class ProjectRepo : GenericRepo
{
	private readonly GeniaContext dBContext;
	private readonly NavigationManager navigationManager;

	public ProjectRepo(GeniaContext dbContext)
	{
		dBContext = dbContext;
	}

	public async Task ExportProjectsToExcel(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/projects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/projects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public async Task ExportProjectsToCSV(Query query = null, string fileName = null)
	{
		navigationManager.NavigateTo(
			query != null
				? query.ToUrl(
					$"export/genia/projects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
				: $"export/genia/projects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
			true);
	}

	public Task<Project> GetByName(string name)
	{
		return Task.Run(() => dBContext.Projects
			.AsNoTracking()
			.FirstOrDefault(i => i.Name == name));
	}

	public Task<IQueryable<Project>> GetProjects(Query query = null)
	{
		var items = dBContext.Projects.AsQueryable()
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

		OnProjectsRead(ref items);

		return Task.FromResult(items);
	}

	public Task<Project> GetProjectWithChildrenById(long? id)
	{
		var items = dBContext.Projects
			.AsNoTracking()
			.Include("Products.Modals.TremTipo")
			.Include("Products.Seasonality.SeasonalitiesMonthData")
			.Where(i => i.Id == id);

		OnGetProjectById(ref items);
		var itemToReturn = items
			.FirstOrDefault();

		OnProjectGet(itemToReturn);

		return Task.FromResult(itemToReturn);
	}

	public async Task<Project> GetProjectById(long? id)
	{
		var items = dBContext.Projects
			.Include(p => p.Products)
			.ThenInclude(prod => prod.Modals)
			.Where(i => i.Id == id);

		OnGetProjectById(ref items);
		var itemToReturn = items
			.FirstOrDefault();

		OnProjectGet(itemToReturn);

		return await Task.FromResult(itemToReturn);
	}

	public async Task<Project> CreateProject(Project project)
	{
		OnProjectCreated(project);

		var existingItem = dBContext.Projects
			.AsNoTracking()
			.FirstOrDefault(i => i.Id == project.Id);

		if (existingItem != null)
		{
			throw new Exception("Item already available");
		}

		try
		{
			dBContext.Projects.Add(project);
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(project).State = EntityState.Detached;
			throw;
		}

		OnAfterProjectCreated(project);

		return project;
	}

	public async Task<Project> CancelProjectChanges(Project item)
	{
		var entityToCancel = dBContext.Entry(item);
		if (entityToCancel.State == EntityState.Modified)
		{
			entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
			entityToCancel.State = EntityState.Unchanged;
		}

		return item;
	}

	public async Task<Project> UpdateProject(
		long? id,
		Project project)
	{
		OnProjectUpdated(project);

		var itemToUpdate = dBContext.Projects
			.FirstOrDefault(i => i.Id == project.Id);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = dBContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(project);
		entryToUpdate.State = EntityState.Modified;

		dBContext.SaveChanges();

		OnAfterProjectUpdated(project);

		return project;
	}

	public async Task<Project> DeleteProject(long? id)
	{
		var itemToDelete = dBContext.Projects
			.Include(i => i.Products)
			.FirstOrDefault(i => i.Id == id);

		if (itemToDelete == null)
		{
			throw new Exception("Item no longer available");
		}

		OnProjectDeleted(itemToDelete);

		dBContext.Projects.Remove(itemToDelete);

		try
		{
			dBContext.SaveChanges();
		}
		catch
		{
			dBContext.Entry(itemToDelete).State = EntityState.Unchanged;
			throw;
		}

		OnAfterProjectDeleted(itemToDelete);

		return itemToDelete;
	}

	partial void OnProjectsRead(ref IQueryable<Project> items);

	partial void OnProjectGet(Project item);

	partial void OnGetProjectById(ref IQueryable<Project> items);

	partial void OnProjectCreated(Project item);

	partial void OnAfterProjectCreated(Project item);

	partial void OnProjectUpdated(Project item);

	partial void OnAfterProjectUpdated(Project item);

	partial void OnProjectDeleted(Project item);

	partial void OnAfterProjectDeleted(Project item);
}