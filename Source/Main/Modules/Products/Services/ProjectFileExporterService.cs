using System.Text;
using GeniaWebApp.Source.Main.Data.Models.Genia;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace GeniaWebApp.Source.Main.Modules.Products.Services;

public class ProjectFileExporterService
{
	private readonly IJSRuntime JSRuntime;

	public ProjectFileExporterService(IJSRuntime JSRuntime)
	{
		this.JSRuntime = JSRuntime;
	}

	/// <summary>
	/// Call JS function to download file
	/// </summary>
	/// <param name="data"></param>
	/// <param name="fileName"></param>
	public async Task ExportJson(Project data, string fileName)
	{
		var jsonString = JsonConvert.SerializeObject(BuildObject(data));
		var binaryData = Encoding.UTF8.GetBytes(jsonString);
		var fileStream = new MemoryStream(binaryData);
		var name = $"{fileName}.json";

		using var streamRef = new DotNetStreamReference(stream: fileStream);
		await JSRuntime.InvokeVoidAsync("downloadFileFromStream",
			name,
			streamRef);
	}

	/// <summary>
	/// Build object to export
	/// </summary>
	/// <param name="project"></param>
	/// <returns></returns>
	private object BuildObject(Project project)
	{
		//TODO: build object to export
		return new object();
	}
}