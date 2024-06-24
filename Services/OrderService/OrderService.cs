using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderApplication.DTOs;
using OrderApplication.DTOs.Requests;
using OrderApplication.DTOs.Responses;
using OrderApplication.Helpers;
using OrderApplication.Models;
using OrderApplication.Services.HttpService;
using OrderApplication.Services.OrderService;
using System.Text;

namespace OrderApplication.Services.OrderService
{
    public class OrderService :IOrderService
    {
        private readonly ApplicationDbContext context;
        private readonly HttpClient client;
        private readonly IHttpService httpService;
        private readonly MicroserviceConfiguration configuration;

        public OrderService(ApplicationDbContext dbContext,  MicroserviceConfiguration msConfiguration, HttpClient client,IHttpService _httpService)
        {
            context = dbContext;
            httpService = _httpService;
            configuration = msConfiguration;
            this.client = client;
        }

        /*public async Task<BaseResponse> AddOrder(CreateOrderRequest request)
            {
                var checkQuantityUrl = $"{configuration.ProductServiceBaseUrl}/api/Product/check-quantity";
                var deductUrl = $"{configuration.ProductServiceBaseUrl}/api/Product/deduct-product";

                try
                {
                    foreach (var orderProduct in request.OrderProducts)
                    {
                        var checkQuantityRequest = new CheckQuantityRequest
                        {
                            ProductId = orderProduct.product_id,
                            ReqQuantity = orderProduct.quantity
                        };
                        var checkQuantityContent = new StringContent(JsonConvert.SerializeObject(checkQuantityRequest), Encoding.UTF8, "application/json");

                        var checkQuantityResponse = await client.PostAsync(checkQuantityUrl, checkQuantityContent);

                        if (!checkQuantityResponse.IsSuccessStatusCode)
                        {
                            return new BaseResponse
                            {
                                status_code = (int)checkQuantityResponse.StatusCode,
                                data = "Failed to check product quantity: " + await checkQuantityResponse.Content.ReadAsStringAsync()
                            };
                        }

                        var checkQuantityResponseData = await checkQuantityResponse.Content.ReadAsStringAsync();
                        var checkQuantityResponseJson = JObject.Parse(checkQuantityResponseData);
                        int quantityCheckStatusCode = (int)checkQuantityResponseJson["status_code"];

                        if (quantityCheckStatusCode != 200)
                        {
                            return new BaseResponse
                            {
                                status_code = quantityCheckStatusCode,
                                data = "Product quantity not available: " + checkQuantityResponseJson["data"]
                            };
                        }
                    }

                    foreach (var orderProduct in request.OrderProducts)
                    {
                        var deductQuantityRequest = new CheckQuantityRequest
                        {
                            ProductId = orderProduct.product_id,
                            ReqQuantity = orderProduct.quantity
                        };
                        var deductQuantityContent = new StringContent(JsonConvert.SerializeObject(deductQuantityRequest), Encoding.UTF8, "application/json");

                        var deductQuantityResponse = await client.PostAsync(deductUrl, deductQuantityContent);

                        if (!deductQuantityResponse.IsSuccessStatusCode)
                        {
                            return new BaseResponse
                            {
                                status_code = (int)deductQuantityResponse.StatusCode,
                                data = "Failed to deduct product quantity: " + await deductQuantityResponse.Content.ReadAsStringAsync()
                            };
                        }

                        var deductQuantityResponseData = await deductQuantityResponse.Content.ReadAsStringAsync();
                        var deductQuantityResponseJson = JObject.Parse(deductQuantityResponseData);
                        int deductStatusCode = (int)deductQuantityResponseJson["status_code"];

                        if (deductStatusCode != 200)
                        {
                            return new BaseResponse
                            {
                                status_code = deductStatusCode,
                                data = "Failed to deduct product quantity: " + deductQuantityResponseJson["data"]
                            };
                        }
                    }

                    var order = new OrderModel
                    {
                        user_id = request.user_id,
                        status = "Pending",
                        placed_at = DateTime.UtcNow
                    };

                    context.Add(order);
                    context.SaveChanges();

                    foreach (var orderProduct in request.OrderProducts)
                    {
                        var productOrder = new ProductOrderModel
                        {
                            order_id = order.id,
                            product_id = orderProduct.product_id,
                            quantity = orderProduct.quantity
                        };

                        context.ProductOrders.Add(productOrder);
                    }

                    context.SaveChanges();

                    order.status = "Placed";
                    context.Update(order);
                    context.SaveChanges();

                    return new BaseResponse
                    {
                        status_code = 200,
                        data = "Order Added and Product Order Updated Successfully"
                    };
                }
                catch (Exception ex)
                {
                    return new BaseResponse
                    {
                        status_code = 500,
                        data = ex.Message
                    };
                }

        }*/


        public async Task<BaseResponse> AddOrder(CreateOrderRequest request)
        {
            var checkQuantityUrl = $"{configuration.ProductServiceBaseUrl}/api/Product/check-quantity";
            var deductUrl = $"{configuration.ProductServiceBaseUrl}/api/Product/deduct-product";

            try
            {
                foreach (var orderProduct in request.OrderProducts)
                {
                    var checkQuantityRequest = new CheckQuantityRequest
                    {
                        ProductId = orderProduct.product_id,
                        ReqQuantity = orderProduct.quantity
                    };

                    var checkQuantityResponse = await httpService.PostAsync(checkQuantityUrl, checkQuantityRequest);

                    if (checkQuantityResponse.status_code != 200)
                    {
                        return new BaseResponse
                        {
                            status_code = checkQuantityResponse.status_code,
                            data = "Failed to check product quantity: " + checkQuantityResponse.data
                        };
                    }
                }

                foreach (var orderProduct in request.OrderProducts)
                {
                    var deductQuantityRequest = new CheckQuantityRequest
                    {
                        ProductId = orderProduct.product_id,
                        ReqQuantity = orderProduct.quantity
                    };

                    var deductQuantityResponse = await httpService.PostAsync(deductUrl, deductQuantityRequest);

                    if (deductQuantityResponse.status_code != 200)
                    {
                        return new BaseResponse
                        {
                            status_code = deductQuantityResponse.status_code,
                            data = "Failed to deduct product quantity: " + deductQuantityResponse.data
                        };
                    }
                }

                var order = new OrderModel
                {
                    user_id = request.user_id,
                    status = "Pending",
                    placed_at = DateTime.UtcNow
                };

                context.Add(order);
                context.SaveChanges();

                foreach (var orderProduct in request.OrderProducts)
                {
                    var productOrder = new ProductOrderModel
                    {
                        order_id = order.id,
                        product_id = orderProduct.product_id,
                        quantity = orderProduct.quantity
                    };

                    context.ProductOrders.Add(productOrder);
                }

                context.SaveChanges();

                order.status = "Placed";
                context.Update(order);
                context.SaveChanges();

                return new BaseResponse
                {
                    status_code = 200,
                    data = "Order Added and Product Order Updated Successfully"
                };
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
        


        public BaseResponse GetPreviousOrders(long userId)
        {
            BaseResponse response;
            try
            {
                using (context)
                {
                    var query = from order in context.Orders
                                join productOrder in context.ProductOrders on order.id equals productOrder.order_id
                                where order.user_id == userId
                                select new OrderDTO
                                {
                                    Id = order.id,
                                    userId = order.user_id,
                                    productId = productOrder.product_id,
                                    status = order.status,
                                    created_at = order.placed_at
                                };

                    var productOrders = query.ToList();

                    if (productOrders.Any())
                    {
                        response = new BaseResponse
                        {
                            status_code = StatusCodes.Status200OK,
                            data = new { productOrders }
                        };
                    }
                    else
                    {
                        response = new BaseResponse
                        {
                            status_code = StatusCodes.Status404NotFound,
                            data = new { message = "No orders found" }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
            }

            return response;
        }
    }
}
