namespace ServicioUsuarios.Dominio.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "";
        public sbyte Activo { get; set; }
        public sbyte MustChangePassword { get; set; }
    }
}