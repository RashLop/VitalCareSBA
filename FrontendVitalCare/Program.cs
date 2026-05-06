using System.Text.Json;
using FrontendVitalCare.Dto.MedicamentoDtos;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<ClienteApiAdapter>(client =>
{
    string baseUrl = builder.Configuration["Servicios:VentasBaseUrl"] ?? "http://localhost:5080";
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
