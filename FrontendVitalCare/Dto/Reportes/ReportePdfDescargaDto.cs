namespace FrontendVitalCare.Dto.Reportes
{
    public class ReportePdfDescargaDto
    {
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
        public string NombreArchivo { get; set; } = string.Empty;
        public string TipoContenido { get; set; } = "application/pdf";
    }
}
