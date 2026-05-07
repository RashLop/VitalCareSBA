namespace FrontendVitalCare.Dto.Reportes
{
    public class ReporteVentasPorRolResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public string Desde { get; set; } = string.Empty;
        public string Hasta { get; set; } = string.Empty;
        public List<ReporteVentasPorRolDto> Data { get; set; } = new();
    }
}
