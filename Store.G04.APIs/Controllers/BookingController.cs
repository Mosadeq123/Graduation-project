using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.Core.Entities;
using Store.G04.Core.Repositories.Contract;

namespace Store.G04.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public BookingController(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }
    [HttpPost("AddBooking")]
    public async Task<IActionResult> UpdateBooking([FromBody] CustomerBooking customerBooking)
    {
        if (customerBooking == null)
        {
            return BadRequest(new ApiErrorResponse(400, "Invalid data"));
        }

        // Perform the update operation
        var updatedBooking = await _bookingRepository.UpdateBookingAsync(customerBooking);

        if (updatedBooking == null)
        {
            return BadRequest(new ApiErrorResponse(400, "Failed to update or create booking"));
        }

        // Optionally map the entity to a DTO if needed
        var result = _mapper.Map<CustomerBooking>(updatedBooking);
        return Ok(result);
    }
}
