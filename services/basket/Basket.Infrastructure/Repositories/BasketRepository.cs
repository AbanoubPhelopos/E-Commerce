using System.Net.Http.Json;
using System.Text.Json;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly ILogger<BasketRepository> _logger;
        public BasketRepository(IDistributedCache redisCache, ILogger<BasketRepository> logger)
        {
            _redisCache = redisCache;
            _logger = logger;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            _logger.LogInformation("Getting basket from Redis for user {UserName}", userName);
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                _logger.LogWarning("Basket not found in Redis for user {UserName}", userName);
                return null!;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket)!;
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            _logger.LogInformation("Updating basket in Redis for user {UserName}", basket.UserName);
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
            _logger.LogInformation("Deleting basket from Redis for user {UserName}", userName);
            var existingBasket = await _redisCache.GetStringAsync(userName);
            if (!string.IsNullOrEmpty(existingBasket))
            {
                await _redisCache.RemoveAsync(userName);
            }
        }
    }
}