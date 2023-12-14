// <copyright file="GeniaGenericException.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Config.Trans;

namespace GeniaWebApp.Source.Main.Exceptions;

public class GeniaGenericException : Exception
{
	public GeniaGenericException(string message)
		: base(message)
	{
	}

	public GeniaGenericException(string? message, string? transKey)
		: base(message)
	{
		TransKey = transKey;
	}

	public GeniaGenericException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public string TransKey { get; set; } = TransKeys.DATA_LAYER_GENERIC_ERROR;
}