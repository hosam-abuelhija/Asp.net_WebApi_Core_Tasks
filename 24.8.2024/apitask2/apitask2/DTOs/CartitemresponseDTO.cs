using apitask2.Models;

namespace apitask2.DTOs
{
    public class CartitemresponseDTO
    {

        public int CartItemId { get; set; }

        public int? CartId { get; set; }

        public int Quantity { get; set; }

        public productresponseDTO? Product { get; set; }
    }
}
