using System.ComponentModel.DataAnnotations;

namespace FrontendVitalCare.Dto.Usuarios
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El numero de carnet es obligatorio.")]
        [RegularExpression(@"^\d{8}(?:-[A-Za-z0-9]{1,2})?$", ErrorMessage = "El CI debe tener 8 digitos y un complemento opcional de hasta dos caracteres.")]
        public string Ci { get; set; } = string.Empty;

        [Required(ErrorMessage = "La extension es obligatoria.")]
        public string CiExtencion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El telefono debe contener solo digitos numericos.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electronico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Role { get; set; } = "Bioquimico";
    }
}
