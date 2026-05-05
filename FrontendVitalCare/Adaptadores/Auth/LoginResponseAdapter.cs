using System.Text.Json;
using FrontendVitalCare.Dto.Auth;

namespace FrontendVitalCare.Adaptadores.Auth
{
    public class LoginResponseAdapter : IAdapter<JsonElement, UsuarioLoginResponseDto>
    {
        public UsuarioLoginResponseDto Adapt(JsonElement origen)
        {
            return new UsuarioLoginResponseDto
            {
                Token = origen.TryGetProperty("token", out JsonElement tokenElement)
                    ? tokenElement.GetString() ?? string.Empty
                    : string.Empty
            };
        }

        public List<UsuarioLoginResponseDto> AdaptList(IEnumerable<JsonElement> origen)
        {
            return origen.Select(Adapt).ToList();
        }
    }
}
