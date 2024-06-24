namespace OrderApplication.DTOs.Requests
{
    public class CheckQuantityRequest
    {
        public long ProductId { get; set; }
        public int ReqQuantity { get; set; }
    }
}
