using ServicioUsuarios.CasosDeUso.Interactores;
using ServicioUsuarios.CasosDeUso.PuertosEntrada;
using ServicioUsuarios.CasosDeUso.PuertosSalida;
using ServicioUsuarios.FrameworksYDrivers.ServiciosExternos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI
builder.Services.AddOpenApi();

// Inyección de dependencias - Módulo Email
builder.Services.AddScoped<IEmailInputPort, EmailInteractor>();
builder.Services.AddScoped<IEmailOutputPort, SmtpEmailSender>();

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