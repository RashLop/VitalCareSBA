using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Adaptadores.Auth;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Auth;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.VentasDtos;
using FrontendVitalCare.Dto.ClasificacionDtos;
using FrontendVitalCare.Services;
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
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddHttpClient<AuthClient>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:Usuarios"]
        ?? builder.Configuration["ApiUrls:ServicioUsuario"]
        ?? "http://localhost:5290/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IAdapter<JsonElement, UsuarioLoginResponseDto>, LoginResponseAdapter>();
builder.Services.AddScoped<IAdapter<JsonElement, MensajeApiDto>, MensajeApiAdapter>();

builder.Services.AddHttpClient<AdapterJSON<MedicamentoDto>>(cliente => ///Api
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? "http://localhost:5080/";
    cliente.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<MedicamentoAdapter>(sp =>
{
    var adapterJson = sp.GetRequiredService<AdapterJSON<MedicamentoDto>>();
    return new MedicamentoAdapter(adapterJson);
});

// Registrar AdapterJSON para Ventas
builder.Services.AddHttpClient<AdapterJSON<VentaDto>>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]
        ?? builder.Configuration["Servicios:VentasBaseUrl"]
        ?? throw new InvalidOperationException("No se encontro la URL de ServicioVentas.");
    client.BaseAddress = new Uri(baseUrl);
});

// Registrar VentaClient
builder.Services.AddScoped<VentaClient>();

// Registrar AdapterJSON para Clasificaciones
builder.Services.AddHttpClient<AdapterJSON<ClasificacionDto>>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:ServicioVentas"] ?? throw new InvalidOperationException("ApiUrls:ServicioVentas missing"));
});

// Registrar ClasificacionAdapter
builder.Services.AddScoped<ClasificacionAdapter>(sp =>
{
    var adapterJson = sp.GetRequiredService<AdapterJSON<ClasificacionDto>>();
    return new ClasificacionAdapter(adapterJson);
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
