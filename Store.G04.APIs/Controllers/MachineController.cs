using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.G04.APIs.Attributes;
using Store.G04.APIs.Errors;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;
using Store.G04.Core.Helper;
using Store.G04.Core.Services.Contract;
using Store.G04.Core.Specifications.Machinee;
using Store.G04.Repositpory.Data.Contexts;
using System;

namespace Store.G04.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MachineController : BaseApiController
{
    private readonly IMachinesService _machinesService;
    private readonly IMapper _mapper;
    private readonly StoreDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;
    public MachineController(IMachinesService machinesService, IMapper mapper, StoreDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _machinesService = machinesService;
        _mapper = mapper;
        _context = context;
        _hostEnvironment = hostEnvironment;
    }
    [HttpGet("all")] 
    [ProducesResponseType(typeof(IEnumerable<MachineDtos>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MachineDtos>>> GetAllMachinesForDropdown()
    {
        var machines = await _context.Machine.Select(m => _mapper.Map<MachineDtos>(m)).ToListAsync();
        return Ok(machines);
    }

    [HttpGet]
    [Cached(100)]
    [ProducesResponseType(typeof(PaginationResponse<MachineDtos>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MachineDtos>>> GetAllMachines([FromQuery] MachineeParams machineeSpec)
    {
        var machines = await _machinesService.GetAllMachinesAsync(machineeSpec);
        return Ok(machines);
    }

    [HttpGet("GetMachines")]
    [ProducesResponseType(typeof(PaginationResponse<MachineDtos>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MachineDtos>>> GetMachines([FromQuery] MachineeParams machineeSpec)
    {
        var machines = await _machinesService.GetAllMachinesAsync(machineeSpec);
        return Ok(machines);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MachineDtos), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MachineDtos>> GetMachineById(int? id)
    {
        if (id == null)
            return BadRequest(new ApiErrorResponse(400, "The machine ID cannot be null."));

        try
        {
            var result = await _machinesService.GetMachineByIdAsync(id.Value);

            if (result == null)
                return NotFound(new ApiErrorResponse(404, $"The machine with ID {id} was not found."));

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while fetching the machine with ID {id}: {ex.Message}"));
        }
    }

    [HttpPost("Book")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookingMachineDto>> CreateBooking(BookingMachineDto createBookingDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid booking data provided."));

        try
        {
            var booking = _mapper.Map<BookingMachine>(createBookingDto);
            _context.BookingMachine.Add(booking); 
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, _mapper.Map<BookingMachineDto>(booking)); // Map back to DTO

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
    public async Task<ActionResult<BookingMachineDto>> GetBookingById(int id)
    {
        try
        {
            var booking = await _context.BookingMachine.FindAsync(id);

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

    [HttpPost("CreateMachine")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MachineDtos>> CreateMachine([FromForm] MachineWithImageDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiErrorResponse(400, "Invalid machine data provided."));

        try
        {
            string? savedFileName = null;
            if (createDto.ImageFile != null && createDto.ImageFile.Length > 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string uploadDir = Path.Combine(wwwRootPath, "Images", "ImageMachine"); // Create a new folder for machine images
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
                string path = "Images\\ImageMachine\\";
                savedFileName = path + uniqueFileName;
            }

            var machine = _mapper.Map<MachineEntity>(createDto);
            machine.PictureUrl = savedFileName;

            _context.Machine.Add(machine);
            await _context.SaveChangesAsync();
            var dtoToReturn = _mapper.Map<MachineDtos>(machine);
            return CreatedAtAction(nameof(GetMachineById), new { id = dtoToReturn.Id }, dtoToReturn);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(500, $"An error occurred while creating the machine: {ex.Message}"));
        }
    }
}
