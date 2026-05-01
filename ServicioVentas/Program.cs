using ServicioVentas.FrameworksYDrivers.Repositorios;
using ServicioVentas.FrameworksYDrivers.Data;
using ServicioVentas.Entidades;
using ServicioVentas.CasosDeUso.Validadores;
using ServicioVentas.CasosDeUso.PuertosSalida;
using ServicioVentas.CasosDeUso.PuertosEntrada;
using ServicioVentas.CasosDeUso.Interactores;
using ServicioVentas.AdaptadoresDeInterfaz.Presentadores;
using ServicioVentas.AdaptadoresDeInterfaz.Gateways;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ConexionString>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IResult<Cliente>, ClienteValidacion>();
builder.Services.AddScoped<IClienteOutputPort, ClientePresenter>();
builder.Services.AddScoped<IClienteInputPort, ClienteInteractor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

