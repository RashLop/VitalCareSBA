using ServicioUsuarios.Dominio.Modelos;

namespace ServicioUsuarios.Dominio.Puertos.PuertoSalida
{
    public interface IUsuarioTokenRepositorio
    {
        void Guardar(UsuarioToken token);
    }
}