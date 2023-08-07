using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shaker.Client;
using Shaker.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CocktailService>();
builder.Services.AddScoped<BarService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddSingleton(_ => new DataService("DefaultEndpointsProtocol=https;AccountName=myshaker;AccountKey=l4MNuLavCOal6Y5HnkabSYibXo3n6M2GuoWlAJ1CfZrvkL9pF12BAxAiUT7GOl3Vg+y1KAjChyCq+AStLid1UQ==;EndpointSuffix=core.windows.net"));

var host = builder.Build();
var dataService = host.Services.GetRequiredService<DataService>();
await dataService.SetCocktailsAndIngredientsCache();
await host.RunAsync();