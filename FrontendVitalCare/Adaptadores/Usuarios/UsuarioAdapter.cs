using System.Text.Json;
using FrontendVitalCare.Dto.Usuarios;

namespace FrontendVitalCare.Adaptadores.Usuarios
{
    public class UsuarioAdapter : IAdapter<JsonElement, UsuarioDto>
    {
        public UsuarioDto Adapt(JsonElement origen)
        {
            return new UsuarioDto
            {
                IdUsuario = LeerInt(origen, "idUsuario"),
                Nombres = LeerString(origen, "nombres"),
                ApellidoPaterno = LeerString(origen, "apellidoPaterno"),
                ApellidoMaterno = LeerString(origen, "apellidoMaterno"),
                Ci = LeerString(origen, "ci"),
                CiExtencion = LeerString(origen, "ciExtencion"),
                Telefono = LeerString(origen, "telefono"),
                Activo = (sbyte)LeerInt(origen, "activo"),
                Email = LeerString(origen, "email"),
                UserName = LeerString(origen, "userName"),
                Role = LeerString(origen, "role"),
                MustChangePassword = (sbyte)LeerInt(origen, "mustChangePassword")
            };
        }

        public List<UsuarioDto> AdaptList(IEnumerable<JsonElement> origen)
        {
            return origen.Select(Adapt).ToList();
        }

        private static string LeerString(JsonElement origen, string propiedad)
        {
            return origen.TryGetProperty(propiedad, out JsonElement valor)
                ? valor.GetString() ?? string.Empty
                : string.Empty;
        }

        private static int LeerInt(JsonElement origen, string propiedad)
        {
            return origen.TryGetProperty(propiedad, out JsonElement valor)
                && valor.TryGetInt32(out int numero)
                    ? numero
                    : 0;
        }
    }
}
