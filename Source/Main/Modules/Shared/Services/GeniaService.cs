// <copyright file="GeniaService.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Linq.Dynamic.Core;
using GeniaWebApp.Source.Main.Data.Config;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Shared.Services;

public partial class GeniaService
{
	private readonly GeniaContext context;

	private readonly NavigationManager navigationManager;

	public GeniaService(GeniaContext context, NavigationManager navigationManager)
	{
		this.context = context;
		this.navigationManager = navigationManager;
	}

	private GeniaContext Context
	{
		get { return this.context; }
	}

	public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
		.ForEach(e => e.State = EntityState.Detached);

	public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
	{
		if (query != null)
		{
			if (!string.IsNullOrEmpty(query.Filter))
			{
				if (query.FilterParameters != null)
				{
					items = items.Where(query.Filter, query.FilterParameters);
				}
				else
				{
					items = items.Where(query.Filter);
				}
			}

			if (!string.IsNullOrEmpty(query.OrderBy))
			{
				items = items.OrderBy(query.OrderBy);
			}

			if (query.Skip.HasValue)
			{
				items = DynamicQueryableExtensions.Skip(items, query.Skip.Value) as IQueryable<T>;
			}

			if (query.Top.HasValue)
			{
				items = DynamicQueryableExtensions.Take(items, query.Top.Value) as IQueryable<T>;
			}
		}
	}
}