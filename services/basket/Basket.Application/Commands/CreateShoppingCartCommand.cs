using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;

namespace Basket.Application.Commands
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItems> Items { get; set; }

        public CreateShoppingCartCommand(string userName, List<ShoppingCartItems> items)
        {
            UserName = userName;
            Items = items;
        }
    }
}