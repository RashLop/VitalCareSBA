using System.Text.Json;
using FrontendVitalCare.Dto.Reportes;

namespace FrontendVitalCare.Adaptadores.Reportes
{
    public class ReporteVentasPorRolAdapter : IAdapter<JsonElement, ReporteVentasPorRolResponseDto>
    {
        public ReporteVentasPorRolResponseDto Adapt(JsonElement origen)
        {
            return new ReporteVentasPorRolResponseDto
            {
                Mensaje = LeerString(origen, "mensaje"),
                Desde = LeerString(origen, "desde"),
                Hasta = LeerString(origen, "hasta"),
                Data = LeerItems(origen)
            };
        }

        public List<ReporteVentasPorRolResponseDto> AdaptList(IEnumerable<JsonElement> origen)
        {
            return origen.Select(Adapt).ToList();
        }

        private static List<ReporteVentasPorRolDto> LeerItems(JsonElement origen)
        {
            if (!origen.TryGetProperty("data", out JsonElement dataElement) || dataElement.ValueKind != JsonValueKind.Array)
                return new List<ReporteVentasPorRolDto>();

            List<ReporteVentasPorRolDto> items = new();
            foreach (JsonElement item in dataElement.EnumerateArray())
            {
                items.Add(new ReporteVentasPorRolDto
                {
                    Rol = LeerString(item, "rol"),
                    CantidadVentas = LeerInt(item, "cantidadVentas"),
                    TotalRecaudado = LeerDecimal(item, "totalRecaudado")
                });
            }

            return items;
        }

        private static string LeerString(JsonElement origen, string propiedad)
        {
            return origen.TryGetProperty(propiedad, out JsonElement valor)
                ? valor.GetString() ?? string.Empty
                : string.Empty;
        }

        private static int LeerInt(JsonElement origen, string propiedad)
        {
            return origen.TryGetProperty(propiedad, out JsonElement valor) && valor.TryGetInt32(out int numero)
                ? numero
                : 0;
        }

        private static decimal LeerDecimal(JsonElement origen, string propiedad)
        {
            return origen.TryGetProperty(propiedad, out JsonElement valor) && valor.TryGetDecimal(out decimal numero)
                ? numero
                : 0m;
        }
    }
}
