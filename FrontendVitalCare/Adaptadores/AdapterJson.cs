using System.Net.Http.Json;

namespace FrontendVitalCare.Adaptadores
{
    public class AdapterJSON<T>
    {
        private readonly HttpClient _httpClient;

        public AdapterJSON(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<T>> GetListAsync(string url)
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(url) ?? new List<T>();
        }

        public async Task<T?> GetAsync(string url)
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }

        public async Task<bool> PostAsync(string url, T data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string url, T data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
