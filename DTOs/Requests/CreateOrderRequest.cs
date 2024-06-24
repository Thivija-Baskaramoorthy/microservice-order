using System.ComponentModel.DataAnnotations;

namespace OrderApplication.DTOs.Requests
{
    public class CreateOrderRequest
    {
        
        public long user_id { get; set; }

        //public long product_id { get; set; }

      //  public int quantity  { get; set; }
        // public string stauts {  get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }




    }
}
