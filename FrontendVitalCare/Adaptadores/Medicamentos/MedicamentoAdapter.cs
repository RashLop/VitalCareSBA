using FrontendVitalCare.Dto.MedicamentoDtos;

namespace VitalCareSBA.FrontendVitalCare.Adaptadores
{
    public class MedicamentoAdapter
    {
        private readonly IAdapter<MedicamentoDto> _adapter;

        public MedicamentoAdapter(IAdapter<MedicamentoDto> adapter)
        {
            _adapter = adapter;
        }

        public Task<List<MedicamentoDto>> GetAllAsync()
        {
            return _adapter.GetListAsync($"api/medicamentos");
        }

        public Task<MedicamentoDto?> GetByIdAsync(int id)
        {
            return _adapter.GetAsync($"api/medicamentos/{id}");
        }

        public Task<bool> CreateAsync(MedicamentoDto medicamento)
        {
            return _adapter.PostAsync($"api/medicamentos", medicamento);
        }

        public Task<bool> UpdateAsync(MedicamentoDto medicamento)
        {
            return _adapter.PutAsync($"api/medicamentos/{medicamento.Id}", medicamento);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _adapter.DeleteAsync($"api/medicamentos/{id}");
        }
    }
}