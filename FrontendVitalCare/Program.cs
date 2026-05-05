using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<MedicamentoClient>(cliente =>
{
    cliente.BaseAddress = new Uri(builder.Configuration["ApiUrls:Medicamentos"]!);
    cliente.BaseAddress = new Uri(builder.Configuration["ApiUrls:Clasificacion  "]!);
});
builder.Services.AddScoped<IAdapter<JsonElement, MedicamentoDto>, MedicamentoAdapter>();

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
