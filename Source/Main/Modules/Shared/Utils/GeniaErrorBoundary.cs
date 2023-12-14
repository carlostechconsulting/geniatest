// <copyright file="GeniaErrorBoundary.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using Microsoft.AspNetCore.Components.Web;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils;

public class GeniaErrorBoundary : ErrorBoundary
{
	public new Exception? CurrentException => base.CurrentException;
}