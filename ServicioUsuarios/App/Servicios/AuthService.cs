using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Ayudadores;
using ServicioUsuarios.Infraestructura.Creadores;
using ServicioUsuarios.Dominio.Validadores;

namespace ServicioUsuarios.App.Servicios
{
    public class AuthService
    {
        private readonly IUsuarioRepositorio repo;
        private readonly ITokenService tokenService;
        private readonly LoginValidador validator;
        private readonly UsuarioTokenService usuarioTokenService;

        public AuthService(
            UsuarioRepositorioCreator creator,
            ITokenService tokenService,
            LoginValidador validator,
            UsuarioTokenService usuarioTokenService)
        {
            this.repo = creator.Crear();
            this.tokenService = tokenService;
            this.validator = validator;
            this.usuarioTokenService = usuarioTokenService;
        }

        public string Login(string email, string password)
        {
            // 🔥 VALIDACIÓN (igual farmacia)
            validator.Validar(email, password);

            var usuario = repo.ObtenerPorEmail(email.Trim());

            if (usuario == null)
                throw new Exception("Usuario no existe");

            if (usuario.Activo != 1)
                throw new Exception("Usuario inactivo");

            if (!PasswordHelper.Verify(password.Trim(), usuario.PasswordHash))
                throw new Exception("Credenciales inválidas");

            // 🔥 GENERAR TOKEN
            var jwt = tokenService.GenerarToken(usuario);

            // 🔥 GUARDAR TOKEN (100% estilo farmacia)
            usuarioTokenService.GuardarToken(usuario.IdUsuario, jwt, 60);
            // ⚠️ Si tu modelo es UsuarioIdUsuario, usa:
            // usuarioTokenService.GuardarToken(usuario.IdUsuarioUsuario, jwt, 60);

            return jwt;
        }
    }
}