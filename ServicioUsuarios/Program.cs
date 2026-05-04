using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicioUsuarios.App.Interfaces;
using ServicioUsuarios.App.Servicios;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Creadores;
using ServicioUsuarios.Dominio.Validadores; // ? Agregar para los validadores
using ServicioUsuarios.App.DTOs; // ? Agregar para los DTOs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ServicioUsuarios API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT como: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("No se encontro Jwt:Key en la configuracion.");
string jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("No se encontro Jwt:Issuer en la configuracion.");
string jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("No se encontro Jwt:Audience en la configuracion.");

Environment.SetEnvironmentVariable("JWT_KEY", jwtKey);
Environment.SetEnvironmentVariable("JWT_ISSUER", jwtIssuer);
Environment.SetEnvironmentVariable("JWT_AUDIENCE", jwtAudience);

builder.Services.AddScoped<UsuarioRepositoryCreator>();
builder.Services.AddScoped<UsuarioTokenRepositoryCreator>();

builder.Services.AddScoped<IUsuarioRepository>(provider =>
{
    UsuarioRepositoryCreator creator = provider.GetRequiredService<UsuarioRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IUsuarioTokenRepository>(provider =>
{
    UsuarioTokenRepositoryCreator creator = provider.GetRequiredService<UsuarioTokenRepositoryCreator>();
    return creator.CreateRepo();
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsuarioTokenService, UsuarioTokenService>();

// ? REGISTRAR IUsuarioService
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// ? REGISTRAR VALIDADORES (necesarios para UsuarioService)
builder.Services.AddScoped<IResult<UsuarioRegistroDto>, UsuarioRegistroValidacion>();
builder.Services.AddScoped<IResult<UsuarioActualizacionDto>, UsuarioActualizacionValidacion>();
builder.Services.AddScoped<UsuarioNegocioValidacion>();

// ? REGISTRAR EMAIL SERVICE (si no está registrado)
builder.Services.AddScoped<IEmailService, EmailService>(); // Ajusta según tu implementación real

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();