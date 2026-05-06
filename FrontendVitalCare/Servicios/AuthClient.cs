using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Auth;

namespace FrontendVitalCare.Servicios
{
    public class AuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAdapter<JsonElement, UsuarioLoginResponseDto> _loginResponseAdapter;
        private readonly IAdapter<JsonElement, MensajeApiDto> _mensajeApiAdapter;

        public AuthClient(
            HttpClient httpClient,
            IAdapter<JsonElement, UsuarioLoginResponseDto> loginResponseAdapter,
            IAdapter<JsonElement, MensajeApiDto> mensajeApiAdapter)
        {
            _httpClient = httpClient;
            _loginResponseAdapter = loginResponseAdapter;
            _mensajeApiAdapter = mensajeApiAdapter;
        }

        public async Task<(OperacionApiDto Resultado, UsuarioLoginResponseDto? Respuesta)> LoginAsync(UsuarioLoginRequestDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "Credenciales incorrectas.");
                return (OperacionApiDto.Error(mensajeError), null);
            }

            JsonElement? json = await LeerJsonAsync(response);
            if (json == null)
                return (OperacionApiDto.Error("No se pudo leer la respuesta del servidor."), null);

            UsuarioLoginResponseDto respuesta = _loginResponseAdapter.Adapt(json.Value);
            if (string.IsNullOrWhiteSpace(respuesta.Token))
                return (OperacionApiDto.Error("El servidor no devolvio un token valido."), null);

            return (OperacionApiDto.Ok("Inicio de sesion correcto."), respuesta);
        }

        public async Task<OperacionApiDto> RegistrarAsync(UsuarioRegistroDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/registrar", request);
            return await LeerResultadoAsync(response, "Usuario registrado correctamente.");
        }

        public async Task<OperacionApiDto> ValidarActivacionAsync(string token)
        {
            string url = $"api/auth/validar-activacion?token={Uri.EscapeDataString(token)}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            return await LeerResultadoAsync(response, "Token valido.");
        }

        public async Task<OperacionApiDto> ActivarCuentaAsync(ActivarCuentaRequestDto request)
        {
            Dictionary<string, string> datos = new Dictionary<string, string>
            {
                ["token"] = request.Token,
                ["nuevaPassword"] = request.NuevaPassword,
                ["confirmarPassword"] = request.ConfirmarPassword
            };

            using FormUrlEncodedContent content = new FormUrlEncodedContent(datos);
            HttpResponseMessage response = await _httpClient.PostAsync("api/auth/activar-cuenta", content);
            return await LeerResultadoAsync(response, "Cuenta activada correctamente.");
        }

        public async Task<OperacionApiDto> LogoutAsync(string token)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/auth/logout");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            return await LeerResultadoAsync(response, "Sesion cerrada correctamente.");
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
