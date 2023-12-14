// <copyright file="TremTipoService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Exceptions;
using GeniaWebApp.Source.Main.Modules.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Modules.TremTipoConfig.Services;

public class TremTipoService
{
	public TremTipoService(TremTipoRepo tremTipoRepo, ILogger<TremTipoService> logger)
	{
		TremTipoRepo = tremTipoRepo;
		Logger = logger;
	}

	private TremTipoRepo TremTipoRepo { get; set; }

	private ILogger<TremTipoService> Logger { get; set; }

	private DiscordNotifier DiscordNotifier { get; set; }

	public async Task Delete(TremTipo tremTipo)
	{
		try
		{
			await TremTipoRepo.DeleteTremTipo(tremTipo.Id.Value);
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

	public Task<TremTipo> GetById(long? id)
	{
		return TremTipoRepo.GetTremTipoById(id);
	}

	public async Task<TremTipo> Create(TremTipo tremTipo)
	{
		try
		{
			var byName = TremTipoRepo.GetByName(tremTipo.Name);
			if (byName is not null)
			{
				Logger.Log(
					LogLevel.Information,
					$"[GENIA] [Create] [Constraint] Transporter's (id: {byName.Id}) name should be unique");
				throw new GeniaConstraintException(
					"Trem Tipo's name should be unique",
					TransKeys.TREM_TIPO_UNIQUE_NAME);
			}

			return await TremTipoRepo.CreateTremTipo(tremTipo);
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

	public async Task<TremTipo> Update(TremTipo tremTipo)
	{
		try
		{
			var byName = TremTipoRepo.GetByName(tremTipo.Name);
			if (byName is not null && byName.Id != tremTipo.Id)
			{
				Logger.Log(
					LogLevel.Information,
					$"[GENIA] [Update] [Constraint] Transporter's (id: {byName.Id}) name should be unique");
				throw new GeniaConstraintException(
					"Product config name should be unique",
					TransKeys.PRODUCT_CONFIG_UNIQUE_NAME);
			}

			return await TremTipoRepo.UpdateTremTipo(tremTipo.Id, tremTipo);
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