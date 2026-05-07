using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Adaptadores.Auth;
using FrontendVitalCare.Adaptadores.Reportes;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Auth;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.VentasDtos;
using FrontendVitalCare.Dto.ClasificacionDtos;
using FrontendVitalCare.Services;
using FrontendVitalCare.Servicios;
using VitalCareSBA.FrontendVitalCare.Adaptadores;


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

// CLIENTES
builder.Services.AddHttpClient<ClienteApiAdapter>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";

    client.BaseAddress = new Uri(baseUrl);
});

// AUTENTICACIÓN / USUARIOS
builder.Services.AddHttpClient<AuthClient>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:Usuarios"]
        ?? builder.Configuration["ApiUrls:ServicioUsuario"]
        ?? "http://localhost:5290/";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IAdapter<JsonElement, UsuarioLoginResponseDto>, LoginResponseAdapter>();
builder.Services.AddScoped<IAdapter<JsonElement, MensajeApiDto>, MensajeApiAdapter>();

// MEDICAMENTOS
builder.Services.AddHttpClient<AdapterJSON<MedicamentoDto>>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<MedicamentoAdapter>(sp =>
{
    var adapterJson = sp.GetRequiredService<AdapterJSON<MedicamentoDto>>();
    return new MedicamentoAdapter(adapterJson);
});

// VENTAS
builder.Services.AddHttpClient<AdapterJSON<VentaDto>>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<VentaClient>();

// CLASIFICACIONES
builder.Services.AddHttpClient<AdapterJSON<ClasificacionDto>>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<ClasificacionAdapter>(sp =>
{
    var adapterJson = sp.GetRequiredService<AdapterJSON<ClasificacionDto>>();
    return new ClasificacionAdapter(adapterJson);
});

// REPORTE DE VENTAS POR ROL
builder.Services.AddHttpClient<ReporteVentasPorRolAdapter>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";

    client.BaseAddress = new Uri(baseUrl);
});

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