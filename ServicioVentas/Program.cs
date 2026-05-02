using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.CasosDeUso.Interactores;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores;

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


builder.Services.AddScoped<MedicamentoRepositoryCreator>();
builder.Services.AddScoped<IMedicamentoRepository>(provider =>
{
    var creator = provider.GetRequiredService<MedicamentoRepositoryCreator>();
    return creator.CreateMedicamentoRepo();
});
builder.Services.AddScoped<IResult<Medicamento>, MedicamentoValidacion>();
builder.Services.AddScoped<IMedicamentoInputPort, MedicamentoService>();


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
