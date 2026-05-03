using ServicioUsuarios.Dominio.Modelos;

namespace ServicioUsuarios.Dominio.Puertos.PuertoSalida
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
    }
}