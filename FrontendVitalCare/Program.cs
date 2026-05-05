using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Adaptadores.Auth;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Auth;
using FrontendVitalCare.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<ClienteApiAdapter>(client =>
{
    string baseUrl = builder.Configuration["Servicios:VentasBaseUrl"] ?? "http://localhost:5080";
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddHttpClient<AuthClient>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:Usuarios"] ?? "http://localhost:5290/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IAdapter<JsonElement, UsuarioLoginResponseDto>, LoginResponseAdapter>();
builder.Services.AddScoped<IAdapter<JsonElement, MensajeApiDto>, MensajeApiAdapter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
