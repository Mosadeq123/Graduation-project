using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.G04.APIs.Errors;
using Store.G04.Core.Dtos.Wishlist;
using Store.G04.Core.Entities;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Repositpory.Data.Contexts;
using System.Linq;

namespace Store.G04.APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : BaseApiController
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IMapper _mapper;
    private readonly StoreDbContext _context;
    private readonly IConfiguration _configuration;

    public WishlistController(IWishlistRepository wishlistRepository, IMapper mapper, StoreDbContext context, IConfiguration configuration)
    {
        _wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("AddWishlist")]
    public async Task<ActionResult<CustomerWishlist>> CreateOrUpdateWishlist([FromBody] customerWishlistDto model)
    {
        if (model == null)
        {
            return BadRequest(new ApiErrorResponse(400, "Invalid data"));
        }

        var wishlistEntity = _mapper.Map<CustomerWishlist>(model);
        var wishlist = await _wishlistRepository.UpdateWishlistAsync(wishlistEntity);

        if (wishlist == null)
        {
            return BadRequest(new ApiErrorResponse(400, "Failed to update or create wishlist"));
        }

        return Ok(wishlist);
    }

    [HttpDelete("RemoveWishlist/{id}")]
    public async Task<IActionResult> DeleteWishlist(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new ApiErrorResponse(400, "Invalid Id"));
        }

        var wishlist = await _wishlistRepository.GetWishlistAsync(id);
        if (wishlist == null)
        {
            return NotFound(new ApiErrorResponse(404, "Wishlist not found"));
        }

        await _wishlistRepository.DeleteWishlistAsync(id);
        return NoContent();
    }

    [HttpGet("GetWishlist")]
    public async Task<ActionResult<CustomerWishlist>> GetWishlist(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new ApiErrorResponse(400, "Invalid Id"));
        }

        var wishlist = await _wishlistRepository.GetWishlistAsync(id);
        if (wishlist == null)
        {
            return NotFound(new ApiErrorResponse(404, "Wishlist not found"));
        }

        return Ok(wishlist);
    }
    [HttpPost("AddWish")]
    public async Task<IActionResult> AddWishlist([FromBody] WishlistDto wishlistDto)
    {
        var wishlist = new Wishlist
        {
            UserId = wishlistDto.UserId,
            MaterialId = wishlistDto.MaterialId,
            MachineId = wishlistDto.MachineId
        };
        _context.Wishlists.Add(wishlist);
        await _context.SaveChangesAsync();
        return Ok(wishlist);
    }

    [HttpDelete("RemoveWish")]
    public async Task<IActionResult> RemoveWishlist([FromBody] WishlistDto wishlistDto)
    {
        // Determine whether to check MaterialId or MachineId
        IQueryable<Wishlist> query = _context.Wishlists.Where(x => x.UserId == wishlistDto.UserId);

        if (wishlistDto.MaterialId.HasValue)
        {
            query = query.Where(x => x.MaterialId == wishlistDto.MaterialId.Value);
        }
        else if (wishlistDto.MachineId.HasValue)
        {
            query = query.Where(x => x.MachineId == wishlistDto.MachineId.Value);
        }
        else
        {
            return BadRequest("Invalid request, missing MaterialId or MachineId.");
        }

        var wishlistItem = await query.FirstOrDefaultAsync();

        if (wishlistItem == null)
        {
            return NotFound();
        }

        _context.Wishlists.Remove(wishlistItem);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("GetWish")]
    public IActionResult GetWish([FromQuery] string id)
    {
        var wishlist = _context.Wishlists
            .Where(w => w.UserId == id)
            .Include(w => w.Material)
            .Include(w => w.Machine)
            .ToList();

        var result = wishlist.Select(w => new
        {
            w.Id,
            w.UserId,
            Material = w.Material != null ? new
            {
                w.Material.NameMaterial,
                w.Material.Description,
                w.Material.Quantity,
                w.Material.StitchLength,
                w.Material.YarnType,
                PictureUrl = $"{_configuration["BaseURL"]}{w.Material.PictureUrl}",
                w.Material.Id
            } : null,
            Machine = w.Machine != null ? new
            {
                w.Machine.NameMachine,
                w.Machine.Description,
                w.Machine.NeedlesCount,
                w.Machine.MachineType,
                w.Machine.Softness,
                w.Machine.Width,
                PictureUrl = $"{_configuration["BaseURL"]}{w.Machine.PictureUrl}",
                w.Machine.Id
            } : null
        }).ToList();

        return Ok(result);
    }
    [HttpGet("GetAllWish")]
    public IActionResult GetAllWish([FromQuery] string id)
    {
        var wishlist = _context.Wishlists
            .Where(w => w.UserId == id)
            .ToList();

        return Ok(wishlist);
    }
}
