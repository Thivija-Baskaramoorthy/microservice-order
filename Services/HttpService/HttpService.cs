using Newtonsoft.Json;
using OrderApplication.DTOs.Responses;
using System.Text;

namespace OrderApplication.Services.HttpService
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient client;

        public HttpService(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<BaseResponse> PostAsync(string url, object requestBody)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync(url, content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<BaseResponse>(responseData);

                    return new BaseResponse
                    {
                        status_code = responseJson.status_code,
                        data = responseJson.data
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        status_code = (int)responseMessage.StatusCode,
                        data = "Failed to place order: " + await responseMessage.Content.ReadAsStringAsync()
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status_code = 500,
                    data = ex.Message
                };
            }
        }

        private async Task<BaseResponse> CreateResponse(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<BaseResponse>(responseData);

                return new BaseResponse
                {
                    status_code = responseJson.status_code,
                    data = responseJson.data
                };
            }
            else
            {
                return new BaseResponse
                {
                    status_code = (int)responseMessage.StatusCode,
                    data = await responseMessage.Content.ReadAsStringAsync()
                };
            }
        }
    }
}
