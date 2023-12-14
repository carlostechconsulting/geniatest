// <copyright file="DiscordNotifier.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Text;
using GeniaWebApp.Source.Main.Data.Models.Genia.EnumTypes;
using GeniaWebApp.Source.Main.Data.Repository;
using Newtonsoft.Json;

namespace GeniaWebApp.Source.Main.Modules.Shared.Services;

public class DiscordNotifier
{
	private AppFeatureToggleRepo appFeatureToggleRepo;

	public DiscordNotifier(HttpClient httpClient, AppFeatureToggleRepo appFeatureToggleRepo)
	{
		HttpClient = httpClient;
		this.appFeatureToggleRepo = appFeatureToggleRepo;
	}

	private HttpClient HttpClient { get; set; }

	public Task PublishException(Exception exception)
	{
		return appFeatureToggleRepo.GetByKey(FeatureToggleKey.DISCORD_LOG)
			.ContinueWith(task =>
			{
				if (task.Result?.Value == false && exception is not null)
				{
					return Task.CompletedTask;
				}

				var errorMessage = $"Message: {exception.Message} \n StackTrace: {exception.StackTrace}";
				var message = DiscordMessage.BuildMessage("Genia - Exception notifier.", errorMessage);

				return Publish(message, exception);
			});
	}

	private async Task Publish(DiscordMessage message, Exception exception = null)
	{
		var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
		try
		{
			await HttpClient
				.PostAsync(
					"https://discord.com/api/webhooks/1166750809963376773/e88V6XOy0rkzmQr-64WtUX3b1lBnxEtfeRm6oQZc_aQMIEKjPMlOop9N1BS7bsuAhWss",
					content);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public class DiscordMessage
	{
		[JsonProperty("embeds")] public List<DiscordEmbed> Embeds { get; set; }

		public static DiscordMessage BuildMessage(string title, string description)
		{
			return new DiscordMessage()
			{
				Embeds = new List<DiscordEmbed>()
				{
					new()
					{
						Title = title,
						Description = description,
						Autor = new DiscordAuthor(),
					},
				},
			};
		}

		public class DiscordEmbed
		{
			[JsonProperty("title")] public string Title { get; set; } = "Genia Web App Tracker";

			[JsonProperty("description")] public string Description { get; set; }

			[JsonProperty("color")] public string Color { get; set; } = "16763480";

			[JsonProperty("autor")] public DiscordAuthor Autor { get; set; }
		}

		public class DiscordAuthor
		{
			[JsonProperty("name")] public string Name { get; set; } = "Genia Web App Tracker";

			[JsonProperty("url")] public string Url { get; set; } = string.Empty;

			[JsonProperty("icon_url")] public string IconUrl { get; set; } = string.Empty;
		}
	}
}