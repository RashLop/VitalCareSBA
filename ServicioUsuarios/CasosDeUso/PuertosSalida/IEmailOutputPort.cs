using ServicioUsuarios.Entidades;
using ServicioUsuarios.CasosDeUso.Validadores;

namespace ServicioUsuarios.CasosDeUso.PuertosSalida
{
    public interface IEmailOutputPort
    {
        Result Enviar(EmailMensaje mensaje, string asunto, string cuerpoHtml);
    }
}