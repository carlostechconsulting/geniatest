// <copyright file="TransporterService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Modules.TransporterConfig.Services;

public class TransporterService
{
	public TransporterService(TransporterRepo transporterRepo, ILogger<TransporterService> logger)
	{
		TransporterRepo = transporterRepo;
		Logger = logger;
	}

	private TransporterRepo TransporterRepo { get; set; }

	private ILogger<TransporterService> Logger { get; set; }

	private DiscordNotifier DiscordNotifier { get; set; }

	public Task<IQueryable<Transporter>> FindAll(Query query = null)
	{
		return TransporterRepo.GetTransporters(query);
	}

	public Task<Transporter> GetById(long? id)
	{
		return TransporterRepo.GetTransporterById(id);
	}

	public async Task Delete(Transporter entity)
	{
		try
		{
			await TransporterRepo.DeleteTransporter(entity.Id.Value);
		}
		catch (DbUpdateException e)
		{
			DiscordNotifier.PublishException(e);
			Logger.Log(LogLevel.Error, e, $"[GENIA] [Create] {e.Message}");
			throw new GeniaGenericException(
				string.Empty,
				TransKeys.DATA_LAYER_GENERIC_ERROR);
		}
	}

	public async Task<Transporter> Create(Transporter entity)
	{
		try
		{
			var byName = TransporterRepo.GetByName(entity.Name);
			if (byName is not null)
			{
				Logger.Log(
					LogLevel.Information,
					$"[GENIA] [Create] [Constraint] Transporter's (id: {byName.Id}) name should be unique");
				throw new GeniaConstraintException(
					"Trem Tipo's name should be unique",
					TransKeys.TREM_TIPO_UNIQUE_NAME);
			}

			return await TransporterRepo.CreateTransporter(entity);
		}
		catch (DbUpdateException e)
		{
			DiscordNotifier.PublishException(e);
			Logger.Log(LogLevel.Error, e, $"[GENIA] [Create] {e.Message}");
			throw new GeniaGenericException(
				string.Empty,
				TransKeys.DATA_LAYER_GENERIC_ERROR);
		}
	}

	public async Task<Transporter> Update(Transporter transporter)
	{
		try
		{
			var byName = TransporterRepo.GetByName(transporter.Name);
			if (byName is not null && byName.Id != transporter.Id)
			{
				Logger.Log(
					LogLevel.Information,
					$"[GENIA] [Update] [Constraint] Transporter's (id: {byName.Id}) name should be unique");
				throw new GeniaConstraintException(
					"Product config name should be unique",
					TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);
			}

			return await TransporterRepo.UpdateTransporter(transporter.Id, transporter);
		}
		catch (DbUpdateException e)
		{
			DiscordNotifier.PublishException(e);
			Logger.Log(LogLevel.Error, e, $"[GENIA] [Update] {e.Message}");
			throw new GeniaGenericException(
				string.Empty,
				TransKeys.DATA_LAYER_GENERIC_ERROR);
		}
	}
}