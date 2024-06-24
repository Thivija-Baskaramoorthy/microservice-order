namespace OrderApplication.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }

        public long  userId {  get; set; }

        public string status { get; set; }

        public DateTime created_at { get; set; }

        public long productId {  get; set; }
    }
}
