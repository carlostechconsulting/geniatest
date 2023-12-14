// <copyright file="ErrorHandler.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Data.Common;
using GeniaWebApp.Source.Main.Exceptions;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils;

public static class ErrorHandler
{
	public static async Task Handle(Exception? exception)
	{
		if (exception is DbException)
		{
			throw new GeniaGenericException(string.Empty);
		}

		// TODO: implement error handling
		return;
	}
}