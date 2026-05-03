using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicioUsuarios.App.Servicios;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Dominio.Validadores;
using ServicioUsuarios.Infraestructura.Ayudadores;
using ServicioUsuarios.Infraestructura.Creadores;
using ServicioUsuarios.Infraestructura.Persistencia.Repositorios;
using ServicioUsuarios.Infraestructura.Persistencia.Conexion;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 Controllers
builder.Services.AddControllers();

// 🔥 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 🔥 CONEXIÓN
builder.Services.AddSingleton<ConexionStringSingleton>();

// 🔥 DI PRINCIPAL (MEJORADO)
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<UsuarioRepositorioCreator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<LoginValidador>();

// 🔥 TOKEN (MIGRACIÓN FARMACIA)
builder.Services.AddScoped<IUsuarioTokenRepositorio, UsuarioTokenRepositorio>();
builder.Services.AddScoped<UsuarioTokenService>();

// 🔥 JWT
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        )
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// 🔥 Swagger SOLO EN DEV
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔥 Middleware
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();