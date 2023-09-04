using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shaker.Client;
using Shaker.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CocktailService>();
builder.Services.AddScoped<BarService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddSingleton<IRepository, BlobRepository>();
builder.Services.AddSingleton<RandomCocktailService>();
builder.Services.AddScoped<CocktailsStateService>();
builder.Services.AddScoped<BarStateService>();

var host = builder.Build();

var dataService = host.Services.GetRequiredService<DataService>();
await dataService.SetCocktailsAndIngredientsCache();
await host.RunAsync();