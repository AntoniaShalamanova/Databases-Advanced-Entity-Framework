using Newtonsoft.Json;

namespace ProductShop.Dtos.Export
{
    public class GetProductsInRangeDto
    {
        [JsonProperty(PropertyName = "name")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public string SellerFullName { get; set; }
    }
}