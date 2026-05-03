using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.App.DTOs;
using ServicioUsuarios.App.Interfaces;
using ServicioUsuarios.Dominio.Validadores;

namespace ServicioUsuarios.Dominio.Puertos.PuertosEntrada.Controladores
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioTokenService _usuarioTokenService;

        public AuthController(IAuthService authService, IUsuarioTokenService usuarioTokenService)
        {
            _authService = authService;
            _usuarioTokenService = usuarioTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginRequestDto dto)
        {
            Result resultado = _authService.IniciarSesion(dto, out UsuarioLoginResponseDto? respuesta);

            if (!resultado.IsSuccess || respuesta == null)
                return Unauthorized(new { mensaje = resultado.Error });

            return Ok(new { token = respuesta.Token });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string? idUsuarioValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioValue, out int idUsuario))
                return Unauthorized(new { mensaje = "Token invalido." });

            Result resultado = _usuarioTokenService.RevocarTokensActivos(idUsuario, "INICIO_SESION");
            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Sesion cerrada correctamente." });
        }
    }
}
