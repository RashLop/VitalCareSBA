using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Helpers;

namespace FrontendVitalCare.Servicios
{
    public class UsuarioClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdapter<JsonElement, UsuarioDto> _usuarioAdapter;
        private readonly IAdapter<JsonElement, MensajeApiDto> _mensajeApiAdapter;

        public UsuarioClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IAdapter<JsonElement, UsuarioDto> usuarioAdapter,
            IAdapter<JsonElement, MensajeApiDto> mensajeApiAdapter)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _usuarioAdapter = usuarioAdapter;
            _mensajeApiAdapter = mensajeApiAdapter;
        }

        public async Task<(OperacionApiDto Resultado, List<UsuarioDto> Usuarios)> ObtenerTodosAsync(string filtro)
        {
            ConfigurarAutorizacion();

            HttpResponseMessage response = await _httpClient.GetAsync("api/usuarios/GetUsers");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudieron obtener los usuarios.");
                return (OperacionApiDto.Error(mensajeError), new List<UsuarioDto>());
            }

            JsonElement? json = await LeerJsonAsync(response);
            if (json == null || !json.Value.TryGetProperty("data", out JsonElement dataElement) || dataElement.ValueKind != JsonValueKind.Array)
            {
                return (OperacionApiDto.Error("La respuesta del servidor no contiene usuarios validos."), new List<UsuarioDto>());
            }

            List<UsuarioDto> usuarios = _usuarioAdapter.AdaptList(dataElement.EnumerateArray());
            usuarios = AplicarFiltro(usuarios, filtro);

            string mensaje = await LeerMensajeAsync(response, "Usuarios obtenidos correctamente.");
            return (OperacionApiDto.Ok(mensaje), usuarios);
        }

        public async Task<(OperacionApiDto Resultado, UsuarioDto? Usuario)> ObtenerPorIdAsync(int idUsuario)
        {
            var (resultado, usuarios) = await ObtenerTodosAsync(string.Empty);
            if (!resultado.Exito)
                return (resultado, null);

            UsuarioDto? usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            return usuario == null
                ? (OperacionApiDto.Error("Usuario no encontrado."), null)
                : (OperacionApiDto.Ok("Usuario obtenido correctamente."), usuario);
        }

        public async Task<OperacionApiDto> CrearAsync(UsuarioCreateDto request)
        {
            ConfigurarAutorizacion();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/usuarios/CrearUsuario", request);
            return await LeerResultadoAsync(response, "Usuario registrado correctamente.");
        }

        public async Task<OperacionApiDto> ActualizarAsync(UsuarioUpdateDto request)
        {
            ConfigurarAutorizacion();

            string? idUsuarioSesion = _httpContextAccessor.HttpContext?.Session.GetInt32("IdUsuario")?.ToString();
            string url = string.IsNullOrWhiteSpace(idUsuarioSesion)
                ? "api/usuarios/actualizarUsuario"
                : $"api/usuarios/actualizarUsuario?idUsuarioSesion={Uri.EscapeDataString(idUsuarioSesion)}";

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, request);
            return await LeerResultadoAsync(response, "Usuario actualizado correctamente.");
        }

        public async Task<OperacionApiDto> EliminarAsync(int idUsuario)
        {
            ConfigurarAutorizacion();

            string? idUsuarioSesion = _httpContextAccessor.HttpContext?.Session.GetInt32("IdUsuario")?.ToString();
            if (string.IsNullOrWhiteSpace(idUsuarioSesion))
                return OperacionApiDto.Error("No se pudo identificar el usuario que realiza la operacion.");

            string url = $"api/usuarios/EliminarUsuario?idUsuario={idUsuario}&idUsuarioSesion={Uri.EscapeDataString(idUsuarioSesion)}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            return await LeerResultadoAsync(response, "Usuario eliminado correctamente.");
        }

        private void ConfigurarAutorizacion()
        {
            string? token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        private static List<UsuarioDto> AplicarFiltro(List<UsuarioDto> usuarios, string filtro)
        {
            string filtroLimpio = FiltroHelper.LimpiarFiltro(filtro);
            if (string.IsNullOrWhiteSpace(filtroLimpio))
                return usuarios;

            string[] partes = FiltroHelper.ObtenerPartes(filtroLimpio);

            return usuarios
                .Where(usuario => partes.All(parte => CoincideParte(usuario, parte)))
                .ToList();
        }

        private static bool CoincideParte(UsuarioDto usuario, string parte)
        {
            return Contiene(usuario.Nombres, parte)
                || Contiene(usuario.ApellidoPaterno, parte)
                || Contiene(usuario.ApellidoMaterno, parte)
                || Contiene(usuario.Ci, parte)
                || Contiene(usuario.Email, parte)
                || Contiene(usuario.UserName, parte)
                || Contiene(usuario.Role, parte);
        }

        private static bool Contiene(string? texto, string parte)
        {
            return !string.IsNullOrWhiteSpace(texto)
                && texto.Contains(parte, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<OperacionApiDto> LeerResultadoAsync(HttpResponseMessage response, string mensajeExito)
        {
            string mensaje = await LeerMensajeAsync(response, mensajeExito);

            return response.IsSuccessStatusCode
                ? OperacionApiDto.Ok(mensaje)
                : OperacionApiDto.Error(mensaje);
        }

        private async Task<string> LeerMensajeAsync(HttpResponseMessage response, string mensajePorDefecto)
        {
            JsonElement? json = await LeerJsonAsync(response);
            if (json == null)
                return mensajePorDefecto;

            string mensaje = _mensajeApiAdapter.Adapt(json.Value).Mensaje;
            return string.IsNullOrWhiteSpace(mensaje) ? mensajePorDefecto : mensaje;
        }

        private static async Task<JsonElement?> LeerJsonAsync(HttpResponseMessage response)
        {
            string contenido = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return null;

            try
            {
                using JsonDocument document = JsonDocument.Parse(contenido);
                return document.RootElement.Clone();
            }
            catch
            {
                return null;
            }
        }
    }
}
