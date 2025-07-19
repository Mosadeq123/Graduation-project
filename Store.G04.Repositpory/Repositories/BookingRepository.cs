using Store.G04.Core.Entities;
using Store.G04.Core.Repositories.Contract;
using StackExchange.Redis;
using System.Text.Json;

namespace Store.G04.Repository.Repositories;
public class BookingRepository : IBookingRepository
{
    private readonly IDatabase _database;

    public BookingRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerBooking?> UpdateBookingAsync(CustomerBooking customerBooking)
    {
        // Serialize the CustomerBooking object and store it in Redis
        var isUpdated = await _database.StringSetAsync(
            customerBooking.Id,
            JsonSerializer.Serialize(customerBooking),
            TimeSpan.FromDays(30) // Set expiry for 30 days
        );

        // Check if the update was successful
        if (!isUpdated) return null;

        // Retrieve the updated booking from Redis
        var updatedBookingData = await _database.StringGetAsync(customerBooking.Id);
        if (updatedBookingData.IsNullOrEmpty) return null;

        // Deserialize the updated data back to CustomerBooking
        return JsonSerializer.Deserialize<CustomerBooking>(updatedBookingData);
    }
}
