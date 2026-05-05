using System.Text.Json;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Adaptadores;

public class MedicamentoAdapter : IAdapter<JsonElement, MedicamentoDto>
{
    public MedicamentoDto Adapt(JsonElement origen)
    {
        return new MedicamentoDto
        {
            Id = origen.GetProperty("id").GetInt32(),
            Nombre = origen.GetProperty("nombre").GetString() ?? string.Empty,
            Presentacion = origen.GetProperty("presentacion").GetString() ?? string.Empty,
            Clasificacion = origen.GetProperty("idClasificacion").GetInt32().ToString(), // CORREGIDO
            Precio = origen.GetProperty("precio").GetDecimal(),
            Stock = origen.GetProperty("stock").GetInt32()
        };
    }

    public List<MedicamentoDto> AdaptList(IEnumerable<JsonElement> origen)
    {
        return origen.Select(Adapt).ToList();
    }
}