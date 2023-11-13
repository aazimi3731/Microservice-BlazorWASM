using ClientApp.Interfaces;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ClientApp.Helpers
{
  public class MicroserviceReqResp : IMicroserviceReqResp
  {
    private readonly HttpClient _httpClient;

    public MicroserviceReqResp(HttpClient httpClient_) 
    {
      _httpClient = httpClient_;
    }

    public async Task<T?> SendGetRequest<T>(string url_)
    {
      var response = await _httpClient.GetAsync(url_);

      if (response == null || !response.IsSuccessStatusCode)
      {
        throw new ApplicationException($"Something went wrong calling the API: {response?.ReasonPhrase}");
      }

      var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<T?> SendPostRequest<T>(string url_, T? object_)
    {
      var objectJson = new StringContent(JsonSerializer.Serialize(object_), Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(url_, objectJson);

      if (response == null || !response.IsSuccessStatusCode)
      {
        throw new ApplicationException($"Something went wrong calling the API: {response?.ReasonPhrase}");
      }

      var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
  }
}
