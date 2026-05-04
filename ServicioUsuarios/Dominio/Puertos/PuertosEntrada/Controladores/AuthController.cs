using System.Security.Claims;
using System.Text.Json;
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
        private readonly IUsuarioService _usuarioService;

        public AuthController(
            IAuthService authService,
            IUsuarioTokenService usuarioTokenService,
            IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioTokenService = usuarioTokenService;
            _usuarioService = usuarioService;
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

        [HttpGet("validar-activacion")]
        public IActionResult ValidarActivacionCuenta([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(new { mensaje = "El token es requerido." });

            Result resultado = _usuarioService.ValidarActivacionCuenta(token);

            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Token válido. Puede proceder a activar su cuenta." });
        }

        [HttpPost("activar-cuenta")]
        public IActionResult ActivarCuenta([FromBody] JsonElement body)
        {
            if (!body.TryGetProperty("token", out JsonElement tokenElement) || string.IsNullOrWhiteSpace(tokenElement.GetString()))
                return BadRequest(new { mensaje = "El token es requerido." });

            if (!body.TryGetProperty("nuevaPassword", out JsonElement passwordElement) || string.IsNullOrWhiteSpace(passwordElement.GetString()))
                return BadRequest(new { mensaje = "La nueva contraseña es requerida." });

            string token = tokenElement.GetString()!;
            string nuevaPassword = passwordElement.GetString()!;

            Result resultado = _usuarioService.ActivarCuenta(token, nuevaPassword);

            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Cuenta activada correctamente. Ya puede iniciar sesión." });
        }

        [Authorize]
        [HttpGet("usuario-actual")]
        public IActionResult ObtenerUsuarioActual()
        {
            string? idUsuarioValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioValue, out int idUsuario))
                return Unauthorized(new { mensaje = "Token invalido." });

            var usuario = _usuarioService.ObtenerUsuarioPorId(idUsuario);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }

        [HttpGet("verificar-email")]
        public IActionResult VerificarEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { mensaje = "El email es requerido." });

            var usuario = _usuarioService.ObtenerUsuarioPorEmail(email);
            bool existe = usuario != null;

            return Ok(new { existe, mensaje = existe ? "El email ya está registrado" : "El email está disponible" });
        }

        [HttpGet("verificar-username")]
        public IActionResult VerificarUserName([FromQuery] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest(new { mensaje = "El nombre de usuario es requerido." });

            var usuario = _usuarioService.ObtenerUsuarioPorUserName(userName);
            bool existe = usuario != null;

            return Ok(new { existe, mensaje = existe ? "El nombre de usuario ya está en uso" : "El nombre de usuario está disponible" });
        }
    }
}