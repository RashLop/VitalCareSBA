using ServicioUsuarios.Entidades;

namespace ServicioUsuarios.CasosDeUso.Validadores
{
    public static class EmailValidacion
    {
        public static Result ValidarCorreoActivacion(EmailMensaje mensaje)
        {
            mensaje.EmailDestino = mensaje.EmailDestino?.Trim() ?? string.Empty;
            mensaje.Nombres = mensaje.Nombres?.Trim() ?? string.Empty;
            mensaje.UserName = mensaje.UserName?.Trim() ?? string.Empty;
            mensaje.PasswordTemporal = mensaje.PasswordTemporal?.Trim() ?? string.Empty;
            mensaje.EnlaceActivacion = mensaje.EnlaceActivacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(mensaje.EmailDestino))
                return Result.Fail("El correo destino es obligatorio.");

            if (string.IsNullOrWhiteSpace(mensaje.UserName))
                return Result.Fail("El nombre de usuario es obligatorio para el correo.");

            if (string.IsNullOrWhiteSpace(mensaje.PasswordTemporal))
                return Result.Fail("La contraseña temporal es obligatoria para el correo.");

            if (string.IsNullOrWhiteSpace(mensaje.EnlaceActivacion))
                return Result.Fail("El enlace de activación es obligatorio.");

            return Result.Ok();
        }
    }
}