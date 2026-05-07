namespace FrontendVitalCare.Dto.Reportes
{
    public class ArchivoDescargaDto
    {
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
        public string NombreArchivo { get; set; } = string.Empty;
        public string TipoContenido { get; set; } = "application/octet-stream";
    }
}
