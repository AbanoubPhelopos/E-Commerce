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

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            var existingBasket = await GetBasketAsync(basket.UserName);
            if (existingBasket != null)
            {
                return await GetBasketAsync(basket.UserName);
            }
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasketAsync(basket.UserName);
        }

        public async Task DeleteBasketAsync(string userName)
        {
            var existingBasket = await _redisCache.GetStringAsync(userName);
            if (!string.IsNullOrEmpty(existingBasket))
            {
                await _redisCache.RemoveAsync(userName);
            }
        }
    }
}