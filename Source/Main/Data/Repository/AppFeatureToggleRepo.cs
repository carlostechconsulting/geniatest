// <copyright file="AppFeatureToggleRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Data.Repository;

/// <summary>
/// Repository class for <see cref="AppFeatureToggle"/>.
/// </summary>
public class AppFeatureToggleRepo : GenericRepo
{
	private readonly GeniaContext _dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AppFeatureToggleRepo"/> class.
	/// </summary>
	/// <param name="dbContext"></param>
	public AppFeatureToggleRepo(GeniaContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Get feature toggle by key.
	/// </summary>
	/// <param name="key"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public Task<AppFeatureToggle> GetByKey(FeatureToggleKey key)
	{
		return Task.Run(() => _dbContext.AppFeatureToggle
			.AsNoTracking()
			.FirstOrDefault(i => i.Key == key));
	}

	public Task<List<AppFeatureToggle>> FetchAll()
	{
		return Task.Run(() => _dbContext.AppFeatureToggle
			.AsNoTracking().ToList());
	}

	public Task<AppFeatureToggle> Update(AppFeatureToggle toggle)
	{
		var itemToUpdate = _dbContext.AppFeatureToggle
			.FirstOrDefault(i => i.Key == toggle.Key);

		if (itemToUpdate == null)
		{
			throw new Exception("Item no longer available");
		}

		var entryToUpdate = _dbContext.Entry(itemToUpdate);
		entryToUpdate.CurrentValues.SetValues(toggle);
		entryToUpdate.State = EntityState.Modified;

		_dbContext.SaveChanges();
		return Task.FromResult(toggle);
	}
}