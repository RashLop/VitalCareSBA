using System.Text.Json;
using FrontendVitalCare.Dto;

namespace FrontendVitalCare.Adaptadores.Auth
{
    public class MensajeApiAdapter : IAdapter<JsonElement, MensajeApiDto>
    {
        public MensajeApiDto Adapt(JsonElement origen)
        {
            return new MensajeApiDto
            {
                Mensaje = origen.TryGetProperty("mensaje", out JsonElement mensajeElement)
                    ? mensajeElement.GetString() ?? string.Empty
                    : string.Empty
            };
        }

        public List<MensajeApiDto> AdaptList(IEnumerable<JsonElement> origen)
        {
            return origen.Select(Adapt).ToList();
        }
    }
}
