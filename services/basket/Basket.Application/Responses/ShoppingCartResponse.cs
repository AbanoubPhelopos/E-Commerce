namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemResponse> Items { get; set; }=new();

        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public Decimal TotalPrice 
        { 
            get
            {
                return Items.Sum(item => item.Price * item.Quantity);
            }
        }
    }
}