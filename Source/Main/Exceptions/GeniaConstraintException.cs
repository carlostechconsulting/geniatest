// <copyright file="GeniaConstraintException.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

namespace GeniaWebApp.Source.Main.Exceptions;

public class GeniaConstraintException : GeniaGenericException
{
	public GeniaConstraintException(string message)
		: base(message)
	{
	}

	public GeniaConstraintException(string message, string transKey)
		: base(message)
	{
		TransKey = transKey;
	}

	public GeniaConstraintException(string message, Exception inner)
		: base(message, inner)
	{
	}
}