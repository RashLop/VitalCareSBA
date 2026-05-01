namespace ServicioVentas.AdaptadoresDeInterfaz.DTOs
{
    public class RespuestaOperacionDto<T>
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }
    }
}
