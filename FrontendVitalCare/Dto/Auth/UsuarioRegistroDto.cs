using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontendVitalCare.Dto.Auth
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El carnet de identidad es obligatorio.")]
        public string Ci { get; set; } = string.Empty;

        [Required(ErrorMessage = "La extension es obligatoria.")]
        public string CiExtencion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es obligatorio.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electronico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido.")]
        public string Email { get; set; } = string.Empty;

        public string? Role { get; set; } = string.Empty;

        [JsonIgnore]
        public string UserName { get; set; } = string.Empty;

        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
    }
}
