using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Persistencia.Repositorios;

namespace ServicioUsuarios.Infraestructura.Creadores
{
    public class UsuarioRepositoryCreator
    {
        public IUsuarioRepository CreateRepo()
        {
            return new UsuarioRepository();
        }
    }
}
