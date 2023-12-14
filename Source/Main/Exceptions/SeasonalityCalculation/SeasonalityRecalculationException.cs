// <copyright file="ProductSharesRecalculationException.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

namespace GeniaWebApp.Source.Main.Exceptions.SeasonalityCalculation;

public class SeasonalityRecalculationException : GeniaGenericException
{
	public SeasonalityRecalculationException(string message)
		: base(message)
	{
	}

	public SeasonalityRecalculationException(string message, string transKey)
		: base(message, transKey)
	{
	}

	public SeasonalityRecalculationException(string message, Exception inner)
		: base(message, inner)
	{
	}
}