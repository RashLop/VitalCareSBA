namespace VitalCareSBA.FrontendVitalCare.Adaptadores
{
    public interface IAdapter<T>
    {
        Task<List<T>> GetListAsync(string url);
        Task<T?> GetAsync(string url);
        Task<bool> PostAsync(string url, T data); ///Post actualizar o crear
        Task<bool> PutAsync(string url, T data); /// Actualizar 
        Task<bool> DeleteAsync(string url); //Eliminar
    }
}