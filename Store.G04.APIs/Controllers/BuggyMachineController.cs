using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.Repositpory.Data.Contexts;

namespace Store.G04.APIs.Controllers
{
    public class BuggyMachineController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyMachineController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet("NotFound")] //GET: /api/Buggy/NotFound
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var machine = await _context.RawMaterial.FindAsync(100);
            if (machine is null)
                return NotFound(new ApiErrorResponse(404));
            return Ok(machine);

        }


        [HttpGet("ServerError")] //GET: /api/Buggy/ServerError
        public async Task<IActionResult> GetServerError()
        {
            var machine = await _context.RawMaterial.FindAsync(100);
            var machineToString = machine.ToString(); // Will Throw Exception (NullReferenceException)
            return Ok(machine);

        }

        [HttpGet("BadRequest")] //GET: /api/Buggy/BadRequest
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("BadRequest/{id}")] //GET: /api/Buggy/BadRequest/{id}
        public async Task<IActionResult> GetBadRequestError(int id) // Validation Error
        {
            return Ok();
        }

        [HttpGet("UnAuthorized")] //GET: /api/Buggy/UnAithorized
        public async Task<IActionResult> GetUnauthorizedError(int id) // Validation Error
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}
