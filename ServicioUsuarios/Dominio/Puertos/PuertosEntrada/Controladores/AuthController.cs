using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.App.Servicios;
using ServicioUsuarios.App.DTOs;

namespace ServicioUsuarios.Dominio.Puertos.PuertosEntrada.Controladores
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService service;

        public AuthController(AuthService service)
        {
            this.service = service;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = service.Login(request.Email, request.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }
    }
}