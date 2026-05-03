using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Persistencia.Repositorios;

namespace ServicioUsuarios.Infraestructura.Creadores
{
    public class UsuarioTokenRepositoryCreator
    {
        public IUsuarioTokenRepository CreateRepo()
        {
            return new UsuarioTokenRepository();
        }
    }
}
