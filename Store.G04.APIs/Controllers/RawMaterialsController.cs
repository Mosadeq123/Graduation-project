using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Store.G04.APIs.Attributes;
using Store.G04.APIs.Errors;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;
using Store.G04.Core.Helper;
using Store.G04.Core.Services.Contract;
using Store.G04.Core.Specifications.RawMaterialss;
using Store.G04.Repositpory.Data.Contexts;

namespace Store.G04.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RawMaterialsController : BaseApiController
{
    private readonly IRawMaterialsService _rawMaterialsService;
    private readonly IMapper _mapper;
    private readonly StoreDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;
    public RawMaterialsController(IRawMaterialsService rawMaterialsService, StoreDbContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
    {
        _rawMaterialsService = rawMaterialsService;
        _context = context;
        _mapper = mapper;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    [Cached(100)]
    [ProducesResponseType(typeof(PaginationResponse<RawMaterialDtos>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginationResponse<RawMaterialDtos>>> GetAllRawMaterials([FromQuery] RawMaterialssSpecParams rawMaterialssSpec)
    {
        var result = await _rawMaterialsService.GetAllRawMaterialsAsync(rawMaterialssSpec);
        return Ok(result);
    }

    [HttpGet("GetRawMaterials")]
    [ProducesResponseType(typeof(PaginationResponse<RawMaterialDtos>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginationResponse<RawMaterialDtos>>> GetRawMaterials([FromQuery] RawMaterialssSpecParams rawMaterialssSpec)
    {
        var result = await _rawMaterialsService.GetAllRawMaterialsAsync(rawMaterialssSpec);
        return Ok(result);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RawMaterialDtos), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RawMaterialDtos>> GetRawMaterialById(int? id)
    {
        if (id == null)
        {
            return BadRequest(new ApiErrorResponse(400, "The raw material ID cannot be null."));
        }

        try
        {
            var result = await _rawMaterialsService.GetRawMaterialById(id.Value);

            if (result == null)
            {
                return NotFound(new ApiErrorResponse(404, $"Raw material with ID {id} was not found."));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while fetching raw material with ID {id}: {ex.Message}"));
        }
    }

    [HttpPost("Book")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookingMaterialDto>> CreateBooking(BookingMaterialDto createBookingDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid booking data provided."));

        try
        {
            var booking = _mapper.Map<BookingMaterial>(createBookingDto);
            _context.BookingMaterials.Add(booking);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, _mapper.Map<BookingMaterialDto>(booking)); // Map back to DTO

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while creating the booking: {ex.Message}"));
        }
    }

    [HttpGet("Booking/{id}")]
    [ProducesResponseType(typeof(BookingMachineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookingMaterialDto>> GetBookingById(int id)
    {
        try
        {
            var booking = await _context.BookingMaterials.FindAsync(id);

            if (booking == null)
            {
                return NotFound(new ApiErrorResponse(404, $"Booking with ID {id} not found."));
            }

            return Ok(_mapper.Map<BookingMachineDto>(booking));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while fetching booking with ID {id}: {ex.Message}"));
        }
    }
   
    [HttpPut("Booking/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBookingMachine(int id, [FromBody] BookingMaterialDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid update data provided."));

        try
        {
            var booking = await _context.BookingMaterials.FindAsync(id);

            if (booking == null)
            {
                return NotFound(new ApiErrorResponse(404, $"Booking with ID {id} not found."));
            }

            // Update the MachineId
            booking.MachineId = updateDto.MachineId;
            _context.Entry(booking).State = EntityState.Modified; // Explicitly mark as modified

            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while updating booking with ID {id}: {ex.Message}"));
        }
    }
    
    [HttpGet("available/for-material/{materialId}")]
    [ProducesResponseType(typeof(IEnumerable<MachineDtos>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MachineDtos>>> GetAvailableMachinesForMaterial(int materialId)
    {
        try
        {
            var rawMaterial = await _context.RawMaterial.FindAsync(materialId);
            if (rawMaterial == null)
            {
                return NotFound(new ApiErrorResponse(404, $"Raw material with ID {materialId} not found."));
            }

            // Example: Fetch machines compatible with the raw material's properties
            // Adjust this logic based on your actual compatibility rules
            var availableMachines = await _context.Machine
                .Where(m => m.Id == rawMaterial.MachineId) // Example: Matching yarn type to machine type
                                                                   // Add other compatibility checks here, e.g., m.Softness >= rawMaterial.Softness
                .Select(m => _mapper.Map<MachineDtos>(m))
                .ToListAsync();

            return Ok(availableMachines);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"Error fetching available machines for material ID {materialId}: {ex.Message}"));
        }
    }
  
    [HttpPost("CreateRawMaterial")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RawMaterialDtos>> CreateRawMaterial([FromForm] RawMaterialWithImageDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid raw material data provided."));

        try
        {
            string? savedFileName = null;
            if (createDto.ImageFile != null && createDto.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "Images", "ImageRawMaterial");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + createDto.ImageFile.FileName;
                string filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createDto.ImageFile.CopyToAsync(fileStream);
                }
                string path = "Images\\ImageRawMaterial\\";
                savedFileName = path + uniqueFileName;
            }



            var rawMaterial = _mapper.Map<RawMaterial>(createDto);
       
            rawMaterial.PictureUrl = savedFileName; 

            _context.RawMaterial.Add(rawMaterial);
            await _context.SaveChangesAsync();
            var dtoToReturn = _mapper.Map<RawMaterialDtos>(rawMaterial);
            return CreatedAtAction(nameof(GetRawMaterialById), new { id = dtoToReturn.Id }, dtoToReturn);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while creating the raw material: {ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRawMaterial(int id, [FromBody] RawMaterialDtos updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid raw material update data provided."));

        var existingRawMaterial = await _context.RawMaterial.FindAsync(id);
        if (existingRawMaterial == null)
            return NotFound(new ApiErrorResponse(404, $"Raw material with ID {id} not found."));

        try
        {
            _mapper.Map(updateDto, existingRawMaterial); // Update the existing entity
            _context.Entry(existingRawMaterial).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while updating raw material with ID {id}: {ex.Message}"));
        }
    }
}
