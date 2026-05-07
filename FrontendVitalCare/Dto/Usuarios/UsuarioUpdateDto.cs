namespace FrontendVitalCare.Dto.Usuarios
{
    public class UsuarioUpdateDto
    {
        public int IdUsuario { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ApellidoMaterno { get; set; }

        public string Ci { get; set; } = string.Empty;

        public string CiExtencion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "Bioquimico";

        public byte Activo { get; set; } = 1;
        public string UserName { get; set; } = string.Empty;
        public byte MustChangePassword { get; set; } = 1;
    }
}
