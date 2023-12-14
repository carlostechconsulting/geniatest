// <copyright file="DataAccessExceptionHandlerUtils.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils;

public class DataAccessExceptionHandlerUtils
{
	public static void Handle(AggregateException aggregateException)
	{
		if (aggregateException is not null)
		{
			foreach (var innerException in aggregateException.InnerExceptions)
			{
				if (innerException is DbUpdateException)
				{
				}
			}
		}
	}
}