namespace VitalCareSBA.Dto.VentasDtos
{
    public class DetalleVentaDto
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int IdVenta { get; set; }
        public int IdMedicamento { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        public string NombreMedicamento { get; set; } = string.Empty;
    }
}
