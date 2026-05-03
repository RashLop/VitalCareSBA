using ServicioUsuarios.Entidades;
using ServicioUsuarios.CasosDeUso.Validadores;

namespace ServicioUsuarios.CasosDeUso.PuertosEntrada
{
    public interface IEmailInputPort
    {
        Result EnviarCorreoActivacionCuenta(EmailMensaje mensaje);
    }
}