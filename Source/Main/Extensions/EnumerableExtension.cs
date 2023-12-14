// <copyright file="EnumerableExtension.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

namespace GeniaWebApp.Extentions;

public static class EnumerableExtension
{
	public static bool HasAny<T>(this IEnumerable<T> data)
	{
		return data?.Any() ?? false;
	}

	public static bool Empty<T>(this IEnumerable<T> data)
	{
		return !HasAny(data);
	}

	public static void IfHasAny<T>(this IEnumerable<T> data, Action<IEnumerable<T>> action)
	{
		if (data.HasAny())
			action.Invoke(data);
	}

	public static IEnumerable<T> OrElseEmpty<T>(this IEnumerable<T> data)
	{
		return data.HasAny() ? data : new List<T>();
	}
}