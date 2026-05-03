using System.Net;
using System.Net.Mail;
using ServicioUsuarios.Entidades;
using ServicioUsuarios.CasosDeUso.PuertosSalida;
using ServicioUsuarios.CasosDeUso.Validadores;
using ServicioUsuarios.FrameworksYDrivers.Ayudadores;

namespace ServicioUsuarios.FrameworksYDrivers.ServiciosExternos
{
    public class SmtpEmailSender : IEmailOutputPort
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _smtpSettings = configuration
                .GetSection("SmtpSettings")
                .Get<SmtpSettings>() ?? new SmtpSettings();
        }

        public Result Enviar(EmailMensaje mensaje, string asunto, string cuerpoHtml)
        {
            try
            {
                using MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(
                    _smtpSettings.RemitenteEmail,
                    _smtpSettings.RemitenteNombre
                );

                mailMessage.To.Add(mensaje.EmailDestino);
                mailMessage.Subject = asunto;
                mailMessage.Body = cuerpoHtml;
                mailMessage.IsBodyHtml = true;

                using SmtpClient client = new SmtpClient(
                    _smtpSettings.Host,
                    _smtpSettings.Port
                );

                client.Credentials = new NetworkCredential(
                    _smtpSettings.RemitenteEmail,
                    _smtpSettings.Password
                );

                client.EnableSsl = _smtpSettings.UseSsl;
                client.Send(mailMessage);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"No se pudo enviar el correo electrónico. Detalle: {ex.Message}");
            }
        }
    }
}