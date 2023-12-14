// <copyright file="ActionDialogsUtils.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using Radzen;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils.Dialogs;

public static class ActionDialogsUtils
{
	public static readonly string CONFIRMATION_DIALOG_TITLE_TRANS_KEY = "dialogConfirmationTitle";

	public static readonly DialogOptions DIALOG_OPTIONS = new()
	{
		CloseDialogOnOverlayClick = false,
		CloseDialogOnEsc = false,
		ShowClose = false,
	};
}