namespace FrontendVitalCare.Dto.MedicamentoDtos
{
    public class MedicamentoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Presentacion { get; set; } = string.Empty;
        public string Clasificacion { get; set; } = string.Empty;

        public string Concentracion {get; set; }  = string.Empty; 

        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}