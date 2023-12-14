// <copyright file="ExportGeniaController.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Modules.Shared.Services;

namespace GeniaWebApp.Source.Main.Modules.Controllers;

public partial class ExportGeniaController : ExportController
{
	private readonly GeniaContext context;
	private readonly GeniaService service;

	public ExportGeniaController(GeniaContext context, GeniaService service)
	{
		this.service = service;
		this.context = context;
	}

	// [HttpGet("/export/Genia/dimensions/csv")]
	// [HttpGet("/export/Genia/dimensions/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportDimensionsToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetDimensions(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/dimensions/excel")]
	// [HttpGet("/export/Genia/dimensions/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportDimensionsToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetDimensions(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/modals/csv")]
	// [HttpGet("/export/Genia/modals/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportModalsToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetModals(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/modals/excel")]
	// [HttpGet("/export/Genia/modals/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportModalsToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetModals(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/products/csv")]
	// [HttpGet("/export/Genia/products/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProductsToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/products/excel")]
	// [HttpGet("/export/Genia/products/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProductsToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/productconfigs/csv")]
	// [HttpGet("/export/Genia/productconfigs/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProductConfigsToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetProductConfigs(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/productconfigs/excel")]
	// [HttpGet("/export/Genia/productconfigs/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProductConfigsToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetProductConfigs(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/projects/csv")]
	// [HttpGet("/export/Genia/projects/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProjectsToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetProjects(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/projects/excel")]
	// [HttpGet("/export/Genia/projects/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportProjectsToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetProjects(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/seasonalities/csv")]
	// [HttpGet("/export/Genia/seasonalities/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportSeasonalitiesToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetSeasonalities(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/seasonalities/excel")]
	// [HttpGet("/export/Genia/seasonalities/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportSeasonalitiesToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetSeasonalities(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/seasonalitymonthdata/csv")]
	// [HttpGet("/export/Genia/seasonalitymonthdata/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportSeasonalityMonthDataToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetSeasonalityMonthData(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/seasonalitymonthdata/excel")]
	// [HttpGet("/export/Genia/seasonalitymonthdata/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportSeasonalityMonthDataToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetSeasonalityMonthData(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/tremtipos/csv")]
	// [HttpGet("/export/Genia/tremtipos/csv(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportTremTiposToCSV(string fileName = null)
	// {
	//     return ToCSV(ApplyQuery(await service.GetTremTipos(), Request.Query), fileName);
	// }
	//
	// [HttpGet("/export/Genia/tremtipos/excel")]
	// [HttpGet("/export/Genia/tremtipos/excel(fileName='{fileName}')")]
	// public async Task<FileStreamResult> ExportTremTiposToExcel(string fileName = null)
	// {
	//     return ToExcel(ApplyQuery(await service.GetTremTipos(), Request.Query), fileName);
	// }
}