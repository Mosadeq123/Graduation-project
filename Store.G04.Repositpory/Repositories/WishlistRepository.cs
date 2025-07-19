using StackExchange.Redis;
using Store.G04.Core.Entities;
using Store.G04.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G04.Repositpory.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly IDatabase _database;
        public WishlistRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteWishlistAsync(string WishlistId)
        {
            return await _database.KeyDeleteAsync(WishlistId);
        }

        public async Task<CustomerWishlist?> GetWishlistAsync(string WishlistId)
        {
            var Wishlist = await _database.StringGetAsync(WishlistId);
            return Wishlist.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerWishlist>(Wishlist);
        }

        public async Task<CustomerWishlist?> UpdateWishlistAsync(CustomerWishlist customer)
        {
            var createdOrUpdateWishlist = await _database.StringSetAsync(customer.Id, JsonSerializer.Serialize(customer), TimeSpan.FromDays(30));
            if (createdOrUpdateWishlist is false) return null;
            return await GetWishlistAsync(customer.Id);
        }
    }
}

