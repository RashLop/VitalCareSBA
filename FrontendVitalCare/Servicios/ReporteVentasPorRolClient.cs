using System.Net.Http.Headers;
using System.Text.Json;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Reportes;

namespace FrontendVitalCare.Servicios
{
    public class ReporteVentasPorRolClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAdapter<JsonElement, ReporteVentasPorRolResponseDto> _reporteAdapter;
        private readonly IAdapter<JsonElement, MensajeApiDto> _mensajeAdapter;

        public ReporteVentasPorRolClient(
            HttpClient httpClient,
            IAdapter<JsonElement, ReporteVentasPorRolResponseDto> reporteAdapter,
            IAdapter<JsonElement, MensajeApiDto> mensajeAdapter)
        {
            _httpClient = httpClient;
            _reporteAdapter = reporteAdapter;
            _mensajeAdapter = mensajeAdapter;
        }

        public async Task<(OperacionApiDto Resultado, ReporteVentasPorRolResponseDto? Reporte)> ObtenerReporteMensualAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/ventas/reporte-ventas-por-rol");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudo generar el reporte de ventas por rol.");
                return (OperacionApiDto.Error(mensajeError), null);
            }

            JsonElement? json = await LeerJsonAsync(response);
            if (json == null)
                return (OperacionApiDto.Error("No se pudo leer la respuesta del servicio de ventas."), null);

            ReporteVentasPorRolResponseDto reporte = _reporteAdapter.Adapt(json.Value);
            return (OperacionApiDto.Ok(reporte.Mensaje), reporte);
        }

        public async Task<(OperacionApiDto Resultado, ArchivoDescargaDto? Archivo)> DescargarPdfMensualAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/ventas/reporte-ventas-por-rol/pdf");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudo descargar el PDF del reporte.");
                return (OperacionApiDto.Error(mensajeError), null);
            }

            byte[] contenido = await response.Content.ReadAsByteArrayAsync();
            if (contenido.Length == 0)
                return (OperacionApiDto.Error("El servicio devolvio un PDF vacio."), null);

            string nombreArchivo = LeerNombreArchivo(response.Content.Headers.ContentDisposition)
                ?? $"reporte-ventas-por-rol-{DateTime.Now:yyyy-MM-dd-HHmmss}.pdf";

            return (OperacionApiDto.Ok("PDF generado correctamente."), new ArchivoDescargaDto
            {
                Contenido = contenido,
                NombreArchivo = nombreArchivo,
                TipoContenido = response.Content.Headers.ContentType?.MediaType ?? "application/pdf"
            });
        }

        public async Task<(OperacionApiDto Resultado, ArchivoDescargaDto? Archivo)> DescargarExcelMensualAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/ventas/reporte-ventas-por-rol/excel");
            if (!response.IsSuccessStatusCode)
            {
                string mensajeError = await LeerMensajeAsync(response, "No se pudo descargar el Excel del reporte.");
                return (OperacionApiDto.Error(mensajeError), null);
            }

            byte[] contenido = await response.Content.ReadAsByteArrayAsync();
            if (contenido.Length == 0)
                return (OperacionApiDto.Error("El servicio devolvio un archivo Excel vacio."), null);

            string nombreArchivo = LeerNombreArchivo(response.Content.Headers.ContentDisposition)
                ?? $"reporte-ventas-por-rol-{DateTime.Now:yyyy-MM-dd-HHmmss}.xls";

            return (OperacionApiDto.Ok("Excel generado correctamente."), new ArchivoDescargaDto
            {
                Contenido = contenido,
                NombreArchivo = nombreArchivo,
                TipoContenido = response.Content.Headers.ContentType?.MediaType ?? "application/vnd.ms-excel"
            });
        }

        private async Task<string> LeerMensajeAsync(HttpResponseMessage response, string mensajePorDefecto)
        {
            JsonElement? json = await LeerJsonAsync(response);
            if (json == null)
                return mensajePorDefecto;

            string mensaje = _mensajeAdapter.Adapt(json.Value).Mensaje;
            return string.IsNullOrWhiteSpace(mensaje) ? mensajePorDefecto : mensaje;
        }

        private static string? LeerNombreArchivo(ContentDispositionHeaderValue? contentDisposition)
        {
            return contentDisposition?.FileNameStar?.Trim('"')
                ?? contentDisposition?.FileName?.Trim('"');
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
