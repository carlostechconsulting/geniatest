// <copyright file="ToolTipDefaultConfig.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils;

public static class ToolTipDefaultConfig
{
	private static readonly TooltipOptions TooltipOptions = new()
	{
		Delay = 0,
		Position = TooltipPosition.Bottom,
		Duration = 2000,
		Style = "background-color: gray",
	};

	public static TooltipOptions GetTooltipOptions()
	{
		return TooltipOptions;
	}

	public static TooltipOptions GetTooltipOptions(TooltipPosition potition)
	{
		return new()
		{
			Delay = 0,
			Position = potition,
			Duration = 1000,
			Style = "background-color: gray",
		};
	}
}