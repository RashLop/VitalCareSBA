using ServicioUsuarios.Dominio.Modelos;

namespace ServicioUsuarios.Dominio.Puertos.PuertoSalida
{
    public interface IUsuarioRepositorio
    {
        Usuario? ObtenerPorEmail(string email);
    }
}