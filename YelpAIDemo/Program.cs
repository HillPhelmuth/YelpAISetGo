using Blazored.LocalStorage;
using YelpAIDemo.Components;
using YelpAIDemo.Core.Helpers;
using YelpAIDemo.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddRazorComponents()
    .AddInteractiveServerComponents();
services.AddSignalR(o =>
{
    o.MaximumReceiveMessageSize = null;
});
services.AddHttpClient();
services.AddYelpAIServices();
services.AddBlazoredLocalStorage();
services.AddScoped<BrowserStorageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
