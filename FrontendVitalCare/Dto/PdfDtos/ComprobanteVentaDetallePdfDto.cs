namespace FrontendVitalCare.Dto.PdfDtos
{
    public class ComprobanteVentaDetallePdfDto
    {
        public int Cantidad { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioUnitario { get; set; }

        public decimal Importe => Cantidad * PrecioUnitario;
    }
}