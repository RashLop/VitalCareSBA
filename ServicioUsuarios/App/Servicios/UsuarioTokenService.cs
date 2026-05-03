using ServicioUsuarios.Dominio.Modelos;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Ayudadores;

namespace ServicioUsuarios.App.Servicios
{
    public class UsuarioTokenService
    {
        private readonly IUsuarioTokenRepositorio repo;

        public UsuarioTokenService(IUsuarioTokenRepositorio repo)
        {
            this.repo = repo;
        }

        public void GuardarToken(int usuarioId, string token, int minutos)
        {
            var entidad = new UsuarioToken(
                usuarioId,
                TokenHelper.GenerarHash(token),
                "JWT",
                DateTime.UtcNow.AddMinutes(minutos)
            );

            repo.Guardar(entidad);
        }
    }
}