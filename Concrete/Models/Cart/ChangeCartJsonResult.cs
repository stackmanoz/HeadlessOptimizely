using Mediachase.Commerce;

namespace IDM.Shared.Models.Cart
{
    public class ChangeCartJsonResult
    {
        /// <summary>
        /// Status = 0 then return Warning, 1 return Success, -1 return Error (use in Product.js, function addToCart)
        /// </summary>
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int CountItems { get; set; }
        public Money? SubTotal { get; set; }

        // for large cart
        public Money? TotalDiscount { get; set; }
        public Money? Total { get; set; }
        public Money? ShippingTotal { get; set; }
        public Money? TaxTotal { get; set; }
    }
}
