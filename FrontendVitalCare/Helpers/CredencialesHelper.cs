using System.Text.RegularExpressions;

namespace FrontendVitalCare.Helpers
{
    public static class CredencialesHelper
    {
        public static string GenerarUserName(string nombres, string apellidoPaterno, string ci)
        {
            string nombre = ObtenerPrimerNombre(nombres);
            string apellido = apellidoPaterno;
            string ciNormalizado = NormalizarCi(ci);

            string baseUser = $"{nombre}.{apellido}.{ciNormalizado}".ToLower();

            baseUser = QuitarTildes(baseUser);
            baseUser = Regex.Replace(baseUser, @"[^a-z0-9\.]", "");

            return baseUser;
        }

        private static string ObtenerPrimerNombre(string nombres)
        {
            if (string.IsNullOrWhiteSpace(nombres))
                return string.Empty;

            return nombres.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private static string NormalizarCi(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return string.Empty;

            ci = ci.Trim().ToUpper();
            return Regex.Replace(ci, @"[^A-Z0-9]", "");
        }

        private static string QuitarTildes(string texto)
        {
            return texto
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u")
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                .Replace("Ó", "O").Replace("Ú", "U");
        }
    }
}
