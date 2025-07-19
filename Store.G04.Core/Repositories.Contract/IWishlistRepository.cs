using Store.G04.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Repositories.Contract
{
    public interface IWishlistRepository
    {
        Task<CustomerWishlist?> GetWishlistAsync(string WishlistId);
        Task<CustomerWishlist?> UpdateWishlistAsync(CustomerWishlist customer);
        Task<bool> DeleteWishlistAsync(string WishlistId);
    }
}
