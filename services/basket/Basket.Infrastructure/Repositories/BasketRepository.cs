using System.Net.Http.Json;
using System.Text.Json;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null!;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket)!;
        }

        public Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBasketAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}