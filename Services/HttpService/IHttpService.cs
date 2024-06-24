using OrderApplication.DTOs.Responses;

namespace OrderApplication.Services.HttpService
{
    public interface IHttpService
    {
        Task<BaseResponse> PostAsync(string url, object data);
    }
}
