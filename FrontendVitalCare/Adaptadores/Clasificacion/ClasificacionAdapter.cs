using FrontendVitalCare.Dto.ClasificacionDtos;

namespace VitalCareSBA.FrontendVitalCare.Adaptadores
{
    public class ClasificacionAdapter
    {
        private readonly IAdapter<ClasificacionDto> _adapter;

        public ClasificacionAdapter(IAdapter<ClasificacionDto> adapter)
        {
            _adapter = adapter;
        }

        public Task<List<ClasificacionDto>> GetAllAsync()
        {
            return _adapter.GetListAsync($"api/clasificaciones");
        }

        public Task<ClasificacionDto?> GetByIdAsync(int id)
        {
            return _adapter.GetAsync($"api/clasificaciones/{id}");
        }

        public Task<bool> CreateAsync(ClasificacionDto clasificacion)
        {
            return _adapter.PostAsync($"api/clasificaciones", clasificacion);
        }

        public Task<bool> UpdateAsync(ClasificacionDto clasificacion)
        {
            return _adapter.PutAsync($"api/clasificaciones/{clasificacion.Id}", clasificacion);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _adapter.DeleteAsync($"api/clasificaciones/{id}");
        }
    }
}
