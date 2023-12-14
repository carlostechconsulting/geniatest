// <copyright file="ModalService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Projects.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Services;

namespace GeniaWebApp.Source.Main.Modules.Modals.Services;

public class ModalService
{
	private ILogger<ProjectService> logger;
	private DiscordNotifier discordNotifier;

	private ModalRepo modalRepo;

	public ModalService(ILogger<ProjectService> logger, DiscordNotifier discordNotifier, ModalRepo modalRepo)
	{
		this.logger = logger;
		this.discordNotifier = discordNotifier;
		this.modalRepo = modalRepo;
	}

	/// <summary>
	/// Find all by query.
	/// </summary>
	/// <param name="modals"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public Task DeleteAll(ICollection<Modal> modals)
	{
		return Task.WhenAll(modals.Select(Delete));
	}

	public Task Delete(Modal modal)
	{
		return modalRepo.DeleteModal(modal.Id)
			.ContinueWith(task =>
			{
				if (task.Exception is null)
				{
					return task;
				}

				logger.Log(LogLevel.Error, task.Exception,
					$"[GENIA] [Delete] {task.Exception.Message}");
				throw new GeniaGenericException(string.Empty);
			});
	}

	public Task<Modal> Create(Modal modal)
	{
		//TODO: review how to do it better: it is throwing constraint error related to unique product PK
		modal.Product = null;
		return modalRepo.CreateModal(modal)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
					throw new GeniaConstraintException(
						"Error when trying to create Modal");
				return task.Result;
			});
	}

	public Task<Modal> Update(Modal modal)
	{
		return modalRepo.UpdateModal(modal.Id, modal)
			.ContinueWith(task =>
			{
				if (task.Exception is not null)
					throw new GeniaConstraintException(
						"Error when trying to create Modal", task.Exception.InnerException);
				return task.Result;
			});
	}
}