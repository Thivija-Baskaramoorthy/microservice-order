using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApplication.DTOs.Requests;
using OrderApplication.DTOs.Responses;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using OrderApplication.Services.OrderService;

namespace OrderApplication.Controllers
{
    [Route("Order-Service")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;   
        }

        [HttpPost]
        [Route("Order")]
        public async Task<BaseResponse> AddOrder([FromBody] CreateOrderRequest request)
        {
           return await orderService.AddOrder(request);

        }

        [HttpPost]
        [Route("view-order")]
        public BaseResponse GetPreviousOrders([FromBody] long userId)
        {
            return  orderService.GetPreviousOrders(userId);

        }

        /////////////////////////////////////////////////////////

        /*[HttpGet]
        [Route("Add")]
        public IActionResult Get([FromQuery] int n1, [FromQuery] int n2) 
        {
            return Content((n1+n2).ToString());
        }*/

    }
}
