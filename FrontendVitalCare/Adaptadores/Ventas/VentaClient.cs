using FrontendVitalCare.Dto.VentasDtos;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Services
{
    public class VentaClient
    {
        private readonly AdapterJSON<VentaDto> _adapter;

        public VentaClient(AdapterJSON<VentaDto> adapter)
        {
            _adapter = adapter;
        }

        public async Task<List<VentaDto>> ObtenerTodosAsync(string filtro = "")
        {
            var url = string.IsNullOrEmpty(filtro) ? "api/ventas" : $"api/ventas?filtro={filtro}";
            return await _adapter.GetListAsync(url);
        }

        public async Task<VentaDto?> ObtenerPorIdAsync(int id)
        {
            return await _adapter.GetAsync($"api/ventas/{id}");
        }

        public async Task<bool> CrearAsync(VentaDto venta)
        {
            return await _adapter.PostAsync("api/ventas", venta);
        }

        public async Task<bool> ActualizarAsync(int id, VentaDto venta)
        {
            return await _adapter.PutAsync($"api/ventas/{id}", venta);
        }

        public async Task<bool> AnularAsync(int id, int idUsuarioEditor)
        {
            return await _adapter.DeleteAsync($"api/ventas/{id}?idUsuarioEditor={idUsuarioEditor}");
        }
    }
}