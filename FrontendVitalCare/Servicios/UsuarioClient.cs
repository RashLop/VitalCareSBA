using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Usuarios;

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

            string filtroUrl = Uri.EscapeDataString(filtro ?? string.Empty);
            HttpResponseMessage response = await _httpClient.GetAsync($"api/usuarios/GetUsers?filtro={filtroUrl}");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudieron obtener los usuarios.");
                return (OperacionApiDto.Error(mensajeError), new List<UsuarioDto>());
            }

            JsonElement? json = await LeerJsonAsync(response);
            if (json == null || !json.Value.TryGetProperty("data", out JsonElement dataElement) || dataElement.ValueKind != JsonValueKind.Array)
            {
                return (OperacionApiDto.Error("La respuesta del servidor no contiene usuarios válidos."), new List<UsuarioDto>());
            }

            List<UsuarioDto> usuarios = _usuarioAdapter.AdaptList(dataElement.EnumerateArray());

            string mensaje = await LeerMensajeAsync(response, "Usuarios obtenidos correctamente.");
            return (OperacionApiDto.Ok(mensaje), usuarios);
        }

        public async Task<(OperacionApiDto Resultado, UsuarioDto? Usuario)> ObtenerPorIdAsync(int idUsuario)
        {
            ConfigurarAutorizacion();

            HttpResponseMessage response = await _httpClient.GetAsync($"api/usuarios/getUserById?id={idUsuario}");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudo obtener el usuario.");
                return (OperacionApiDto.Error(mensajeError), null);
            }

            JsonElement? json = await LeerJsonAsync(response);
            if (json == null || !json.Value.TryGetProperty("data", out JsonElement dataElement) || dataElement.ValueKind != JsonValueKind.Object)
            {
                return (OperacionApiDto.Error("La respuesta del servidor no contiene un usuario válido."), null);
            }

            UsuarioDto usuario = _usuarioAdapter.Adapt(dataElement);
            string mensaje = await LeerMensajeAsync(response, "Usuario obtenido correctamente.");
            return (OperacionApiDto.Ok(mensaje), usuario);
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
