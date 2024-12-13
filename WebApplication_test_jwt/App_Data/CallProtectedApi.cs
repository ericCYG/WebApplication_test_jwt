using System.Net.Http;
using System.Threading.Tasks;

public class ProtectedApi {
    public async Task<string> CallProtectedApi(string token)
    {
        using (var client = new HttpClient())
        {
            var url = "http://localhost:31376/WeatherForecast";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
        }
    }
}