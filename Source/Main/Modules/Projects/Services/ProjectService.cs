// <copyright file="ProjectService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Products.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Utils;
using GeniaWebApp.Source.Main.Modules.Shared.Utils;
using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Projects.Services;

public class ProjectService
{
	private ILogger<ProjectService> logger;
	private ProjectRepo projectRepo;
	private ProductService productService;
	private DiscordNotifier discordNotifier;

	public ProjectService(ILogger<ProjectService> logger, ProjectRepo projectRepo, ProductService productService,
		DiscordNotifier discordNotifier)
	{
		this.logger = logger;
		this.projectRepo = projectRepo;
		this.productService = productService;
		this.discordNotifier = discordNotifier;
	}

	/// <summary>
	/// Find all projects.
	/// </summary>
	/// <param name="query"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public Task<IQueryable<Project>> FindAll(Query query = null)
	{
		return projectRepo.GetProjects(query)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					var inner = task.Exception.InnerException;
					logger.Log(LogLevel.Error, inner,
						$"[GENIA] [FetchAll] {inner.Message}");
					ErrorHandler.Handle(inner);
				}

				return task.Result;
			});
	}

	/// <summary>
	/// Create a new project.
	/// </summary>
	/// <param name="project"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <exception cref="GeniaConstraintException"></exception>
	/// <exception cref="GeniaGenericException"></exception>
	public Task<Project> Create(Project project)
	{
		return projectRepo.GetByName(project.Name)
			.ContinueWith(pt =>
			{
				if (pt.Result is not null)
					throw new GeniaConstraintException(
						"Project name should be unique",
						TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);

				return projectRepo.CreateProject(project).Result;
			}).ContinueWith(
				previousTask =>
				{
					if (previousTask.Exception is not null)
					{
						// DataAccessExceptionHandlerUtils.Handle(previousTask.Exception);
						logger.Log(LogLevel.Error, previousTask.Exception,
							$"[GENIA] [Delete] {previousTask.Exception.Message}");
						throw new GeniaGenericException(
							string.Empty,
							TransKeys.DATA_LAYER_GENERIC_ERROR);
					}

					return previousTask.Result;
				}, TaskContinuationOptions.OnlyOnFaulted);
	}
	public Task<Project> Update(Project project)
	{
		return projectRepo.GetByName(project.Name)
			.ContinueWith(pt =>
			{
				if (pt.Result is not null)
					throw new GeniaConstraintException(
						"Project name should be unique",
						TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);

				return projectRepo.CreateProject(project).Result;
			}).ContinueWith(
				previousTask =>
				{
					if (previousTask.Exception is not null)
					{
						// DataAccessExceptionHandlerUtils.Handle(previousTask.Exception);
						logger.Log(LogLevel.Error, previousTask.Exception,
							$"[GENIA] [Delete] {previousTask.Exception.Message}");
						throw new GeniaGenericException(
							string.Empty,
							TransKeys.DATA_LAYER_GENERIC_ERROR);
					}

					return previousTask.Result;
				}, TaskContinuationOptions.OnlyOnFaulted);
	}
	public Task<Project> Delete(Project project)
	{
		return productService.DeleteAll(project.Products)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
				{
					var inner = task.Exception.InnerException;
					logger.Log(LogLevel.Error, inner,
						$"[GENIA] [DeleteAll] {inner.Message}");
					ErrorHandler.Handle(inner);
				}

				return projectRepo.DeleteProject(project.Id)
					.ContinueWith((previousTask) =>
					{
						if (previousTask.Exception is not null)
						{
							DataAccessExceptionHandlerUtils.Handle(previousTask.Exception);
							logger.Log(LogLevel.Error, previousTask.Exception,
								$"[GENIA] [Delete] {previousTask.Exception.Message}");
							throw new GeniaGenericException(
								string.Empty,
								TransKeys.DATA_LAYER_GENERIC_ERROR);
						}

						return previousTask.Result;
					}).Result;
			});
	}
}