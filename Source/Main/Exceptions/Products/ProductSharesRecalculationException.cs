// <copyright file="ProductSharesRecalculationException.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

namespace GeniaWebApp.Source.Main.Exceptions.Products;

public class ProductSharesRecalculationException : GeniaGenericException
{
	public ProductSharesRecalculationException(string message)
		: base(message)
	{
	}

	public ProductSharesRecalculationException(string message, string transKey)
		: base(message, transKey)
	{
	}

	public ProductSharesRecalculationException(string message, Exception inner)
		: base(message, inner)
	{
	}
}