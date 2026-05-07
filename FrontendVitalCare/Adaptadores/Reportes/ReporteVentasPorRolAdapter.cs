using System.Text.Json;
using FrontendVitalCare.Dto.Reportes;

namespace FrontendVitalCare.Adaptadores.Reportes
{
    public class ReporteVentasPorRolAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ReporteVentasPorRolAdapter(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ReporteVentasPorRolResponseDto?> ObtenerReporteMensualAsync()
        {
            string? baseUrl = _configuration["ApiUrls:ServicioVentas"];

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("No se encontró la URL de ServicioVentas en appsettings.json.");

            string url = $"{baseUrl.TrimEnd('/')}/api/ventas/reporte-ventas-por-rol";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ReporteVentasPorRolResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}