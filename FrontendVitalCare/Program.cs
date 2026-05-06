using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.VentasDtos;
using FrontendVitalCare.Services;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<ClienteApiAdapter>(client =>
{
    string baseUrl = builder.Configuration["ApiUrls:ServicioVentas"]!;
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<AdapterJSON<MedicamentoDto>>(cliente => ///Api
{
    cliente.BaseAddress = new Uri(builder.Configuration["ApiUrls:ServicioVentas"]!);
});

builder.Services.AddScoped<MedicamentoAdapter>(sp =>
{
    var adapterJson = sp.GetRequiredService<AdapterJSON<MedicamentoDto>>();
    return new MedicamentoAdapter(adapterJson);
});

// Registrar AdapterJSON para Ventas
builder.Services.AddHttpClient<AdapterJSON<VentaDto>>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:ServicioVentas"] ?? throw new InvalidOperationException("ApiUrls:ServicioVentas missing"));
});

// Registrar VentaClient
builder.Services.AddScoped<VentaClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
