// <copyright file="GenericRepo.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Linq.Dynamic.Core;
using Radzen;

namespace GeniaWebApp.Source.Main.Data.Repository;

public class GenericRepo
{
	protected void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
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