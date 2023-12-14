using Microsoft.AspNetCore.Components;
using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Shared.Services;

public class LoadingDialog
{
	private DialogService DialogService;

	public LoadingDialog(DialogService dialogService)
	{
		DialogService = dialogService;
	}


	public Task<dynamic> OpenLoading(RenderFragment<DialogService> childContent)
	{
		return DialogService.OpenAsync("", childContent, new DialogOptions()
		{
			CloseDialogOnOverlayClick = false,
			CloseDialogOnEsc = false,
			ShowClose = false,
		});
	}

	public void CloseLoading()
	{
		DialogService.Close();
	}
}