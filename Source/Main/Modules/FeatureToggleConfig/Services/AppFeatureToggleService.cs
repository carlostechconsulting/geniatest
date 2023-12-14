// <copyright file="AppFeatureToggleService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Projects.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Services;

namespace GeniaWebApp.Source.Main.Modules.FeatureToggleConfig.Services;

/// <summary>
/// AppFeatureToggleService.
/// </summary>
public class AppFeatureToggleService
{
	private readonly AppFeatureToggleRepo _appFeatureToggleRepo;
	private readonly ILogger<ProjectService> _logger;
	private DiscordNotifier _discordNotifier;

	public AppFeatureToggleService(AppFeatureToggleRepo appFeatureToggleRepo, ILogger<ProjectService> logger,
		DiscordNotifier discordNotifier)
	{
		_appFeatureToggleRepo = appFeatureToggleRepo;
		_logger = logger;
		_discordNotifier = discordNotifier;
	}

	/// <summary>
	/// Get value by key.
	/// </summary>
	/// <param name="key"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <exception cref="GeniaGenericException"></exception>
	public Task<bool> GetValue(FeatureToggleKey key)
	{
		return _appFeatureToggleRepo.GetByKey(key)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					_logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [GetValue] {task.Exception.Message}");
					throw new GeniaGenericException(string.Empty);
				}

				return task.Result is not null && task.Result.Value;
			});
	}

	public Task<List<AppFeatureToggle>> FetchAll()
	{
		return _appFeatureToggleRepo.FetchAll()
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					_logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [GetValue] {task.Exception.Message}");
					throw new GeniaGenericException(string.Empty);
				}

				return task.Result;
			});
	}

	public Task<AppFeatureToggle> Update(AppFeatureToggle appFeatureToggle)
	{
		return _appFeatureToggleRepo.Update(appFeatureToggle)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					_logger.Log(LogLevel.Error, task.Exception,
						$"[GENIA] [GetValue] {task.Exception.Message}");
					throw new GeniaGenericException(string.Empty);
				}

				return task.Result;
			});
	}
}