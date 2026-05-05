
using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto.MedicamentoDtos;

namespace FrontendVitalCare.Services
{
    public class MedicamentoClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAdapter<JsonElement, MedicamentoDto> _adapter;

        public MedicamentoClient(HttpClient httpClient, IAdapter<JsonElement, MedicamentoDto> adapter)
        {
            _httpClient = httpClient;
            _adapter = adapter;
        }

        public async Task<List<MedicamentoDto>> ObtenerTodosAsync()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            using var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return _adapter.AdaptList(jsonDoc.RootElement.EnumerateArray());
        }

        public async Task<MedicamentoDto?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");
            response.EnsureSuccessStatusCode();

            using var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return _adapter.Adapt(jsonDoc.RootElement);
        }

        public async Task<bool> CrearAsync(MedicamentoDto dto)
            => (await _httpClient.PostAsJsonAsync("", dto)).IsSuccessStatusCode;

        public async Task<bool> ActualizarAsync(int id, MedicamentoDto dto)
            => (await _httpClient.PutAsJsonAsync($"{id}", dto)).IsSuccessStatusCode;

        public async Task<bool> EliminarAsync(int id, int idUsuario)
            => (await _httpClient.DeleteAsync($"{id}?idUsuario={idUsuario}")).IsSuccessStatusCode;
    }
}