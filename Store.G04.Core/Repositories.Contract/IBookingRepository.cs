using Store.G04.Core.Entities;
using System.Threading.Tasks;

namespace Store.G04.Core.Repositories.Contract
{
    public interface IBookingRepository
    {
        Task<CustomerBooking?> UpdateBookingAsync(CustomerBooking customerBooking);
    }
}