using ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using ServicioVentas.CasosDeUso.Interactores;
using ServicioVentas.CasosDeUso.PuertosEntrada;
using ServicioVentas.CasosDeUso.Validadores;
using ServicioVentas.Entidades;
using ServicioVentas.FrameworksYDrivers.Creadores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ClienteRepositoryCreator>();
builder.Services.AddScoped<IClienteRepository>(provider =>
{
    var creator = provider.GetRequiredService<ClienteRepositoryCreator>();
    return creator.CreateClienteRepo();
});
builder.Services.AddScoped<IResult<Cliente>, ClienteValidacion>();
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
