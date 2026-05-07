namespace FrontendVitalCare.Dto.Usuarios
{
    public class UsuarioCreateDto
    {
        public string Nombres { get; set; } = string.Empty;

        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ApellidoMaterno { get; set; }

        public string Ci { get; set; } = string.Empty;

        public string CiExtencion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "Bioquimico";
    }
}
