namespace ServicioVentas.AdaptadoresDeInterfaz.DTOs
{
    public class ClienteCrearActualizarDto
    {
        public bool EsConsumidorFinal { get; set; }
        public string Nit { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string? CorreoElectronico { get; set; }
        public int IdUsuario { get; set; }
    }
}
