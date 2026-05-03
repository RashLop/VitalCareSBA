using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Persistencia.Conexion;
using ServicioUsuarios.Infraestructura.Persistencia.Repositorios;

namespace ServicioUsuarios.Infraestructura.Creadores
{
    public class UsuarioRepositorioCreator
    {
        private readonly ConexionStringSingleton conexion;

        public UsuarioRepositorioCreator(ConexionStringSingleton conexion)
        {
            this.conexion = conexion;
        }

        public IUsuarioRepositorio Crear()
        {
            return new UsuarioRepositorio(conexion);
        }
    }
}