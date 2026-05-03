namespace ServicioUsuarios.Entidades
{
    public class EmailMensaje
    {
        public string EmailDestino { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordTemporal { get; set; } = string.Empty;
        public string EnlaceActivacion { get; set; } = string.Empty;
    }
}