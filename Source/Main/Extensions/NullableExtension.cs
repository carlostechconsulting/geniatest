// <copyright file="NullableExtension.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

namespace GeniaWebApp.Extentions;

/// <summary>
/// For objects null verification purposes.
/// </summary>
public static class NullableExtension
{
	public delegate T Supplier<out T>();

	/// <summary>
	/// get value or else resolved value.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="defaultValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T? OrElse<T>(this T? nullable, T defaultValue)
		where T : struct
	{
		return nullable ?? defaultValue;
	}

	/// <summary>
	/// get value or else resolved lazy.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="supplier"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T? OrElse<T>(this T? nullable, Supplier<T> supplier)
		where T : struct
	{
		return nullable ?? supplier.Invoke();
	}

	/// <summary>
	/// Verify if is empty.
	/// </summary>
	/// <param name="nullable"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static bool IsEmpty<T>(this T? nullable)
		where T : struct
	{
		return !nullable.HasValue;
	}

	/// <summary>
	/// get value or else throw exception resolved.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="exception"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static T? OrElseThrow<T>(this T? nullable, Exception exception)
		where T : struct
	{
		if (!nullable.HasValue)
		{
			throw exception;
		}

		return nullable;
	}

	/// <summary>
	/// get value or else throw exception resolved lazy.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="action"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T? OrElseThrow<T>(this T? nullable, Action action)
		where T : struct
	{
		if (!nullable.HasValue)
		{
			action.Invoke();
		}

		return nullable;
	}

	/// <summary>
	/// Apply action if has value.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="action"></param>
	/// <typeparam name="T"></typeparam>
	public static void IfHasValueOrElse<T>(this T? nullable, Action<T> action, Action actionElse)
		where T : struct
	{
		if (nullable.HasValue)
		{
			action.Invoke(nullable.Value);
		}
		else
		{
			actionElse.Invoke();
		}
	}


	/// <summary>
	/// Apply action if has value or else.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="action"></param>
	/// <param name="actionElse"></param>
	/// <typeparam name="T"></typeparam>
	public static void IfHasValueElse<T>(this T? nullable, Action<T> action, Action<T> actionElse)
		where T : struct
	{
		if (nullable.HasValue)
		{
			action.Invoke(nullable.Value);
		}
		else
		{
			actionElse.Invoke(nullable.Value);
		}
	}

	/// <summary>
	/// Map value if has value.
	/// </summary>
	/// <param name="nullable"></param>
	/// <param name="action"></param>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="R"></typeparam>
	/// <returns></returns>
	public static R? Map<T, R>(this T? nullable, Func<T, R> action)
		where T : struct
		where R : struct
	{
		return nullable.HasValue ? action.Invoke(nullable.Value) : null;
	}
	

}