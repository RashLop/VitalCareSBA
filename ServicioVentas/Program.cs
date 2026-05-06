using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.CasosDeUso.Interactores;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores;
using VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosSalida;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
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

builder.Services.AddScoped<IVentaFacade, VentaFacade>();
builder.Services.AddScoped<FachadaVenta>();
builder.Services.AddScoped<FachadaAnular>();
builder.Services.AddScoped<FachadaActualizarStock>();

builder.Services.AddScoped<IVentaInputPort, VentaInteractor>();
builder.Services.AddScoped<IVentaOutputPort, VentaRepository>();

builder.Services.AddScoped<IResult<Venta>, VentaValidacion>();
// CLASIFICACION

builder.Services.AddScoped<IResult<Clasificacion>, ClasificacionValidacion>();
builder.Services.AddScoped<IClasificacionInputPort, ClasificacionInteractor>();

builder.Services.AddScoped<ClasificacionRepositoryCreator>();
builder.Services.AddScoped<IClasificacionRepository>(provider =>
{
    var creator = provider.GetRequiredService<ClasificacionRepositoryCreator>();
    return creator.CreateClasificacionRepo();
});

//CORS para el frontend 🔥
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5051") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

//activar cors...
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
