using System.ComponentModel.DataAnnotations;

namespace FrontendVitalCare.Dto.Auth
{
    public class ActivarCuentaRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NuevaPassword { get; set; } = string.Empty;

        [Required]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
