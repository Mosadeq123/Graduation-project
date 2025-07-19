using Store.G04.Core.Dtos.Wishlist;
using System.Threading.Tasks;

namespace Store.G04.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<customerWishlistDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);
    }
}