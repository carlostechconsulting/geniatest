// <copyright file="ExportController.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.AspNetCore.Mvc;

namespace GeniaWebApp.Source.Main.Modules.Controllers;

public class OutputExporter : Controller
{
	[HttpPost("/export/project-export")]
	public async Task<FileStreamResult> ExportProject([FromBody]Project project)
	{
		var result = new FileStreamResult(
			new MemoryStream(UTF8Encoding.Default.GetBytes(
				"{}")),
			"application/json");
		result.FileDownloadName = $"{project.Name}-{DateTime.Now.ToString()}" + ".json";

		return result;
	}

	[HttpGet("/export/hello")]
	public async Task<FileStreamResult> Hello()
	{
		var result = new FileStreamResult(
			new MemoryStream(UTF8Encoding.Default.GetBytes(
				"{}")),
			"application/json");
		result.FileDownloadName = $"helooo-{DateTime.Now.ToString()}" + ".json";

		return result;
	}

	public FileStreamResult Export(Project project)
	{
		var result = new FileStreamResult(
			new MemoryStream(UTF8Encoding.Default.GetBytes(
				"{}")),
			"application/json");
		result.FileDownloadName = $"{project.Name}-{DateTime.Now.ToString()}" + ".json";

		return result;
	}
}