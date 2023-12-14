// <copyright file="Program.cs" company="LPC Latina">
// Copyright (c) LPC Latina 2024. All rights reserved
// </copyright>

using System.Globalization;
using BlazorPro.BlazorSize;
using GeniaWebApp.Source.Main.Config;
using GeniaWebApp.Source.Main.Data.Config;
using GeniaWebApp.Source.Main.Data.Repository;
using GeniaWebApp.Source.Main.Modules.FeatureToggleConfig.Services;
using GeniaWebApp.Source.Main.Modules.Modals.Services;
using GeniaWebApp.Source.Main.Modules.Products.Services;
using GeniaWebApp.Source.Main.Modules.ProductsConfig.Services;
using GeniaWebApp.Source.Main.Modules.Projects.Services;
using GeniaWebApp.Source.Main.Modules.Seasonalities.Services;
using GeniaWebApp.Source.Main.Modules.Shared.Services;
using GeniaWebApp.Source.Main.Modules.TransporterConfig.Services;
using GeniaWebApp.Source.Main.Modules.TremTipoConfig.Services;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(o => { o.MaximumReceiveMessageSize = 10 * 1024 * 1024; });
builder.Services.AddLocalization();
builder.Services.AddResizeListener();
builder.Services.AddTransient(sp => new HttpClient());
//TODO: organize these declarations better
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<GeniaService>();
builder.Services.AddScoped<ProjectFileExporterService>();

builder.Services.AddScoped<DimensionRepo>();
builder.Services.AddScoped<ProductConfigRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<ProjectRepo>();
builder.Services.AddScoped<SeasonalityRepo>();
builder.Services.AddScoped<TremTipoRepo>();
builder.Services.AddScoped<TransporterRepo>();

builder.Services.AddScoped<ModalRepo>();
builder.Services.AddScoped<AppFeatureToggleRepo>();
builder.Services.AddScoped<ProductConfigService>();
builder.Services.AddScoped<TremTipoService>();
builder.Services.AddScoped<TransporterService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductShareService>();
builder.Services.AddScoped<ProjectShareService>();
builder.Services.AddScoped<ModalService>();
builder.Services.AddScoped<LoadingDialog>();

builder.Services.AddScoped<SeasonalityMonthDataService>();

builder.Services.AddScoped<AppFeatureToggleService>();

builder.Services.AddScoped<DiscordNotifier>();

builder.Services.AddDbContext<GeniaContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("GeniaConnection"));
});
var connectionString = builder.Configuration["ConnectionStrings:GeniaConnection"];
builder.Services.AddTransient(sp => new EnvConfigs()
{
	ConnectionString = connectionString,
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

// var supportedCultures = new[] { "pt", "en" };
// var localizationOptions = new RequestLocalizationOptions()
//     .SetDefaultCulture("pt")
//     .AddSupportedCultures(supportedCultures)
//     .AddSupportedUICultures(supportedCultures);
// CultureInfo cultureInfo = new CultureInfo(appLanguage);
// app.UseRequestLocalization(localizationOptions);

// var jsInterop = builder.Build().Services.GetRequiredService<IJSRuntime>();
// var appLanguage = await jsInterop.InvokeAsync<string>("appCulture.get");
CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("pt");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();