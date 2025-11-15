using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItems> Items { get; set; }=new();

        public ShoppingCart()
        {
        }
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
    }
}