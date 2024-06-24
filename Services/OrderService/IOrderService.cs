using OrderApplication.DTOs.Requests;
using OrderApplication.DTOs.Responses;

namespace OrderApplication.Services.OrderService
{
    public interface IOrderService
    {
        Task<BaseResponse> AddOrder(CreateOrderRequest request);
        BaseResponse GetPreviousOrders(long userId);
    }
}
